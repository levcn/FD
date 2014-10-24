using Fw.Caches;
using Fw.Pools.Files;


namespace Fw.IO
{
    /// <summary>
    /// 文件数据库管理的基类
    /// </summary>
    public abstract class BaseFileManage
    {
        #region 

        
        /// <summary>
        /// 读取指定文件的内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadFile(string path)
        {
            var val = CacheHelper.Get<string>(path);
            if (val == null)
            {
                ReadFileManage.Execute(path, w => val = w == null ? "" : w.ReadAllString());
                CacheHelper.Add(path, val);
            }
            return val;
        }
        /// <summary>
        /// 读取指定文件的内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void WriteFile(string path,string content)
        {
            CacheHelper.Remove(path);
            WriteByPath(path, content);
        }
        
        /// <summary>
        /// 写入硬盘
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public void WriteByPath(string path, string content)
        {
            CacheHelper.Remove(path);
            WriteFileManage.Execute(path, w => w.WriteAllText(content));
        }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">路径</param>
        public string ReadByPath(string path)
        {
            string re = "";
            WriteFileManage.Execute(path, w => re = w.ReadAllString());
            return re;
        }
        
        #endregion

        /// <summary>
        /// 保存文件集合的跟地址
        /// </summary>
        protected abstract string FileRootPath { get; }
    }
}