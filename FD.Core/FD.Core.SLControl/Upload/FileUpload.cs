using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SLControls.DataClientTools;
using StaffTrain.FwClass.DataClientTools;
using StaffTrain.FwClass.Serializer;
using Telerik.Windows.Controls;


namespace SLControls.Upload
{
    public enum FileUploadStatus
    {
        Pending,
        Uploading,
        Complete,
        Error,
        Canceled,
        Removed,
        Resizing
    }
    public delegate void ProgressChangedEvent(object sender, UploadProgressChangedEventArgs args);
    public class UploadProgressChangedEventArgs
    {
        public int ProgressPercentage { get; set; }
        public long BytesUploaded { get; set; }
        public long TotalBytesUploaded { get; set; }
        public long TotalBytes { get; set; }
        public string FileName { get; set; }

        public UploadProgressChangedEventArgs() { }

        public UploadProgressChangedEventArgs(int progressPercentage, long bytesUploaded, long totalBytesUploaded, long totalBytes, string fileName)
        {
            ProgressPercentage = progressPercentage;
            BytesUploaded = bytesUploaded;
            TotalBytes = totalBytes;
            FileName = fileName;
            TotalBytesUploaded = totalBytesUploaded;
        }
    }
    public class FileUpload : INotifyPropertyChanged
    {
        public event ProgressChangedEvent UploadProgressChanged;
        public event EventHandler StatusChanged;

        public long ChunkSize = 1024 * 1024 * 4;//4M

        public Uri UploadUrl { get; set; }
        private FileInfo file;
        public FileInfo File
        {
            get { return file; }
            set
            {
                file = value;
                Stream temp = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                FileLength = temp.Length;
                temp.Close();
            }
        }
        public string Name { get { return File.Name; } }
        private long fileLength;
        public long FileLength
        {
            get { return fileLength; }
            set
            {
                fileLength = value;

                Dispatcher.BeginInvoke(() =>
                {
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("FileLength"));
                });
            }
        }
        //(/\*(.|[\r\n])*?\*/)|(//.*)
        private MemoryStream resizeStream;
        public bool ResizeImage { get; set; }
        public int ImageSize { get; set; }

