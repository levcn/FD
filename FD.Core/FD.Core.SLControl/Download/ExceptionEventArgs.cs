using System;


namespace SLControls.Download
{
    /// <summary> 
    /// 包含 Exception 事件数据的类 
    /// </summary> 
    public class ExceptionEventArgs : EventArgs
    {
        private readonly DownLoadState _DownloadState;
        private readonly Exception _Exception;

        internal ExceptionEventArgs(Exception e, DownLoadState DownloadState)
        {
            _Exception = e;
            _DownloadState = DownloadState;
        }

        public DownLoadState DownloadState
        {
            get { return _DownloadState; }
        }

        public Exception Exception
        {
            get { return _Exception; }
        }

        public ExceptionActions ExceptionAction { get; set; }
    }
}
