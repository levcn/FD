using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Markup;
//using StaffTrain.FwClass.DataClientTools.Configs;
using StaffTrain.FwClass.Serializer;
using STComponse.CFG;


namespace StaffTrain.FwClass.DataClientTools
{
    public static class DataAccess
    {


        private static Uri baseuri;
        /// <summary>
        /// 返回当前站点的根地址
        /// <code>
        /// http://127.0.0.1/
        /// </code>
        /// http://127.0.0.1/site1/
        /// </summary>
        public static Uri BaseUri
        {
            get
            {
                if (baseuri == null)
                {
                    WebClient wc = new WebClient();
                    var url1 = new Uri(wc.BaseAddress);
                    var index = url1.ToString().ToLower().LastIndexOf("clientbin/");
                    if (index > 0)
                    {
                        var d = url1.ToString();
                        var host = d.Substring(0, index);
                        baseuri = new Uri(host);
                    }
                    else
                    {
                        var host = wc.BaseAddress.Replace(url1.AbsolutePath, "");
                        baseuri = new Uri(host);
                    }
                    //                    MessageBox.Show(baseuri.ToString());
                    //                    var str = HtmlPage.Document.DocumentUri.ToString();
                    //                    if (!string.IsNullOrEmpty(HtmlPage.Document.DocumentUri.AbsolutePath) &&
                    //                        HtmlPage.Document.DocumentUri.AbsolutePath.Length > 1)
                    //                    {
                    //                        if (!str.EndsWith("/"))
                    //                        {
                    //                            var index = str.LastIndexOf("/");
                    //                            str = str.Substring(0, index);
                    ////                            str = str.Replace(HtmlPage.Document.DocumentUri.AbsolutePath, "");
                    //                        }
                    //                        else
                    //                        {
                    //                            str = str.Substring(0, str.Length - 1);
                    //                        }
                    //                    }
                    //                    if (!str.EndsWith("/"))
                    //                    {
                    //                        str +="/";
                    //                    }
                    //                    baseuri = new Uri(str);
                }
                return baseuri;
            }
        }


        public static async Task<ResultData> ActionRequest(Uri uri, ActionCommand ac,  Action<ResultData> exception = null)
        {
            PostArgs pa = new PostArgs();
            pa["ActionCmd"] = JsonHelper.JsonSerializer(ac);
            ActionConfig acb = new ActionConfig { Exception = exception};
            var t = await ActionRequest(uri, pa, acb);
            return t;
        }

