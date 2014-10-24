using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fw.Pools.Files
{
    /// <summary>
    /// 单线程的文件读取管理
    /// </summary>
    public class ReadFileManage
    {
        ReadFileManage()
        {
        }
        private static ReadFileManage current;
        public static ReadFileManage Current
        {
            get
            {
                if (current == null) current = new ReadFileManage();
                return current;
            }
        }
        public static void Execute(string path, Action<PoolFileStream> action = null, Action<PoolFileStream, FileThread> action1 = null)
        {
            var file = new FileThread(path);
            file.Open();
            file.Execute(action, action1);
            file.Close();
        }
    }
}
