using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fw.Extends
{
    public static class FileExtends
    {
        /// <summary>
        /// 返回文件,成功删除,或文件不存在,返回true,删除时出现错误,返回false
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool TryDeleteFile(this string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var fi = new FileInfo(path);
                    fi.Delete();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 返回扩展名exe,rar
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtendName(this string path)
        {
            var index = path.LastIndexOf(".");
            if (index >= 0)
            {
                return path.Substring(index + 1);
            }
            return "";
        }
    }
}
