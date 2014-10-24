using System;
using System.IO;
using System.Linq;
using System.Text;
using Fw;
using Fw.ActionMethod;
using Fw.DataAccess;
using Fw.IO;
using STComponse.Annotations;
using STComponse.CFG;


namespace StaffTrain.SVFw.UIPage
{
    /// <summary>
    /// 框架中关于页面的方法
    /// </summary>
    public class PageAction : BaseController
    {
        /// <summary>
        /// 保存页面设置
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool SavePageConfig(PageConfig pc)
        {
            var path = GetPageConfigFilePath(pc);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists && fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            File.WriteAllText(path,pc.ToJson());
            return true;
        }

        /// <summary>
        /// 读取页面设置
        /// </summary>
        /// <returns></returns>
        public string ReadPageConfig(string themeName,string fileName)
        {
            var path = GetPageConfigFilePath(themeName, fileName);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "";
        }

        /// <summary>
        /// 返回数据的设置
        /// </summary>
        /// <returns></returns>
        public FwConfig GetConfig()
        {
            return DataAccessFactory.FwConfig;
        }
        private static string GetPageConfigFilePath(PageConfig pc)
        {
            if(pc == null)throw new Exception("参数pc:PageConfig为空");
            if (string.IsNullOrWhiteSpace(pc.PageCode))
            {
                throw new Exception("页面设置中Name不能为空");
            }
            return GetPageConfigFilePath(pc.ThemeCode ?? "Default", pc.PageCode);
        }
        /// <summary>
        /// 返回一个页面配置文件的绝对路径.
        /// </summary>
        /// <returns></returns>
        private static string GetPageConfigFilePath(string themeName,string pageName)
        {
            return Path.Combine(AppSetting.PageConfigPath, themeName, pageName, "Page.cfg");
        }

        /// <summary>
        /// 保存错误日志
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool ReportError([NotNull] Error error)
        {
            if (error == null) throw new ArgumentNullException("error");
            var filePath = AppSetting.CurrentErrorLogFilePath;
            FileHelper.CreateFolderByFilePath(filePath);
            File.AppendAllText(filePath,error.ToJson() + "\r\n");
            return true;
        }

        /// <summary>
        /// 返回数据库是否可以打开
        /// </summary>
        /// <returns></returns>
        public bool DBConnection()
        {
            return AppSetting.DBConnection;
        }
    }
    /// <summary>
    /// 错误实体
    /// </summary>
    public class Error
    {
        public Error()
        {
            Date = DateTime.Now;
        }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }
    }
}
