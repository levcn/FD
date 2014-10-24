using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fw.Extends;


namespace Fw.IO
{
    public static class FileHelper
    {
        public static void TryDeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                
            }
        }
        public static void CreateFolderByFilePath(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
        }
        /// <summary>
        /// "图片文件(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp"
        /// </summary>
        /// <param name="extendNames"></param>
        /// <returns></returns>
        public static string GetFilter(string name,string[] extendNames)
        {
            var names = extendNames.Select(w => "*." + w).ToList();
            return string.Format("{0}({1})|{2}", name, names.Serialize(","), names.Serialize(";"));
        }

        public static void WriteAllText(string fwConfigFullPath, string content)
        {
            File.WriteAllText(fwConfigFullPath,content,Encoding.UTF8);
        }
    }
}
