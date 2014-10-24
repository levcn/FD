using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fw.Pools.Files
{
    /// <summary>
    /// 线程池中的一个线程
    /// </summary>
    public class FileThread
    {
        #region 构造

        internal FileThread(string _path)
        {
            Path = _path;
        }

        #endregion

        #region 属性

        private string path;
        private DateTime firstOpenTime;
        private DateTime lastExecuteTime;
        private PoolFileStream stream;

        /// <summary>
        /// 文件操作流
        /// </summary>
        public PoolFileStream Stream
        {
            get
            {
                return stream;
            }
        }

        /// <summary>
        /// 是否打开
        /// </summary>
        public bool Opened { get; set; }

        /// <summary>
        /// 是否繁忙
        /// </summary>
        public bool Busy { get; set; }

        /// <summary>
        /// 是否可以使用
        /// </summary>
        public bool CanUse
        {
            get
            {
                return Opened && !Busy;
            }
        }
        /// <summary>
        /// 是否标记已经使用(在获取空闲线程之后,先标记该线程已经被注册,使用完线程之后恢复该标记)
        /// </summary>
        internal bool RegisterUsed { get; set; }
        /// <summary>
        /// 当前线程所操作的文件
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        /// <summary>
        /// 当前线程开启后,第一次启动的时间(如果文件重新打开,则重新开始计算)
        /// </summary>
        public DateTime FirstOpenTime
        {
            get
            {
                return firstOpenTime;
            }
        }
        /// <summary>
        /// 当前线程最后使用的时间
        /// </summary>
        public DateTime LastExecuteTime
        {
            get
            {
                return lastExecuteTime;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 执行指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="action1"></param>
        public void Execute(Action<PoolFileStream> action = null, Action<PoolFileStream, FileThread> action1 = null)
        {
            Busy = true;
            lastExecuteTime = DateTime.Now;
            try
            {
                if (action != null) action(Stream);
                if (action1 != null) action1(Stream, this);
                if (Stream!=null&&Stream.CanWrite) Stream.Flush();
            }
            catch(Exception e)
            {
                throw;
            }
            Busy = false;
            RegisterUsed = false;
        }
        /// <summary>
        /// 关闭当前操作流
        /// </summary>
        internal void Close()
        {
            firstOpenTime = DateTime.MinValue;
            Opened = false;
            if(Stream!=null)Stream.Close();
        }

        public bool WriteEnabled { get; set; }
        /// <summary>
        /// 打开当前操作流
        /// </summary>
        internal void Open()
        {
            if (DateTime.MinValue ==  firstOpenTime) firstOpenTime = DateTime.Now;
            Opened = true;
            if (WriteEnabled)
            {
                stream = new PoolFileStream(Path,FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            else
            {
                stream = new PoolFileStream(Path);
            }
            
        }
        #endregion

        /// <summary>
        /// 当前线程的ID
        /// </summary>
        public int ID { get; set; }
    }
}
