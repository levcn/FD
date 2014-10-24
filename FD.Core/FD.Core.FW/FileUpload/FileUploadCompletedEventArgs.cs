namespace Fw.FileUpload
{
    /// <summary>
    /// �ϴ��ļ���ɲ���
    /// </summary>
    public class FileUploadCompletedEventArgs
    {
        /// <summary>
        /// �ļ���
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// ·��
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