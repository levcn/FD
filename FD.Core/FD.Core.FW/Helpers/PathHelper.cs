using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fw.Helpers
{
    public static class PathHelper
    {
        /// <summary>
        /// 返回目录重命名的最大数
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetMaxRenameNumber(string path)
        {
            return Enumerable.Range(1, 9999999).FirstOrDefault(w => !Directory.Exists(path + w));
        }
    }
}
