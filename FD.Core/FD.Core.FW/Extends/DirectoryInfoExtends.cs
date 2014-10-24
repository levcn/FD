using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerFw.Extends
{
    public static class DirectoryInfoExtends
    {
        public static void MoveTo1(this DirectoryInfo di, string dirPath)
        {
            di.MoveTo2(dirPath);
            di.Delete1();
        }
        public static void Delete1(this DirectoryInfo di)
        {
            di.GetFiles().ToList().ForEach(w => w.Delete());
            di.GetDirectories().ToList().ForEach(w => w.Delete1());
            di.Delete(false);
        }
        static void MoveTo2(this DirectoryInfo di, string dirPath)
        {
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            di.GetFiles().ToList().ForEach(w =>
            {
                var newPath = Path.Combine(dirPath, w.Name);
                if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                }
                w.MoveTo(newPath);
            });
            di.GetDirectories().ToList().ForEach(w => w.MoveTo1(Path.Combine(dirPath, w.Name)));
        }
    }
}
