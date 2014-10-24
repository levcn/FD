using System.Threading;
using Levcn.Framework.Web.Downloader;


namespace SLControls.Download
{
    /// <summary> 
    /// 记录下载的字节位置 
    /// </summary> 
    public class DownLoadState
    {
        private readonly string _attachmentName;
        private readonly byte[] _data;
        private readonly string _fileName;
        private readonly int _length;
        private int _totalLength;
        private int _startPosition;
        private readonly int _position;
        private readonly string _requestURL;
        private readonly string _responseURL;
        private readonly ThreadCallbackHandler _threadCallback;
        private HttpWebClient _hwc;
        private Thread _thread;

        internal DownLoadState(string requestURL, string responseURL, string fileName, string attachmentName,
                               int position, int length, byte[] data, int totalLength,int startPosition)
        {
            _fileName = fileName;
            _requestURL = requestURL;
            _responseURL = responseURL;
            _attachmentName = attachmentName;
            _position = position;
            _data = data;
            _length = length;
            _totalLength = totalLength;
            _startPosition = startPosition;
        }

        internal DownLoadState(string requestURL, string responseURL, string fileName, string attachmentName,
                               int position, int length, ThreadCallbackHandler tch, int totalLength, int startPosition)
        {
            _requestURL = requestURL;
            _responseURL = responseURL;
            _fileName = fileName;
            _attachmentName = attachmentName;
            _position = position;
            _length = length;
            _threadCallback = tch;
            _totalLength = totalLength;
            _startPosition = startPosition;
        }

        internal DownLoadState(string requestURL, string responseURL, string fileName, string attachmentName,
                               int position, int length, int totalLength, int startPosition)
        {
            _requestURL = requestURL;
            _responseURL = responseURL;
            _fileName = fileName;
            _attachmentName = attachmentName;
            _position = position;
            _length = length;
            _totalLength = totalLength;
            _startPosition = startPosition;
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public int Position
        {
            get { return _position; }
        }
        public int TotalLength
        {
            get { return _totalLength; }
        }

        public int Length
        {
            get { return _length; }
        }

        public string AttachmentName
        {
            get { return _attachmentName; }
        }

        public string RequestURL
        {
            get { return _requestURL; }
        }

        public string ResponseURL
        {
            get { return _responseURL; }
        }

        public byte[] Data
        {
            get { return _data; }
        }

        public HttpWebClient HttpWebClient
        {
            get { return _hwc; }
            set { _hwc = value; }
        }

        internal Thread thread
        {
            get { return _thread; }
            set { _thread = value; }
        }

        public int StartPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }

        // 
        internal void StartDownloadFileChunk()
        {
            if (_threadCallback != null)
            {
                _threadCallback(_requestURL, _fileName, _position, _length);
                _hwc.OnThreadProcess(_thread);
            }
        }
    }

    

}