using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Extends;


namespace SLControls.Download
{
    public class FileDownload
    {
        public FileDownload(string filePath, FileDownloadMap map, string url, string urlPath, Stream stream)
        {

            FilePath = filePath;
            Map = map;
            Url = url;
            UrlPath = urlPath;
            fileStream = stream;
        }
        /// <summary>
        /// 返回服务器上的文件内容
        /// Resourse/aa.docx
        /// </summary>
        /// <param name="url">course/a.txt</param>
        /// <param name="readResult"></param>
        public static void ReadServerFile(string url, string path, long start, long end, Action<Stream> readResult)
        {
            //            url = url.TrimEnd('&');
            //            var index = url.IndexOf("?");
            //            if (index == -1) url += "?";
            //            var id = string.Format("sIDdctxzm0p={0}", Guid.NewGuid());
            //            url += id;
            //            HttpWebRequest client = (HttpWebRequest)WebRequest.Create(url);
            //            client.Headers["Range"] = string.Format("bytes={0}-{1}", start, end);
            //            IAsyncResult r = null;
            //            r = client.BeginGetResponse(new AsyncCallback(w =>
            //            {
            //                var rs = client.EndGetResponse(r);
            //                var stream = rs.GetResponseStream();
            //                readResult(stream);
            //            }), null);

            WebClient client = new WebClient();
            //                        if (start != 0)
            //                        {
            //                            if(end ==0)
            //                            client.Headers["Range"] = string.Format("bytes={0}-", start);
            //                            else
            //                            client.Headers["Range"] = string.Format("bytes={0}-{1}", start, end);
            //                        }
            //                        else
            //                        {
            //                            if (end != 0)
            //                            {
            //                                client.Headers["Range"] = string.Format("bytes=-{0}", end);
            //                            }
            //                        }

            //            request.
            //                        client.Headers["Range"] = string.Format("bytes=10000-", end);
            //                        client.Headers["Range"] = string.Format("bytes={0}-{1}", 5000, 6000);
            //                        var ere = client.Headers.AllKeys.Select(w => string.Format("{0}:{1}", w, client.Headers[w])).Serialize("\n");

            url = url.TrimEnd('&');
            var index = url.IndexOf("?");
            if (index == -1) url += "?";
            var id = string.Format("sIDdctxzm0p={0}", Guid.NewGuid());
            url += id;
            url = string.Format("{0}&type=1&p={1}&e={2}&s={3}", url, path, end, start);
            var address = new Uri(url);
            client.OpenReadCompleted +=
                    (s, e) =>
                    {
                        if (e.Error == null)
                        {
                            readResult(e.Result);
                        }
                        else
                        {
                            throw e.Error;
                        }
                    };
            client.OpenReadAsync(address);

        }
        /// <summary>
        /// 返回服务器上的文件内容
        /// Resourse/aa.docx
        /// </summary>
        /// <param name="url">course/a.txt</param>
        /// <param name="readResult"></param>
        void ReadServerFileSize(string url, string path, Action<long> readResult)
        {
            url = url.TrimEnd('&');
            var index = url.IndexOf("?");
            if (index == -1) url += "?";
            var id = string.Format("sIDdctxzm0p={0}", Guid.NewGuid());
            url += id;
            url = string.Format("{0}&type=2&p={1}", url, path);
            WebClient client = new WebClient();
            var address = new Uri(url);
            client.OpenReadCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    MemoryStream ms = new MemoryStream();
                    e.Result.CopyTo(ms);
                    var re = ms.ToArray().ToStr().ToLong();
                    FileLength = re;
                    e.Result.Close();
                    readResult(re);
                }
                else
                {
                    throw e.Error;
                }
            };
            //            client.DownloadProgressChanged += (s, e1) =>
            //            {
            //                var t = e1.TotalBytesToReceive;
            //                client.CancelAsync();
            //                if(!readed)readResult(t);
            //                readed = true;
            //            };
            //            client.OpenReadCompleted +=
            //                    (s, e1) =>
            //                    {
            //                        if (e1.Error == null)
            //                        {
            //                            var l = e1.Result.Length;
            //                            e1.Result.Close();
            //                            readResult(l);
            //                        }
            //                        else
            //                        {
            //                            readResult(-1);
            //                        }
            //                    };
            client.OpenReadAsync(address);
        }


        public string FilePath;
        public string Url;
        public string UrlPath;
        public long FileLength;
        public FileDownloadMap Map;
        private bool closed = false;

        private Stream fileStream;

        public void Close()
        {
            if (FileStream != null) FileStream.Close();
        }
        Stream FileStream
        {
            get
            {
                return fileStream;
                //                if (fileStream == null)
                //                {
                //                    fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write);
                //                    if (fileStream.Length == 0)
                //                    {
                //                        fileStream.SetLength(FileLength);
                //                    }
                //                }
                //                return fileStream;
            }
        }

        public void ReadFileLength()
        {
            ReadServerFileSize(Url, UrlPath, OnReadedLength);
        }
        public long CurrentDownloadPostion()
        {
            return fileStream.Position;
        }

        private Guid downloadThread;
        public bool StartDownload(long position, Action downloadedAction = null)
        {
            if (state == "STOP") return false;
            //            var dfer = position / (5000*1024);
            downloadThread = Guid.NewGuid();
            Guid cu = downloadThread;
            closed = true;
            //            lock (this)
            {
                closed = false;
                var section = Map.GetLastNoDownloadSection(position);
                if (section != null)
                {
                    ReadServerFile(Url, UrlPath, section.Start, section.End, s =>
                    {
                        bool sectionDownload = false;
                        long currentPostion = section.Start;
                        //                        lock (this)
                        {
                            byte[] buffer = new byte[1024 * 10];

                            var fs = FileStream;
                            while (!closed)
                            {
                                int readCount = s.Read(buffer, 0, buffer.Length);
                                if (readCount == 0)
                                {
                                    sectionDownload = true;
                                    break;
                                }
                                Map.AddDownloadSection(currentPostion, currentPostion + readCount - 1);
                                lock (fs)
                                {
                                    if (fs.CanWrite && !closed)
                                    {
                                        fs.Seek(currentPostion, SeekOrigin.Begin);
                                        fs.Write(buffer, 0, readCount);
                                    }
                                }
                                currentPostion += readCount;
                            }
                            s.Close();
                            if (!closed)
                            {
                                fs.Flush();
                            }
                        }
                        if (sectionDownload)
                        {
                            if (downloadedAction != null) downloadedAction();
                            if (cu == downloadThread) OnSectionDownloaded(currentPostion);
                            else
                            {

                            }
                            //                            FileStream.Close();

                        }
                    });
                    return true;
                }
                else
                {
                    if (position == 0)
                    {
                        //                        FileStream.Close();
                    }
                    return false;
                }
            }
        }

        public event TEventHandler<long> SectionDownloaded;
        public event TEventHandler<long> ReadedLength;

        protected virtual void OnReadedLength(long args)
        {
            var handler = ReadedLength;
            if (handler != null) handler(args);
        }

        protected virtual void OnSectionDownloaded(long currentPostion)
        {

            var handler = SectionDownloaded;
            if (handler != null) handler(currentPostion);
        }

        private string state = "DOWNLOAD";
        internal void Stop()
        {
            state = "STOP";
            closed = true;
        }
    }
    /// <summary>
    /// 文件下载的进度列表
    /// </summary>
    public class FileDownloadMap
    {
        public long Length;
        public string FilePath;

        public List<Section> DownloadSection = new List<Section>();


        /// <summary>
        /// 从index开始,查找后面没有下载的区间
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Section GetLastNoDownloadSection(long index)
        {
            lock (this)
            {
                Section re = null;
                var s = DownloadSection.FirstOrDefault(z => z.Start <= index && z.End >= index);
                if (s != null)
                {
                    index = s.End + 1;
                }
                var nextSection = DownloadSection.Where(z => z.Start > index).OrderBy(w => w.Start).FirstOrDefault();
                if (nextSection == null)
                {
                    //                var end = DownloadSection.Count>0 ?DownloadSection.Max(w => w.End):0;
                    var end = Length;
                    if (index < Length)
                    {
                        re = new Section(index, end);
                    }
                    else
                    {
                        return null; //如果 为空,说明从index开始,后面的数据都已经下载完成
                    }
                }
                if (nextSection != null)
                {
                    re = new Section(index, nextSection.Start);
                }
                var maxSize = 2000 * 1024;
                if (re != null)
                {

                    if (re.End - re.Start > maxSize)
                    {
                        re.End = re.Start + maxSize;
                    }
                }
                var startindex = re.Start / maxSize;
                var endindex = re.End / maxSize;
                return re;
            }
        }

        /// <summary>
        /// 添加一个已经下载的区间
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void AddDownloadSection(long start, long end)
        {
            lock (this)
            {

                var section = DownloadSection.FirstOrDefault(z => z.End + 1 == start); //拼接到一个区间后面
                if (section != null)
                {
                    section.End = end;
                    var nextSection = DownloadSection.FirstOrDefault(z => z.Start == end + 1); //如果接到一个区间后面之后,正好和下面的区间相连,则合并区间
                    if (nextSection != null)
                    {
                        nextSection.Start = section.Start;
                        DownloadSection.Remove(section);
                    }
                }
                else
                {
                    section = DownloadSection.FirstOrDefault(z => z.Start == end + 1); //拼接到一个区间前面
                    if (section != null)
                    {
                        section.Start = start;
                    }
                    else
                    {
                        DownloadSection.Add(new Section(start, end));
                    }
                }
            }
        }

        /// <summary>
        /// 返回这个区域是否下载完成
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal bool Downloaded(long start, int end)
        {
            lock (this)
            {
                return null != DownloadSection.ToList().FirstOrDefault(w => w.Start <= start && w.End >= end);
            }

        }
    }
    public class Section
    {
        public long Start;
        public long End;

        public Section()
        {

        }
        public Section(long start, long end)
        {
            Start = start;
            End = end;
        }
    }
}