        private long bytesUploaded;
        public long BytesUploaded
        {
            get { return bytesUploaded; }
            set
            {
                bytesUploaded = value;

                this.Dispatcher.BeginInvoke(delegate()
                {
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("BytesUploaded"));
                });
            }
        }

        private int uploadPercent;
        public int UploadPercent
        {
            get { return uploadPercent; }
            set
            {
                uploadPercent = value;

                this.Dispatcher.BeginInvoke(delegate()
                {
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("UploadPercent"));
                });
            }
        }

        private FileUploadStatus status;
        public FileUploadStatus Status
        {
            get { return status; }
            set
            {
                status = value;

                this.Dispatcher.BeginInvoke(delegate()
                {
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Status"));
                    if (StatusChanged != null)
                        StatusChanged(this, null);
                });
            }
        }

        private Dispatcher Dispatcher;

        private bool cancel;
        private bool remove;

        private bool displayThumbnail;
        public bool DisplayThumbnail
        {
            get { return displayThumbnail; }
            set
            {
                displayThumbnail = value;

                this.Dispatcher.BeginInvoke(delegate()
                {
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("DisplayThumbnail"));
                });
            }
        }

        public FileUpload(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            Status = FileUploadStatus.Pending;
        }

        public FileUpload(Dispatcher dispatcher, Uri uploadUrl)
            : this(dispatcher)
        {
            UploadUrl = uploadUrl;
        }

        public FileUpload(Dispatcher dispatcher, Uri uploadUrl, FileInfo fileToUpload)
            : this(dispatcher, uploadUrl)
        {
            File = fileToUpload;
        }

        public void Upload()
        {
            if (File==null) throw new Exception("File为空");
            if (UploadUrl == null) throw new Exception("UploadUrl为空");
            Status = FileUploadStatus.Uploading;
            cancel = false;

            if (ResizeImage && file.Name.ToLower().EndsWith("jpg") && resizeStream == null && ImageSize > 0)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

                worker.RunWorkerAsync();
            }
            else
                CheckFileOnServer();
        }

        private void CheckFileOnServer()
        {
            UriBuilder ub = new UriBuilder(UploadUrl);
            ub.Query = string.Format("{1}filename={0}&GetBytes=true", File.Name.Replace("#", ""), string.IsNullOrEmpty(ub.Query) ? "" : ub.Query.Remove(0, 1) + "&");
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadStringAsync(ub.Uri);
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            long lengthtemp = 0;
            if (!string.IsNullOrEmpty(e.Result))
            {
                try
                {
                    lengthtemp = long.Parse(e.Result);
                }
                catch (Exception ex)
                {
                    var r = JsonHelper.JsonDeserialize<ResultData>(e.Result);
                    if (r != null)
                    {
                        throw new Exception(r.ErrorMsg+ r.DetailErrorMsg);
                    }
                    //MessageBox.Show(e.Result);
                }
            }

            if (lengthtemp > 0)
            {

                if (lengthtemp >= FileLength)
                {
                    RadWindow.Confirm("该文件在服务器上已经存在,是否覆盖?",
                          (s,ee)=>
                          {
                              if (ee.DialogResult.HasValue && ee.DialogResult.Value)
                              {
                                  lengthtemp = 0;
                              }
                              else
                              {
                                  UploadProgressChangedEventArgs args = new UploadProgressChangedEventArgs(100, FileLength - BytesUploaded, BytesUploaded, FileLength, file.Name);
                                  this.Dispatcher.BeginInvoke(delegate()
                                  {
                                      UploadProgressChanged(this, args);
                                  });
                                  BytesUploaded = FileLength;
                                  Status = FileUploadStatus.Complete;

                              }
                          }
                                                                    );

                }
                else
                {
                    RadWindow.Confirm("该文件在服务器上已经存在是否续传?",
                                                                    (s, ee) =>
                                                                    {
                                                                        if (ee.DialogResult.HasValue && !ee.DialogResult.Value)

                                                                        {
                                                                            lengthtemp = 0;
                                                                        }
                                                                    });
                }
            }
            BytesUploaded = lengthtemp;
            UploadFileEx();
        }

        public void CancelUpload()
        {
            cancel = true;
        }

        public void RemoveUpload()
        {
            cancel = true;
            remove = true;
            if (Status != FileUploadStatus.Uploading)
                Status = FileUploadStatus.Removed;
        }

        public void UploadFileEx()
        {
            Status = FileUploadStatus.Uploading;
            long temp = FileLength - BytesUploaded;

            UriBuilder ub = new UriBuilder(UploadUrl);
            bool complete = temp <= ChunkSize;
            ub.Query = string.Format("{3}filename={0}&StartByte={1}&Complete={2}&FileType={4}&SavePath={5}", File.Name.Replace("#", ""), BytesUploaded, complete, string.IsNullOrEmpty(ub.Query) ? "" : ub.Query.Remove(0, 1) + "&", FileType, SavePath);

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(ub.Uri);
            webrequest.Method = "POST";
            webrequest.BeginGetRequestStream(WriteCallback, webrequest);
        }

        private void WriteCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the operation.
            Stream requestStream = webrequest.EndGetRequestStream(asynchronousResult);

            byte[] buffer = new Byte[4096 * 1000];
            int bytesRead = 0;
            int tempTotal = 0;

            Stream fileStream = resizeStream != null ? (Stream)resizeStream : File.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //using (FileStream fileStream = File.OpenRead())
            //{
            fileStream.Position = BytesUploaded;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0 && tempTotal + bytesRead < ChunkSize && !cancel)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();
                BytesUploaded += bytesRead;
                tempTotal += bytesRead;
            }
            if (UploadProgressChanged != null)
            {
                int percent = (int)(((double)BytesUploaded / (double)FileLength) * 100);
                UploadProgressChangedEventArgs args = new UploadProgressChangedEventArgs(percent, bytesRead, BytesUploaded, FileLength, file.Name);
                this.Dispatcher.BeginInvoke(() => UploadProgressChanged(this, args));
            }
            //}

            // only close the stream if it came from the file, don't close resizestream so we don't have to resize it over again.
            if (resizeStream == null)
                fileStream.Close();
            requestStream.Close();
            webrequest.BeginGetResponse(new AsyncCallback(ReadCallback), webrequest);

        }
        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)webrequest.EndGetResponse(asynchronousResult);
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string responsestring = reader.ReadToEnd();
            var res = JsonHelper.JsonDeserialize<ResponseResult>(responsestring);
            string fileName = "";
            if (res != null && res.FileName != null)
            {
                fileName = res.FileName;
            }
            else
            {
                var r = JsonHelper.JsonDeserialize<ResultData>(responsestring);
                if (r != null)
                {
                    throw new Exception(r.ErrorMsg+ r.DetailErrorMsg);
                }
            }
            reader.Close();

            if (cancel)
            {
                if (resizeStream != null)
                    resizeStream.Close();
                if (remove)
                    Status = FileUploadStatus.Removed;
                else
                    Status = FileUploadStatus.Canceled;
            }
            else if (BytesUploaded < FileLength)
                UploadFileEx();
            else
            {
                if (resizeStream != null)
                    resizeStream.Close();

                Status = FileUploadStatus.Complete;
                OnComplete(new CompleteEventArgs { Result = res });
            }

        }
        /// <summary>
        /// 上传完成之后的结果数据对象
        /// </summary>
        public class ResponseResult
        {
            /// <summary>
            /// 修改过后的文件名
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// 上传之前的原始文件名
            /// </summary>
            public string OriginalFileName { get; set; }

        }

        public event EventHandler<CompleteEventArgs> Complete;
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        protected void OnComplete(CompleteEventArgs e)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                if (e.Result != null)
                {
                    FileName = e.Result.FileName;
                    OriginalFileName = e.Result.OriginalFileName;
                }
                EventHandler<CompleteEventArgs> handler = Complete;
                if (handler != null) handler(this, e);
            });

        }

        void Resize()
        {
            Status = FileUploadStatus.Resizing;
            Stream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (fileStream)
            {
                // Decode
                //DecodedJpeg jpegIn = new JpegDecoder(fileStream).Decode();

                //if (!ImageResizer.ResizeNeeded(jpegIn.Image, ImageSize))
                //{
                //    return;
                //}
                //else
                //{

                //    // Resize
                //    DecodedJpeg jpegOut = new DecodedJpeg(
                //        new ImageResizer(jpegIn.Image)
                //            .Resize(ImageSize, ResamplingFilters.NearestNeighbor),
                //        jpegIn.MetaHeaders); // Retain EXIF details

                //    // Encode
                //    resizeStream = new MemoryStream();
                //    new JpegEncoder(jpegOut, 90, resizeStream).Encode();
                //    // Display 
                //    resizeStream.Seek(0, SeekOrigin.Begin);
                //    FileLength = resizeStream.Length;
                //}
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CheckFileOnServer();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Resize();
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public int FileType { get; set; }
        public string SavePath { get; set; }
    }
    /// <summary>
    /// 上传之完成事件的参数
    /// </summary>
    public class CompleteEventArgs : EventArgs
    {
        /// <summary>
        /// 上传返回的结果
        /// </summary>
        public FileUpload.ResponseResult Result { get; set; }
    }
}
