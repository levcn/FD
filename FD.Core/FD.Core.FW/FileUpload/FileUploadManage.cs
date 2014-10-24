using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace Fw.FileUpload
{
    /// <summary>
    /// 文件上传管理
    /// </summary>
    public static class FileUploadManage
    {
        /// <summary>
        /// 初始化上传文件设置
        /// </summary>
        /// <param name="uploadRootPath">上传文件是绝对根目录</param>
        /// <param name="configFileName"></param>
        public static void Init(string uploadRootPath, string configFileName)
        {
            if (FileEntityManage == null)
            {
                FileEntityManage = new FileEntityManage(uploadRootPath,  configFileName);
            }
        }
        private static readonly object LockedObject = new object();
        static FileEntityManage FileEntityManage { get; set; }
        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="context">当前会话</param>
        /// <param name="completed">上传文件的事件</param>
        public static void ProcessRequest(HttpContext context, FileUploadCompletedEvent completed)
        {
            lock (LockedObject)
            {
                string fileName;
                bool isNew;
                long fileLength, uploadLength;
                FileUploadProcess fileUpload = new FileUploadProcess();
                fileUpload.FileUploadCompleted += completed;
                fileUpload.ProcessRequest(context, FileEntityManage.UploadRootPath,out isNew,out fileName,out fileLength,out uploadLength);
                FileEntityManage.Update(fileName,isNew,fileLength,uploadLength);
            }
        }
    }
}
