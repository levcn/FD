using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using Levcn.Framework.Web.Downloader;


namespace SLControls.Download
{
    /// <summary> 
    /// 支持断点续传多线程下载的类 
    /// </summary> 
    public class HttpWebClient
    {
        #region Delegates

        public delegate void DataReceiveEventHandler(HttpWebClient sender, DownLoadEventArgs e);

        public delegate void ExceptionEventHandler(HttpWebClient sender, ExceptionEventArgs e);

        public delegate void ThreadProcessEventHandler(HttpWebClient sender, ThreadProcessEventArgs e);

        #endregion

        private int _fileLength; //下载文件的总大小
        private Uri _mBaseAddress;
//        private ICredentials _mCredentials = CredentialCache.DefaultCredentials;
//        private NameValueCollection _mRequestParameters;

        public int FileLength
        {
            get { return _fileLength; }
        }

//        public ICredentials Credentials
//        {
//            get { return _mCredentials; }
//            set { _mCredentials = value; }
//        }
//
//        public NameValueCollection QueryString
//        {
//            get { return _mRequestParameters ?? (_mRequestParameters = new NameValueCollection()); }
//            set { _mRequestParameters = value; }
//        }

        public string BaseAddress
        {
            get
            {
                if (_mBaseAddress != null)
                {
                    return _mBaseAddress.ToString();
                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _mBaseAddress = null;
                }
                else
                {
                    try
                    {
                        _mBaseAddress = new Uri(value);
                    }
                    catch (Exception exception1)
                    {
                        throw new ArgumentException("value", exception1);
                    }
                }
            }
        }

        public event DataReceiveEventHandler DataReceive; //接收字节数据事件
        public event ExceptionEventHandler ExceptionOccurrs; //发生异常事件
        public event ThreadProcessEventHandler ThreadProcessEnd; //发生多线程处理完毕事件

        ///// <summary> 
        ///// 分块下载文件 
        ///// </summary> 
        ///// <param name="address">URL 地址</param> 
        ///// <param name="fileName">保存到本地的路径文件名</param> 
        ///// <param name="chunksCount">块数,线程数</param> 
        //public void DownloadFile(string address, string fileName, int chunksCount)
        //{
        //    int p = 0; // position 
        //    int s = 0; // chunk size 
        //    string a = null;
        //    HttpWebResponse hwrp = null;
        //    try
        //    {
        //        HttpWebRequest hwrq = (HttpWebRequest)WebRequest.Create(GetUri(address));
        //        hwrp = (HttpWebResponse)hwrq.GetResponse();
        //        long L = hwrp.ContentLength;
        //        hwrq.Credentials = _mCredentials;
        //        L = ((L == -1) || (L > 0x7fffffff)) ? (0x7fffffff) : L;
        //        //Int32.MaxValue 该常数的值为 2,147,483,647; 即十六进制的 0x7FFFFFFF
        //        var l = (int)L;
        //        _fileLength = l;
        //        // 在本地预定空间(竟然在多线程下不用先预定空间) 
        //        // FileStream sw = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite); 
        //        // sw.Write(new byte[l], 0, l); 
        //        // sw.Close(); 
        //        // sw = null;
        //        bool b = (hwrp.Headers["Accept-Ranges"] != null & hwrp.Headers["Accept-Ranges"] == "bytes");
        //        a = hwrp.Headers["Content-Disposition"]; //attachment 
        //        a = a != null ? a.Substring(a.LastIndexOf("filename=") + 9) : fileName;
        //        int ss = s;
        //        if (b)
        //        {
        //            s = l / chunksCount;
        //            if (s < 2 * 64 * 1024) //块大小至少为 128 K 字节 
        //            {
        //                s = 2 * 64 * 1024;
        //            }
        //            ss = s;
        //            int i = 0;
        //            while (l > s)
        //            {
        //                l -= s;
        //                if (l < s)
        //                {
        //                    s += l;
        //                }
        //                if (i++ > 0)
        //                {
        //                    var x = new DownLoadState(address, hwrp.ResponseUri.AbsolutePath, fileName, a, p, s,
        //                                              DownloadFileChunk) {HttpWebClient = this};
        //                    // 单线程下载 
        //                    // x.StartDownloadFileChunk();
        //                    //多线程下载 
        //                    var t = new Thread(x.StartDownloadFileChunk);
        //                    //this.OnThreadProcess(t); 
        //                    t.Start();
        //                }
        //                p += s;
        //            }
        //            s = ss;
        //            ResponseAsBytes(address, hwrp, s, fileName);
        //            OnThreadProcess(Thread.CurrentThread);
        //            // lock (_SyncLockObject) 
        //            // { 
        //            // this._Bytes += buffer.Length; 
        //            // } 
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ExceptionActions ea = ExceptionActions.Throw;
        //        if (ExceptionOccurrs != null)
        //        {
        //            var x = new DownLoadState(address, hwrp.ResponseUri.AbsolutePath, fileName, a, p, s);
        //            var eea = new ExceptionEventArgs(e, x);
        //            ExceptionOccurrs(this, eea);
        //            ea = eea.ExceptionAction;
        //        }
        //        if (ea == ExceptionActions.Throw)
        //        {
        //            if (!(e is WebException) && !(e is SecurityException))
        //            {
        //                throw new WebException("net_webclient", e);
        //            }
        //            throw;
        //        }
        //    }
        //}

