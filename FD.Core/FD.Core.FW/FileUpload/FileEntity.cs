using System;


namespace Fw.FileUpload
{
    public  class FileEntity
    {
        /// <summary>
        /// 上传文件的相对目录和文件名
        /// </summary>
        public string FilePathName { get; set; }
        /// <summary>
        /// 文件总大小
        /// </summary>
        public  long FileSize { get; set; }
        /// <summary>
        /// 已经上传的大小 
        /// </summary>
        public  long UploadSize { get; set; }
        /// <summary>
        /// 开始上传时间
        /// </summary>
        public  DateTime StartTime { get; set; }
        /// <summary>
        /// 最后一次上传时间
        /// </summary>
        public  DateTime LastActiveTime { get; set; }
    }
}
