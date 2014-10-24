using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectCreater.Threading
{
    public class PCThreadPool
    {
        static List<PCTask> Actions = new List<PCTask>();

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="action"></param>
        public static void AddAction(string name,Action action)
        {
            lock (Actions)
            {
                var item = Actions.FirstOrDefault(w => w.Name == name);
                if (item == null)
                {
                    Actions.Add(new PCTask(name,action));
                }
            }
        }

        /// <summary>
        /// 删除一个任务
        /// </summary>
        /// <param name="action"></param>
        public static void RemoveAction(string name)
        {
            lock (Actions)
            {
                var item = Actions.FirstOrDefault(w => w.Name == name);
                if (item == null)
                {
                    Actions.Remove(item);
                }
            }
        }
        public static void Start()
        {
            new Action(() =>
            {
                while(true)
                {
                    Actions.ToList().ForEach(w =>
                    {
                        if ((DateTime.Now - w.LastRunTime) > w.DelayTimeSpan)
                        {
                            w.Action.BeginInvoke(null,null);
                            w.LastRunTime = DateTime.Now;
                        }
                    });
                    Thread.Sleep(500);
                }
            }).BeginInvoke(null,null);
        }
    }
}
