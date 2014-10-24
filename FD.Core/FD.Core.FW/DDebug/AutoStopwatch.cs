using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServerFw.DDebug
{
    /// <summary>
    /// 执行时间
    /// </summary>
    public class AutoStopwatch : Stopwatch, IDisposable
    {
        private readonly string name;

        public AutoStopwatch(string name)
        {
            this.name = name;
            Start();
        }

        public void Dispose()
        {
            Stop();
            Debug.WriteLine("{0}运行时间: {1}", name, this.Elapsed);
        }
    }
    public class testAutoStopwatch
    {
        public void Test()
        {
            using (new AutoStopwatch("sdfasdfa"))
            {

            }
        }
    }
}
