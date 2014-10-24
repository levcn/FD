using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fw.Pools.Files
{
    public class FilePoolManage
    {
        public string Name { get; set; }
        public int Age{ get; set; }
        public int ID { get; set; }

        FilePoolManage()
        {
            
        }
        public static void Exec(string path, Action<PoolFileStream> action = null, Action<PoolFileStream, FileThread> action1 = null, bool autoClose = false)
        {
            Current.Execute(path, action, action1, autoClose);
        }
        /// <summary>
        /// 使用文件池指定指定的操作
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <param name="action1"></param>
        public void Execute(string path,Action<PoolFileStream> action = null,Action<PoolFileStream,FileThread> action1=null,bool autoClose = false)
        {
            PoolFile pf = GetPoolFile(path);
            pf.Execute(action,action1);
            if (autoClose) Close(pf);
        }

        public bool Close(PoolFile poolFile)
        {
            return Close(poolFile.Path);
        }
        /// <summary>
        /// 关闭指定文件的线程池,并把该文件池清除
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Close(string path)
        {
            var pf = GetPoolFile(path, false);
            if(pf!=null)
            {
                pf.Close();
                poolFiles.Remove(pf);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 从线程池中返回一个指定文件的线程池
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mustBeReturn">是否一定要返回,如果是且线程池中没有文件,则创建一个返回</param>
        /// <returns></returns>
        private PoolFile GetPoolFile(string path,bool mustBeReturn = true)
        {
            var file = poolFiles.FirstOrDefault(w => w.Path == path);
            if (file == null && mustBeReturn)
            {
                file = new PoolFile(path);
                poolFiles.Add(file);
            }
            return file;
        }
        /// <summary>
        /// 返回文件池的文件数量
        /// </summary>
        public int FileCount
        {
            get
            {
                return poolFiles.Count;
            }
        }
        /// <summary>
        /// 返回文件池中的文件名列表
        /// </summary>
        public List<string> FileNames
        {
            get
            {
                return poolFiles.Select(w => w.Path).ToList();
            }
        }
        List<PoolFile> poolFiles = new List<PoolFile>();
        private static FilePoolManage current;
        public static FilePoolManage Current
        {
            get
            {
                if(current==null)current=new FilePoolManage();
                return current;
            }
        }
    }
}
