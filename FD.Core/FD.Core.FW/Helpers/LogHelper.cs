using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerFw.Helpers
{
    public static class LogHelper
    {
        public static void AddLog(string path, string text)
        {
            text = "\r\n----------------------------------\r\n" + DateTime.Now + "\r\n" + text;
            try
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Directory != null && !fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                File.AppendAllText(path, text);
            }
            catch (Exception e)
            {
                //throw e;
            }
        }
    }
}
