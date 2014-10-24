using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Fw.IO;
using Fw.Web;


namespace Fw.Helpers
{

    /// <summary>
    /// 系统升级
    /// </summary>
    public static class UpdateHelper
    {
        /// <summary>
        /// 删除SQL文件中的注释
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetCleardSql(string sql)
        {
            sql = Regex.Replace(sql, @"(--[\s\S]*?\n)|(\/\*[\s\S]*?\*\/)", "", RegexOptions.IgnoreCase);
            return sql;
        }
        /// <summary>
        /// 按GO把SQL分成多个
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<string> SplitByGo(string sql)
        {
            return Regex.Split(sql, @"[\n][\s]*?go[\s]*?\n", RegexOptions.IgnoreCase).ToList();
        }

        private static bool delay = false;
        /// <summary>
        /// 更新系统
        /// </summary>
        /// <param name="getCurrentState">返回当前进度</param>
        /// <param name="zipFile">c:\a.zip</param>
        /// <param name="siteRoot">c:\wwwroot</param>
        /// <param name="sqlConnection">sqldb</param>
        /// <param name="sqlFilePath">\sql</param>
        /// <param name="tempPath">c:\wwwroot\temp</param>
        public static void Update(Action<UpdateState> getCurrentState,string zipFile,string siteRoot,string sqlConnection,string sqlFilePath,string webFilePath,string tempPath)
        {
            var zipFileName = (new FileInfo(zipFile)).Name;
            zipFileName = zipFileName.Substring(0, zipFileName.Length - 4);
            var unzipTempPath = Path.Combine(tempPath, zipFileName);
            if (Directory.Exists(unzipTempPath)) DirectoryHelper.Delete(unzipTempPath);
            UpdateState us =new UpdateState();
            us.Total_StepCount = 3;
            us.Total_StepIndex = 0;
            //解压
            us.Total_StepIndex++;
            getCurrentState(us);
//            Thread.Sleep(1000);
            UnZipFile(zipFile, unzipTempPath, ref us, getCurrentState);
            Regex r = new Regex(@"[\s\S]*(?<n>[0-9]+).[a-zA-Z]+");
            var sqlFiles = Directory.GetFiles(Path.Combine(unzipTempPath, sqlFilePath)).Select(w =>
                                                                        {
                                                                            var fi = new FileInfo(w);
                                                                            return new {f = fi, match = r.Match(fi.Name)};
                                                                        }).Where(w=>w.match.Success).ToList();
            var files = sqlFiles.OrderBy(w => Convert.ToInt32(w.match.Groups["n"].Value)).Select(w=>w.f).ToList();
            if (delay) Thread.Sleep(4000);
            //更新数据库
            us.Total_StepIndex++;
            UpdateDataBase(getCurrentState, files, sqlConnection, ref us);
            if (delay) Thread.Sleep(4000);
            us.Total_StepIndex++;
            UpdateFiles(getCurrentState, ref us, Path.Combine(unzipTempPath, webFilePath), siteRoot);
            if (delay) Thread.Sleep(4000);
            us.Total_StepIndex = us.Total_StepCount;
            us.Current_StepCount = us.Current_StepIndex = 1;
        }
        /// <summary>
        /// 更新站点文件
        /// </summary>
        /// <param name="getCurrentState"></param>
        /// <param name="us"></param>
        /// <param name="unzipTempPath"></param>
        /// <param name="siteRoot"></param>
        private static void UpdateFiles(Action<UpdateState> getCurrentState, ref UpdateState us, string unzipTempPath, string siteRoot)
        {
            us.CurrentOperation = "更新文件";
            us.Current_StepCount = 1;
            us.Current_StepIndex = 1;
            DirectoryInfo source = new DirectoryInfo(unzipTempPath);
            DirectoryInfo target = new DirectoryInfo(siteRoot);
            DirectoryHelper.CopyFolder(source,target);
        }
        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="getCurrentState"></param>
        /// <param name="sqlFiles"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="us"></param>
        private static void UpdateDataBase(Action<UpdateState> getCurrentState, List<FileInfo> sqlFiles, string sqlConnection, ref UpdateState us)
        {
            var us1 = us;
            us1.CurrentOperation = "更新数据库";
            us1.Current_StepCount = sqlFiles.Count;
            us1.Current_StepIndex = 1;
            SqlConnection sc = new SqlConnection(sqlConnection);
            sc.Open();
            sqlFiles.ForEach(file =>
                                 {
                                     getCurrentState(us1);
                                     UpdateDataBaseWithSqlFile(file, sc);
//                                     if(delay)Thread.Sleep(5000);
                                     us1.Current_StepIndex++;
                                 });
            us1.Current_StepIndex = us1.Current_StepCount;
            sc.Close();
        }
        /// <summary>
        /// 一个SQL文件的数据更新操作
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sc"></param>
        private static void UpdateDataBaseWithSqlFile(FileInfo file, SqlConnection sc)
        {
            string commandText = File.ReadAllText(file.FullName);
            var commandTexts = SplitByGo(GetCleardSql(commandText)).Where(w=>!string.IsNullOrEmpty(w)).ToList();
            SqlCommand comm = new SqlCommand();
            comm.Connection = sc;
            commandTexts.ForEach(w =>
                                     {
                                         comm.CommandText = w;
                                         try
                                         {
                                             comm.ExecuteNonQuery();
                                         }
                                         catch (Exception e)
                                         {
                                             throw new Exception(string.Format("File:{0}\r\nSqlCommand:{1}\r\n{2}", file.FullName,w, e.Message));
                                         }
                                     });
        }
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="tempPath"></param>
        /// <param name="us"></param>
        /// <param name="getCurrentState"></param>
        private static void UnZipFile(string zipFile, string tempPath,ref UpdateState us, Action<UpdateState> getCurrentState)
        {
            var us1 = us;
            us1.Current_StepCount = 0;
            us1.Current_StepIndex = 0;
            ZipHelper zip = new ZipHelper();
            zip.UnZipDirectory(zipFile, tempPath, null, (a, b, c, d) =>
                                                            {
                us1.Current_StepCount = a;
                us1.Current_StepIndex = b;
                us1.CurrentOperation = string.Format("正在解压文件({0}%)...", c);
//                                                                if(delay)Thread.Sleep(1000);
                getCurrentState(us1);
            });
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        public class UpdateState
        {
            /// <summary>
            /// 当前操作名称
            /// </summary>
            public string CurrentOperation { get; set; }
            /// <summary>
            /// 所有的操作步骤
            /// </summary>
            public int Total_StepCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int Total_StepIndex { get; set; }
            public long Current_StepCount { get; set; }
            public long Current_StepIndex { get; set; }
            public bool HaveError { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
