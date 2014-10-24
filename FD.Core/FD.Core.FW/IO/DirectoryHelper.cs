using System;
using System.IO;
using System.Linq;


namespace Fw.IO
{
    public class DirectoryHelper
    {
        /// <summary>
        /// �ݹ�ɾ��ָ����Ŀ¼
        /// </summary>
        /// <param name="dir"></param>
        public static void Delete(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(d);//ֱ��ɾ�����е��ļ�   
                    }
                    else
                        Delete(d);//�ݹ�ɾ�����ļ���   
                }
                Directory.Delete(dir);//ɾ���ѿ��ļ���  
            }
        }
//        /// <summary>
//        /// ��һ��Ŀ¼�����ݸ��Ƶ�����һ���ط�
//        /// </summary>
//        /// <param name="source"></param>
//        /// <param name="target"></param>
//        public static void CopyFolderContent(DirectoryInfo source, DirectoryInfo target)
//        {
//            source.GetDirectories().ToList().ForEach();
//        }
        /// <summary>
        /// ��һ��Ŀ¼���Ƶ�����һ���ط�
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            var files = source.GetFiles().ToList();
            if (!target.Exists) target.Create();
            var targetFiles = target.GetFiles().ToList();
            files.ForEach(w => 
            {
                var tar = targetFiles.FirstOrDefault(z => z.Name.Equals(w.Name, StringComparison.OrdinalIgnoreCase));
                if (tar != null) tar.Delete();
                w.CopyTo(Path.Combine(target.FullName, w.Name));
            });
            var targetDirs = target.GetDirectories().ToList();
            var sourceDirs = source.GetDirectories().ToList();
            sourceDirs.ForEach(w =>
            {
                var tar = targetDirs.FirstOrDefault(z => z.Name.Equals(w.Name, StringComparison.OrdinalIgnoreCase));
//                if (tar == null)
//                {
//                    
//                }
                var t = new DirectoryInfo(Path.Combine(target.FullName, w.Name));
                CopyFolder(w, t);
            });
        }
        /// <summary>
        /// ɾ����Ŀ¼
        /// </summary>
        /// <param name="dir"></param>
        public void DeletePEmptyFolder(string dir)
        {
            if (Directory.GetDirectories(dir).Length == 0)
            {
                string pdir = Directory.GetParent(dir).FullName;
                if (Directory.Exists(dir))
                    Directory.Delete(dir);
                DeletePEmptyFolder(pdir);
            }
        } 
    }
}