        internal void OnThreadProcess(Thread t)
        {
            if (ThreadProcessEnd != null)
            {
                var tpea = new ThreadProcessEventArgs(t);
                ThreadProcessEnd(this, tpea);
            }
        }
        long totalLength = 0;
        private long startPosition = 0;
        /// <summary> 
        /// 下载一个文件块,利用该方法可自行实现多线程断点续传 
        /// </summary> 
        /// <param name="address">URL 地址</param> 
        /// <param name="fileName">保存到本地的路径文件名</param>
        /// <param name="fromPosition"></param>
        /// <param name="length">块大小</param> 
        public void DownloadFileChunk(string address, string fileName)
        {
            HttpWebResponse hwrp = null;
            string a = null;
            long fromPosition = 0;
            
            try
            {
                FileInfo fi = new FileInfo(fileName);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                using (var sw = new BinaryWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    sw.BaseStream.Position = sw.BaseStream.Length;
                    fromPosition = sw.BaseStream.Position;
                    startPosition = fromPosition;
                    //this._FileName = FileName; 
                    var hwrq = (HttpWebRequest) WebRequest.Create(GetUri(address));
                    //hwrq.Credentials = this.m_credentials; 
                    hwrq.AddRange(fromPosition);
                    hwrp = (HttpWebResponse) hwrq.GetResponse();
                    a = hwrp.Headers["Content-Disposition"]; //attachment 
                    var hhhhh = hwrp.Headers.AllKeys.Select(k => new {Key = k, Value = hwrp.Headers[k]}).ToList();
                    string sdfwerwer = hwrp.ToString();
                    totalLength = hwrp.ContentLength + fromPosition;
                    if (a != null)
                    {
                        a = a.Substring(a.LastIndexOf("filename=") + 9);
                    }
                    else
                    {
                        a = fileName;
                    }


                    ResponseAsBytes(address
                                    , hwrp
                                    , fileName
                                    , (count, buffer) =>
                                          {
                                              if (count > 0)
                                              {
                                                  sw.Write(buffer);
                                                  sw.Flush();
                                                  //Thread.Sleep(50);
                                              }
                                              return (int) sw.BaseStream.Position;
                                          });
                    sw.Close();
                }
                // lock (_SyncLockObject) 
                // { 
                // this._Bytes += buffer.Length; 
                // } 
            }
            catch (Exception e)
            {
                ExceptionActions ea = ExceptionActions.Throw;
                if (ExceptionOccurrs != null)
                {
                    string url = "";
                    if(hwrp!=null)
                    {
                        url = hwrp.ResponseUri.AbsolutePath;
                    }
                    var x = new DownLoadState(address, url, fileName, a, (int)fromPosition, 0, (int)totalLength, (int)startPosition);
                    var eea = new ExceptionEventArgs(e, x);
                    ExceptionOccurrs(this, eea);
                    ea = eea.ExceptionAction;
                }
                if (ea == ExceptionActions.Throw)
                {
                    if (!(e is WebException) && !(e is SecurityException))
                    {
                        throw new WebException("net_webclient", e);
                    }
                    //throw;
                }
            }
        }

