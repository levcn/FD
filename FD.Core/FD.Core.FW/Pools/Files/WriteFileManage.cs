using System;


namespace Fw.Pools.Files
{
    public class WriteFileManage
    {
        WriteFileManage()
        {
        }
        private static WriteFileManage current;
        public static WriteFileManage Current
        {
            get
            {
                if (current == null) current = new WriteFileManage();
                return current;
            }
        }
        public static void Execute(string path,Action<PoolFileStream> action = null,Action<PoolFileStream,FileThread> action1=null)
        {
            var file = new FileThread(path);
            file.WriteEnabled = true;
            file.Open();
            file.Execute(action,action1);
            file.Close();
        }
    }
}
