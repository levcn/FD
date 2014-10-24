using System;
using System.Net;
using System.Threading;

namespace StaffTrain.FwClass.Threads
{
    public class Worker
    {
        public string Name { get; set; }
        Action<object> action;
        private object param;
        public int Delay { get; set; }
        /// <summary>
        /// 1正在运行,2暂停,0停止
        /// </summary>
        public int IsPause { get; set; }
        public Worker(Action<object> _action, object _param, int delay = 1000)
        {
            action = _action;
            param = _param;
            Delay = delay;
        }
        public bool IsStop()
        {
            return IsPause == 0 || IsPause == 3;
        }
        public void Stop()
        {
            IsPause = 0;
        }

        public void Pause()
        {
            IsPause = 2;
        }

        public void Resume()
        {
            IsPause = 1;
        }
        public void Reset()
        {
            count = 0;
        }

        private int count = 0;
        public void Start()
        {
            if (IsPause == 1 || IsPause == 2) return;
            IsPause = 1;
            Thread thread = new Thread(
                    () =>
                    {
                        while (true)
                        {
                            count = 0;
                            while (true)
                            {
                                if (IsPause == 1)
                                {
                                    if (count++ > Delay) break;
                                }
                                else if (IsPause == 2)
                                {

                                }
                                else if (IsPause == 3)
                                {
                                    return;
                                }
                                else if (IsPause == 0)
                                {
                                    return;
                                }
                                try
                                {
                                    Thread.Sleep(1);
                                }
                                catch (Exception e)
                                {
                                    
                                }
                            }
                            Thread.Sleep(1);
                            action(param);
                        }
                    });
            thread.Start();
        }
    }
}
