using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Levcn.Framework.Web.Downloader
{
    public class ThreadProcessEventArgs : EventArgs
    {
        private readonly Thread _thread;

        public ThreadProcessEventArgs(Thread thread)
        {
            _thread = thread;
        }

        public Thread thread
        {
            get { return _thread; }
        }
    }
}
