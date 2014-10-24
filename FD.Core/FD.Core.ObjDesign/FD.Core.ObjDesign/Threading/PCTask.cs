using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCreater.Threading
{
    public class PCTask
    {
        public PCTask(string name,Action action, TimeSpan delayTimeSpan = default (TimeSpan))
        {
            Name = name;
            Action = action;
            if (delayTimeSpan == default(TimeSpan)) delayTimeSpan = TimeSpan.FromSeconds(10);
            DelayTimeSpan = delayTimeSpan;
        }
        public string Name { get; set; }
        public Action Action;
        public TimeSpan DelayTimeSpan;
        public DateTime LastRunTime { get; set; }
    }
}
