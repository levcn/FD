namespace Fw.FileUpload
{
    /// <summary>
    /// 上传文件完成参数
    /// </summary>
    public class FileUploadCompletedEventArgs
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string FilePath { get; set; }

        public FileUploadCompletedEventArgs() { }

        public FileUploadCompletedEventArgs(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }
    }
}