using System;
using System.IO;
using System.Linq;
using System.Web;


namespace Fw.FileUpload
{
    public class FileUploadProcess
    {
        public event FileUploadCompletedEvent FileUploadCompleted;
        /// <summary>
        /// Determines if uploaded files should be renamed according to the user uploading them, otherwise if
        /// multiple users upload a file of the same name, it would try to save the file to the same name, throwing an error.
        /// Another way to prevent this is to create a seperate folder for each file.
        /// </summary>
        public bool UniqueUserUpload { get; set; }

        public FileUploadProcess()
        {
        }

        public void ProcessRequest(HttpContext context, string uploadPath)
        {
            bool isNew;
            string outFileName;
            long fileLength;
            long uploadLength;
            ProcessRequest(context, uploadPath, out isNew, out outFileName, out fileLength, out uploadLength);
        }
        public void ProcessRequest(HttpContext context, string uploadPath,out bool isNew,out string outFileName,out long fileLength,out long uploadLength)
        {//upload.aspx?filename=aa.flv&GetBytes=true
            // 0
            //20000
            //upload.aspx?filename=aa.flv&StartByte=20000
            string filename = context.Request["filename"];
            string path = context.Request["Path"];
            if (string.IsNullOrEmpty(path))
            {
                outFileName = filename;
            }
            else
            {
                outFileName = Path.Combine(path, filename);
            }
            isNew = false;
            uploadLength = 0;
            if (!string.IsNullOrEmpty(path))
            {
                uploadPath = Path.Combine(uploadPath, path);
            }
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            bool complete = string.IsNullOrEmpty(context.Request["Complete"]) ? true : bool.Parse(context.Request["Complete"]);
            bool getBytes = string.IsNullOrEmpty(context.Request["GetBytes"]) ? false : bool.Parse(context.Request["GetBytes"]);
            long startByte = string.IsNullOrEmpty(context.Request["StartByte"]) ? 0 : long.Parse(context.Request["StartByte"]);
            fileLength = string.IsNullOrEmpty(context.Request["FileSize"]) ? 0 : long.Parse(context.Request["FileSize"]);

            string filePath;
            if (UniqueUserUpload)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    filePath = Path.Combine(uploadPath, string.Format("{0}_{1}", context.User.Identity.Name.Replace("\\", ""), filename));
                }
                else
                {
                    if (context.Session["fileUploadUser"] == null)
                        context.Session["fileUploadUser"] = Guid.NewGuid();
                    filePath = Path.Combine(uploadPath, string.Format("{0}_{1}", context.Session["fileUploadUser"], filename));
                }
            }
            else
                filePath = Path.Combine(uploadPath, filename);

            if (getBytes)
            {
                FileInfo fi = new FileInfo(filePath);
                if (!fi.Exists)
                    context.Response.Write("0");
                else
                    context.Response.Write(fi.Length.ToString());

                context.Response.Flush();
            }
            else
            {
                Stream stream = context.Request.InputStream;
                if (context.Request.Files.Count > 0)
                {
                    stream = context.Request.Files[0].InputStream;
                }
                if (startByte > 0 && File.Exists(filePath))
                {
                    using (FileStream fs = File.Open(filePath, FileMode.Append))
                    {
                        SaveFile(stream, fs);
                        uploadLength = fs.Length;
                        fs.Close();
                        var f = new string("".ToList().TakeWhile(a => a != ' ').ToArray());
                    }
                }
                else
                {
                    isNew = true;
                    using (FileStream fs = File.Create(filePath))
                    {
                        SaveFile(stream, fs);
                        uploadLength = fs.Length;
                        fs.Close();
                    }
                }
                if (complete)
                {
                    if (FileUploadCompleted != null)
                    {
                        FileUploadCompletedEventArgs args = new FileUploadCompletedEventArgs(filename, filePath);
                        FileUploadCompleted(this, args);
                    }
                }
            }
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }
    }
}
