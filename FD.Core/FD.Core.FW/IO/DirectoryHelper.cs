using System;
using System.IO;
using System.Linq;


namespace Fw.IO
{
    public class DirectoryHelper
    {
        /// <summary>
        /// 递归删除指定的目录
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
                        File.Delete(d);//直接删除其中的文件   
                    }
                    else
                        Delete(d);//递归删除子文件夹   
                }
                Directory.Delete(dir);//删除已空文件夹  
            }
        }
//        /// <summary>
//        /// 把一个目录的内容复制到别外一个地方
//        /// </summary>
//        /// <param name="source"></param>
//        /// <param name="target"></param>
//        public static void CopyFolderContent(DirectoryInfo source, DirectoryInfo target)
//        {
//            source.GetDirectories().ToList().ForEach();
//        }
        /// <summary>
        /// 把一个目录复制到别外一个地方
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
        /// 删除空目录
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