        internal void ResponseAsBytes(string requestURL, WebResponse response, string fileName,Func<int,byte[],int> onReaded)
        {
            string a = null; //AttachmentName 
            int P = 0; //整个文件的位置指针 
            int readedCount = 0;
            try
            {
                a = response.Headers["Content-Disposition"]; //attachment 
                if (a != null)
                {
                    a = a.Substring(a.LastIndexOf("filename=") + 9);
                }
                bool flag1 = false;
                //var buffer1 = new byte[(int)num1];
                var buffer1 = new byte[100*1024];//100K

                int p = 0; //本块的位置指针
                string s = response.Headers["Content-Range"];
                if (s != null)
                {
                    s = s.Replace("bytes ", "");
                    s = s.Substring(0, s.IndexOf("-"));
                    P = Convert.ToInt32(s);
                }
                int num3 = 0;
                Stream S = response.GetResponseStream();
                Type t = S.GetType();
                var peof = t.GetProperty("Eof", BindingFlags.Instance | BindingFlags.NonPublic);
                do
                {
                    readedCount = S.Read(buffer1, 0,buffer1.Length);
                    if (readedCount > 0)//读到数据
                    {
                            var buffer = new byte[readedCount];
                            Buffer.BlockCopy(buffer1, p, buffer, 0, buffer.Length);
                            
                            int position = onReaded(readedCount, buffer);

                            //触发事件 
                            var dls = new DownLoadState(requestURL, response.ResponseUri.AbsolutePath, fileName, a,position,
                                                        readedCount, buffer, (int)totalLength, (int)startPosition);
                            var dlea = new DownLoadEventArgs(dls);
                            OnDataReceive(dlea);
                            //System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        var eof = (bool)peof.GetValue(S,null);
                        if (eof)
                        {
                            break;
                        }
                    }
                } while (readedCount != 0);
                S.Close();
            }
            catch (Exception e)
            {
                ExceptionActions ea = ExceptionActions.Throw;
                if (ExceptionOccurrs != null)
                {
                    var x = new DownLoadState(requestURL, response.ResponseUri.AbsolutePath, fileName, a, P, readedCount, (int)totalLength, (int)startPosition);
                    var eea = new ExceptionEventArgs(e, x);
                    ExceptionOccurrs(this, eea);
                    ea = eea.ExceptionAction;
                }
                if (ea == ExceptionActions.Throw)
                {
                    if (!(e is WebException) && !(e is SecurityException))
                    {
                        throw new WebException("net_webclient", e);
                    }
                    throw;
                }
                //return null;
            }
        }

        private void OnDataReceive(DownLoadEventArgs e)
        {
            if (DataReceive != null)
            {
                //触发数据到达事件 
                DataReceive(this, e);
            }
        }

        Encoding ASCII
        {
            get
            {
                return Encoding.GetEncoding("");
            }
        }
        public byte[] UploadFile(string address, string fileName)
        {
            return UploadFile(address, "POST", fileName, "file");
        }

        public string UploadFileEx(string address, string method, string fileName, string fieldName)
        {
            return Encoding.GetEncoding("ASCII").GetString(UploadFile(address, method, fileName, fieldName));
        }

