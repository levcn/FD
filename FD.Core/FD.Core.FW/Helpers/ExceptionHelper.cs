using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fw;
using Fw.Entity;


namespace StaffTrain.SVFw.Helpers
{
    /// <summary>
    /// 服务器日志类
    /// </summary>
    public class ExceptionHelper
    {
        private static Dictionary<string, string> _exceptionToMsg;
        public static Dictionary<string, string> ExceptionToMsg
        {
            get
            {
                return _exceptionToMsg ?? (_exceptionToMsg = new Dictionary<string, string>
                                                   {
                                                       {"System.Data.SqlClient.SqlException", ""}
                                                   });
            }
        }

        public static void ExceptionOper(Exception ee, ResultData resultData)
        {
            if (resultData == null) return;
            var errorMsg = string.Format("{0}{1}{2}{1}", ee.Message, Environment.NewLine, ee.StackTrace); ;
            if (ee is System.Data.SqlClient.SqlException)
            {
                var eSql = (ee as System.Data.SqlClient.SqlException);
                resultData.FriendlyErrorMsg = eSql.Message;
                errorMsg = string.Format("{0}{1}数据库错误编号：{2}", errorMsg, Environment.NewLine, eSql.Number);
            }
            else
            {

            }
            var folderFullName = Path.Combine(AppSetting.SiteRoot, string.Format("log/errors/{0}/{1}", DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists(folderFullName)) Directory.CreateDirectory(folderFullName);
            var logpath = Path.Combine(folderFullName,"error.log");
            var errorLog = new FileStream(logpath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            var sw = new StreamWriter(errorLog);
            errorLog.Seek(0,SeekOrigin.End);
            sw.Write(errorMsg);
            sw.WriteLine("==============================================================");
            sw.Flush();
        }
    }
}
