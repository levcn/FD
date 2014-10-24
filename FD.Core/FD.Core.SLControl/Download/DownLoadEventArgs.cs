using System;
using Levcn.Framework.Web.Downloader;


namespace SLControls.Download
{
    /// <summary> 
    /// 包含 DownLoad 事件数据的类 
    /// </summary> 
    public class DownLoadEventArgs : EventArgs
    {
        private readonly DownLoadState _DownloadState;

        public DownLoadEventArgs(DownLoadState DownloadState)
        {
            _DownloadState = DownloadState;
        }

        public DownLoadState DownloadState
        {
            get { return _DownloadState; }
        }
    }
}