        public static async Task<ResultData> ActionRequest(Uri uri, List<ActionCommand> ac, ActionConfig acb)
        {
            PostArgs pa = new PostArgs();
            pa["ActionCmd"] = JsonHelper.JsonSerializer(ac);
            try
            {
                return await ActionRequest(uri, pa, acb);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        public static async Task<ResultData> ActionRequest(Uri uri, ActionCommand ac, ActionConfig acb)
        {
            return await ActionRequest(uri, new List<ActionCommand> { ac }, acb);
        }

        public async static Task<PostResult> ActionRequestList(Uri uri, PostArgs pa, ActionConfig acb)
        {
            var w = await PostRequest(uri, pa);
            return new PostResult(w);

//            var pr = new PostResult<string>(w);
//            if (pr.ResultData == null) return pr;
//            if (pr.ResultData.HaveError)
//            {
//                throw new Exception(pr.ResultData.ErrorMsg + pr.ResultData.DetailErrorMsg);
//                if (acb.Exception != null) acb.Exception(pr.ResultData);
//            }
//            else
//            {
//                return pr;
//                //                    acb.PostBackMethod(pr);
//
//            }
        }
        public async static Task<ResultData> ActionRequest(Uri uri, PostArgs pa, ActionConfig acb)
        {
            var list = await ActionRequestList(uri, pa, acb);

            return list.ResultData[0];
            {
//                DateTime start = DateTime.Now;
//                    var w = await PostRequest(uri, pa, acb.Waitting, acb.WaittingConfig);

                //                    var end = DateTime.Now;
                //                    var totalMS = (end - start).TotalMilliseconds;
                //                    Debug.WriteLine("服务器执行完成:{0}ms", totalMS);
                //                    var dfrrr = pa;
                //                    if (totalMS > 500)
                //                    {}
                //                    if (w.Error != null)
                //                    {
                //                        throw w.Error;
                //                    }
//                var pr = new PostResult(w);
//                return pr;
                //                if (pr.ResultData == null) return pr;
                //                if (pr.ResultData.HaveError)
                //                {
                //                    throw new Exception(pr.ResultData.ErrorMsg + pr.ResultData.DetailErrorMsg);
                //                    if (acb.Exception != null) acb.Exception(pr.ResultData);
                //                }
                //                else
                //                {
                //                    return pr;
                ////                    acb.PostBackMethod(pr);
                //
                //                }

            }
        }

        public static Task<string> PostRequest(Uri uri, PostArgs pa,  string Method = "POST")
        {
            var tcs = new TaskCompletionSource<string>();
            WebClient wc = new WebClient();
            wc.UploadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.TrySetException(e.Error);
                }
                else if (e.Cancelled) tcs.TrySetCanceled();
                else
                {
                    //                    Thread.Sleep(2000);
                    tcs.TrySetResult(e.Result);
                }
            };
            var data = pa.ToString();
            if (Method == "POST")
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.UploadStringAsync(uri, "POST", data);
            }
            else
            {
                wc.UploadStringAsync(uri, "GET", data);
            }
            return tcs.Task;
        }
//        public static void ReadServerTextFile(string url, Action<string> readResult)
//        {
//            ReadServerFile(url, w =>
//                                    {
//                                        readResult(Encoding.UTF8.GetString(w, 0, w.Length));
//                                    });
//        }
         /// <summary>
        /// 返回服务器上的文件内容
        /// Resourse/aa.docx
        /// </summary>
        /// <param name="url">course/a.txt</param>
        /// <param name="readResult"></param>
        public static async Task<Assembly> AsyncReadServerDLLFile(string url)
        {
            url = url.TrimEnd('&');
            var index = url.IndexOf("?");
            if (index == -1) url += "?";
            var id = string.Format("sIDdctxzm0p={0}", Guid.NewGuid());
            url += id;
            WebClient client = new WebClient();
            var address = new Uri(BaseUri + url.Replace("\\", "/"));
            var stream = await client.OpenReadTaskAsync(address);
            var bytes =new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            AssemblyPart part = new AssemblyPart();
            return part.Load(new MemoryStream(bytes));
        }
        /// <summary>
        /// 返回服务器上的文件内容
        /// Resourse/aa.docx
        /// </summary>
        /// <param name="url">course/a.txt</param>
        /// <param name="readResult"></param>
        public static async Task<byte[]> AsyncReadServerByteFile(string url)
        {
            url = url.TrimEnd('&');
            var index = url.IndexOf("?");
            if (index == -1) url += "?";
            var id = string.Format("sIDdctxzm0p={0}", Guid.NewGuid());
            url += id;
            WebClient client = new WebClient();
            var address = new Uri(BaseUri + url.Replace("\\", "/"));
//            client.OpenReadAsync(address);
            var stream = await client.OpenReadTaskAsync(address);
            var bytes =new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            return bytes;
            //            client.OpenReadCompleted +=
            //                    (s, e1) =>
            //                    {
            //                        if (e1.Error == null)
            //                        {
            //                            var len = e1.Result.Length;
            //                            byte[] b = new byte[len];
            //                            e1.Result.Read(b, 0, b.Length);
            //                            e1.Result.Close();
            //                            //                            StreamReader myReader = new StreamReader(e1.Result);
            //                            //                            string reString = myReader.ReadToEnd();
            //                            //                            myReader.Close();
            //                            try
            //                            {
            //                                readResult(b);
            //                                //                                readResult(reString);
            //                            }
            //                            catch (XamlParseException e)
            //                            {
            ////                                LogHelper.AddError(e, url);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            readResult(null);
            //                        }
            //                    };
        }
        /// <summary>
        /// 返回服务器上的文件内容
        /// Resourse/aa.docx
        /// </summary>
        /// <param name="url">course/a.txt</param>
        public static async Task<string> AsyncReadServerTxtFile(string url)
        {
            url = url.TrimEnd('&');
            var index = url.IndexOf("?");
            if (index == -1) url += "?";
            var id = string.Format("sIDdctxzm0p={0}", Guid.NewGuid());
            url += id;
            WebClient client = new WebClient();
            var address = new Uri(BaseUri,url.Replace("\\", "/"));
            var re = await client.DownloadStringTaskAsync(address);
            return re;
        }

    }

//    public class WebClient1 : WebClient
//    {
//        ~WebClient1()
//        {
//            Debug.WriteLine("WebClient回收");
//        }
//    }
}
