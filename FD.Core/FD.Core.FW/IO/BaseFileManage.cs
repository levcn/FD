using Fw.Caches;
using Fw.Pools.Files;


namespace Fw.IO
{
    /// <summary>
    /// �ļ����ݿ����Ļ���
    /// </summary>
    public abstract class BaseFileManage
    {
        #region 

        
        /// <summary>
        /// ��ȡָ���ļ�������
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
        /// ��ȡָ���ļ�������
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void WriteFile(string path,string content)
        {
            CacheHelper.Remove(path);
            WriteByPath(path, content);
        }
        
        /// <summary>
        /// д��Ӳ��
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="content">����</param>
        public void WriteByPath(string path, string content)
        {
            CacheHelper.Remove(path);
            WriteFileManage.Execute(path, w => w.WriteAllText(content));
        }
        /// <summary>
        /// ��ȡ�ļ�����
        /// </summary>
        /// <param name="path">·��</param>
        public string ReadByPath(string path)
        {
            string re = "";
            WriteFileManage.Execute(path, w => re = w.ReadAllString());
            return re;
        }
        
        #endregion

        /// <summary>
        /// �����ļ����ϵĸ���ַ
        /// </summary>
        protected abstract string FileRootPath { get; }
    }
}