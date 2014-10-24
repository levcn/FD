/*
 性能测试E5700 3.0G 双核 6G内存
 线程池的查询速度为 250万/秒
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Fw.Pools.Files
{

    /// <summary>
    /// 对于单个文件的线程池(一个线程池管理一个文件)
    /// </summary>
    public class PoolFile
    {
        public PoolFile(string filePath)
        {
            path = filePath;
            Threads = new List<FileThread>(MinThread);
        }
        #region 属性

        private string path;
        private int maxThread = 30;
        private int minThread = 2;
        private int timeOut = 5000;
        private List<FileThread> threads;

        public int ThreadCount{get
        {
            return threads.Count;
        }}
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

        public int MaxThread
        {
            get
            {
                return maxThread;
            }
            set
            {
                maxThread = value;
            }
        }

        public int MinThread
        {
            get
            {
                return minThread;
            }
            set
            {
                minThread = value;
            }
        }

        public int TimeOut
        {
            get
            {
                return timeOut;
            }
            set
            {
                timeOut = value;
            }
        }

        public List<FileThread> Threads
        {
            get
            {
                return threads;
            }
            set
            {
                threads = value;
            }
        }

        #endregion

        #region 方法

        private object exe = new object();
        public void Execute(Action<PoolFileStream> action = null,Action<PoolFileStream,FileThread> action1=null)
        {
            FileThread thread = null;
            int c = 0;
            lock (exe)
            {
                //_count++;
                //if (_count == int.MaxValue) _count = 1;
                //c = _count;
                thread = GetFreeThread(c);
            }

            thread.Execute(action, action1);
        }
        public int FreeThreadCount
        {
            get
            {
                return Threads.Count(
                    w =>
                        w.CanUse && !w.RegisterUsed);
            }
        }
        public FileThread GetFreeThread(int index)
        {
            int z = 0;
            FileThread returnValue = null;
            while (z < TimeOut)
            {
                bool haveNew = false;//是否有新线程添加进来,或者回收了线程
                var re = Threads.FirstOrDefault(
                    w =>
                        w.CanUse && !w.RegisterUsed);
                
                if(re!=null)
                {
                    re.RegisterUsed = true;
                    returnValue = re;
                    break;
                }
                else
                {
                    if(RepairThreads() == 0)
                    {
                        if(AddThread())
                        {
                            haveNew = true;
                        }
                    }
                    else
                    {
                        haveNew = true;
                    }
                }
                if (!haveNew) 
                    Thread.Sleep(1);
            }
            //_currentIndex++;
            //if (_currentIndex == int.MaxValue) _currentIndex = 1;
            return returnValue;
        }
        /// <summary>
        /// 添加一个线程到池中
        /// </summary>
        /// <returns></returns>
        private bool AddThread()
        {
            if(Threads.Count<MaxThread)
            {
                var file = (new FileThread(path));
                file.ID = Threads.Count + 1;
                file.Open();
                Threads.Add(file);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 打开关闭的线程
        /// </summary>
        private int RepairThreads()
        {
            var list = Threads.Where(w => !w.Opened).ToList();
            list.ForEach(w=>w.Open());
            return list.Count;
        }
        #endregion

        public void Close()
        {
            Threads.ForEach(w=>w.Close());
            Threads.Clear();
        }
    }
}
