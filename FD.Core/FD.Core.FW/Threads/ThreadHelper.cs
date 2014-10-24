using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fw.Extends;
using StaffTrain.FwClass.Threads;


namespace Fw.Threads
{
    public static class ThreadHelper
    {
//        /// <summary>
//        /// 启动一个线程执行指定的程序
//        /// </summary>
//        /// <param name="action"></param>
//        public static void StartThread(Action action)
//        {
//            StartThread(o => action(), null);
//        }
        static List<Worker> workers = new List<Worker>();
        public static void DelayRun(Action action, int s = 100, string name = null)
        {
            DelayRun(() =>
            {
                action();
                return true;
            }, s, name);
        }
        /// <summary>
        /// 延迟执行指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="s"></param>
        /// <param name="name"></param>
        public static void DelayRun(Func<bool> action, int s = 100, string name = null, int count = 10000, int sec = 30)
        {
//            SynchronizationContext currentContext = SynchronizationContext.Current;
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
                        () =>
                        {
                            var a = new Action<object>(w =>
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
                            });
//                            if (currentContext != null)
//                            {
//                                currentContext.Post(w =>
//                                {
//                                    a(w);
//                                }, null);
//                            }
//                            else
                            {
                                a(null);
                            }
                        }, s, count);
            }
            if (name != null)
            {
                work.Name = name;
                if (isNew) workers.Add(work);
            }
        }
        /// <summary>
        /// 每X毫秒执行一下指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Worker StartThread(Action action, int delay, int repeat = 0)
        {
            Worker wk = null;
            Action ac = action;
            if (repeat > 0)
            {
                int count = 0;
                ac = () =>
                {
                    count++;
                    if (count >= repeat) if (wk != null) wk.Stop();
                    action();
                };
            }

            wk = StartThread(o => ac(), null, delay);
            return wk;
        }
//        /// <summary>
//        /// 每X毫秒执行一下指定的方法
//        /// </summary>
//        /// <param name="action"></param>
//        /// <param name="param"></param>
//        /// <param name="delay"></param>
//        /// <returns></returns>
//        public static Worker StartThread(Action<object> action, object param, int delay)
//        {
//            Worker worker = new Worker(action, null, delay);
//
//            worker.Start();
//            return worker;
//        }
//        /// <summary>
//        /// 启动一个线程执行指定的程序
//        /// </summary>
//        /// <param name="action"></param>
//        public static void StartThread(Action<object> action, object o)
//        {
//            var th = new Thread(() => action(o));
//            th.Start();
//        }
        /// <summary>
        /// 试着执行指定的方法,如果出错,会在5秒以内再次尝试执行,
        /// </summary>
        /// <param name="action">要执行的方法</param>
        /// <param name="sleep">执行间隔</param>
        public static void Try(Action action, int sleep = 1)
        {
            Try(action, new TimeSpan(0,0,5), sleep);
        }
        /// <summary>
        /// 试着执行指定的方法,如果出错,会在指定的时间内再次尝试执行,
        /// </summary>
        /// <param name="action">要执行的方法</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="sleep">执行间隔</param>
        public static void Try(Action action, TimeSpan timeout, int sleep = 1)
        {
            DateTime dt = DateTime.Now;
            while (true)
            {
                bool haveError = false;
                TryAction(action, e =>
                                      {
                                          haveError = true;
                                      });
                if (!haveError) break;
                if (DateTime.Now - dt > timeout) break;
                Thread.Sleep(sleep);
            }
        }
        /// <summary>
        /// 试着执行指定方法如果出错,执行另外一个方法
        /// </summary>
        /// <param name="action">要执行的方法</param>
        /// <param name="eAction">出错时执行的方法</param>
        public static void TryAction(Action action, Action<Exception> eAction)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                eAction(e);
            }
        }
        /// <summary>
        /// 启动一个线程执行指定的程序
        /// </summary>
        /// <param name="action"></param>
        public static void StartThread(Action action)
        {
            StartThread((o) => action(), null);
        }
        /// <summary>
        /// 每X毫秒执行一下指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Worker StartThread(Action action,int delay)
        {
            return StartThread(o => action(), null, delay);
        }
        /// <summary>
        /// 每X毫秒执行一下指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="param"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Worker StartThread(Action<object> action,object param,int delay)
        {
            Worker worker = new Worker(action,null,delay);
            worker.Start();
            return worker;
        }
        /// <summary>
        /// 启动一个线程执行指定的程序
        /// </summary>
        /// <param name="action"></param>
        public static void StartThread(Action<object> action,object o)
        {
            ThreadPool.QueueUserWorkItem(_ => action(o));
        }
    }
}