        public byte[] UploadFile(string address, string method, string fileName, string fieldName)
        {
            byte[] buffer4;
            FileStream stream1 = null;
            try
            {
                fileName = Path.GetFullPath(fileName);
                string text1 = "---------------------" + DateTime.Now.Ticks.ToString("x");
                const string text2 = "application/octet-stream";
                stream1 = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                WebRequest request1 = WebRequest.Create(GetUri(address));
//                request1.Credentials = _mCredentials;
                request1.ContentType = "multipart/form-data; boundary=" + text1;
                request1.Method = method;
                var textArray1 = new string[7]
                                     {
                                         "--", text1,
                                         "\r\nContent-Disposition: form-data; name=\"" + fieldName + "\"; filename=\"",
                                         Path.GetFileName(fileName), "\"\r\nContent-Type: ", text2, "\r\n\r\n"
                                     };
                string text3 = string.Concat(textArray1);
                byte[] buffer1 = Encoding.UTF8.GetBytes(text3);
                byte[] buffer2 = Encoding.ASCII.GetBytes("\r\n--" + text1 + "\r\n");
                long num1 = 0x7fffffffffffffff;
                try
                {
                    num1 = stream1.Length;
                    request1.ContentLength = (num1 + buffer1.Length) + buffer2.Length;
                }
                catch
                {
                }
                var buffer3 = new byte[Math.Min(0x2000, (int)num1)];
                using (Stream stream2 = request1.GetRequestStream())
                {
                    int num2;
                    stream2.Write(buffer1, 0, buffer1.Length);
                    do
                    {
                        num2 = stream1.Read(buffer3, 0, buffer3.Length);
                        if (num2 != 0)
                        {
                            stream2.Write(buffer3, 0, num2);
                        }
                    } while (num2 != 0);
                    stream2.Write(buffer2, 0, buffer2.Length);
                }
                stream1.Close();
                stream1 = null;
                WebResponse response1 = request1.GetResponse();
                buffer4 = ResponseAsBytes(response1);
            }
            catch (Exception exception1)
            {
                if (stream1 != null)
                {
                    stream1.Close();
                    stream1 = null;
                }
                if (!(exception1 is WebException) && !(exception1 is SecurityException))
                {
                    //throw new WebException(SR.GetString("net_webclient"), exception1); 
                    throw new WebException("net_webclient", exception1);
                }
                throw;
            }
            return buffer4;
        }

        private byte[] ResponseAsBytes(WebResponse response)
        {
            int num2;
            long num1 = response.ContentLength;
            bool flag1 = false;
            if (num1 == -1)
            {
                flag1 = true;
                num1 = 0x10000;
            }
            var buffer1 = new byte[(int)num1];
            Stream stream1 = response.GetResponseStream();
            int num3 = 0;
            do
            {
                num2 = stream1.Read(buffer1, num3, ((int)num1) - num3);
                num3 += num2;
                if (flag1 && (num3 == num1))
                {
                    num1 += 0x10000;
                    var buffer2 = new byte[(int)num1];
                    Buffer.BlockCopy(buffer1, 0, buffer2, 0, num3);
                    buffer1 = buffer2;
                }
            } while (num2 != 0);
            stream1.Close();
            if (flag1)
            {
                var buffer3 = new byte[num3];
                Buffer.BlockCopy(buffer1, 0, buffer3, 0, num3);
                buffer1 = buffer3;
            }
            return buffer1;
        }

        private Uri GetUri(string path)
        {
            Uri uri1;
            try
            {
                if (_mBaseAddress != null)
                {
                    uri1 = new Uri(_mBaseAddress, path);
                }
                else
                {
                    uri1 = new Uri(path);
                }
                if (_mRequestParameters == null)
                {
                    return uri1;
                }
                var builder1 = new StringBuilder();
                string text1 = string.Empty;
                for (int num1 = 0; num1 < _mRequestParameters.Count; num1++)
                {
                    builder1.Append(text1 + _mRequestParameters.AllKeys[num1] + "=" + _mRequestParameters[num1]);
                    text1 = "&";
                }
                var builder2 = new UriBuilder(uri1);
                builder2.Query = builder1.ToString();
                uri1 = builder2.Uri;
            }
            catch (UriFormatException)
            {
                uri1 = new Uri(Path.GetFullPath(path));
            }
            return uri1;
        }
    }
}
