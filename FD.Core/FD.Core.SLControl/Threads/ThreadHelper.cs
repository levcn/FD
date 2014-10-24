using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Extends;


namespace SLControls.Threads
{
    public static class ThreadHelper
    {
        /// <summary>
        /// 启动一个线程执行指定的程序
        /// </summary>
        /// <param name="action"></param>
        public static void StartThread(Action action)
        {
            StartThread(o => action(), null);
        }
        static List<Worker> workers = new List<Worker>();
//        public static void DelayRun(Action action, int s = 100, string name = null)
//        {
//            DelayRun(() =>
//            {
//                action();
//                return true;
//            }, s, name);
//        }
        public static void DelayRunNew(Action action, int s = 100)
        {
            new Thread(() =>
            {
                Thread.Sleep(s);
                action();
            }).Start();
        }
        /// <summary>
        /// 延迟执行指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="s"></param>
        /// <param name="name"></param>
        public static void DelayRun(Func<bool> action, int s = 100, string name = null, int count = 10000, int sec = 30)
        {
            SynchronizationContext currentContext = SynchronizationContext.Current;
            Worker work = null;
            bool reset = false;
            bool isNew = false;
            if (name != null)
            {
                work = workers.FirstOrDefault(w => w.Name == name && !w.IsStop());
                workers.Remove1(w => w.IsStop());
                if (work != null)
                {
                    reset = true;
                    work.Reset();
                }
            }
            if (!reset)
            {
                isNew = true;
                DateTime dt = DateTime.Now;
                work = ThreadHelper.StartThread(
                        (o) =>
                        {
                            currentContext.Post(w =>
                            {
                                if (work != null && work.IsStop())
                                {
                                    return;
                                }
                                if (action())
                                {
                                    if (work != null) work.Stop();
                                }
                                if ((DateTime.Now - dt).TotalSeconds > sec)
                                {
                                    if (work != null) work.Stop();
                                }
                            }, null);

                        }, s, count);
            }
            if (name != null)
            {
                work.Name = name;
                if (isNew) workers.Add(work);
            }
        }
//        /// <summary>
//        /// 每X毫秒执行一下指定的方法
//        /// </summary>
//        /// <param name="action"></param>
//        /// <param name="delay"></param>
//        /// <returns></returns>
//        public static Worker StartThread(Action action, int delay, int repeat = 0)
//        {
//            Worker wk = null;
//            Action ac = action;
//            if (repeat > 0)
//            {
//                int count = 0;
//                ac = () =>
//                {
//                    count++;
//                    if (count >= repeat) if (wk != null) wk.Stop();
//                    action();
//                };
//            }
//
//            wk = StartThread(o => ac(), null, delay);
//            return wk;
//        }
        static Random r = new Random();
        public static void DelayRun(Action action, int delay, string name = null)
        {
            if (name == null) name = action.GetHashCode().ToString() + r.Next(1, 10000000); ;
            lock (Workers)
            {
                var worker = Workers.FirstOrDefault(w => w.Name == name);
                if (worker != null)
                {
                    worker.RunTime = DateTime.Now.AddMilliseconds(delay);
                }
                else
                {
                    worker = new Worker() { Action = action, Name = name, RunTime = DateTime.Now.AddMilliseconds(delay) };
                    Workers.Add(worker);
                }
            }
        }
        /// <summary>
        /// 每X毫秒执行一下指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="param"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Worker StartThread(Action<object> action, object param, int delay)
        {
            Worker worker = new Worker(action, null, delay);

            worker.Start();
            return worker;
        }
        /// <summary>
        /// 每X毫秒执行一下指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Worker StartThread(Action action, int delay)
        {
            return StartThread(o => action(), null, delay);
        }
        /// <summary>
        /// 启动一个线程执行指定的程序
        /// </summary>
        /// <param name="action"></param>
        public static void StartThread(Action<object> action, object o)
        {
            var th = new Thread(() => action(o));
            th.Start();
        }
        static List<Worker> Workers = new List<Worker>();
        static ThreadHelper()
        {
//            Action action = new Action(Start);
//            action.Invoke();
            ThreadPool.QueueUserWorkItem(w => Start());
        }

        private static void Start()
        {
            while (true)
            {
                lock (Workers)
                {
                    var runner = Workers.Where(w => w.RunTime <= DateTime.Now).ToList();
                    if (runner.Count > 0)
                    {
                        runner.ForEach(w => Workers.Remove(w));
                        ThreadPool.QueueUserWorkItem(z =>
                        {
                            runner.ForEach(w =>
                            {
                                ThreadPool.QueueUserWorkItem(v =>
                                {
                                    w.Action();
                                });
                            });
                        });
                    }
                }
                Thread.Sleep(1);
            }
        }

        
    }
}
