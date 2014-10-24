using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fw
{
    public static class AppSetting
    {
        private const string _FileTempPath = "Resourse/Temp";

        /// <summary>
        /// c:\wwwroot\site1
        /// </summary>
        public static string SiteRoot;

        public static string LogFileRoot
        {
            get
            {
                return Path.Combine(SiteRoot, "Log");
            }
        }
        public static string FileTempPath
        {
            get
            {
                return "UploadTemp";
            }
        }

        public static string RunningLogFileRoot
        {
            get
            {
                return Path.Combine(SiteRoot, "Running");
            }
        }

        static string LogAbsoluteName
        {
            get
            {
                return DateTime.Now.ToString("yyyy/MM/dd") + ".log";
            }
        }
        /// <summary>
        /// 返回当前错误日志的文件全路径
        /// </summary>
        public static string CurrentErrorLogFilePath
        {
            get
            {
                return Path.Combine(LogFileRoot, LogAbsoluteName);
            }
        }
        /// <summary>
        /// 返回错误日志的根目录
        /// </summary>
        public static string ErrorLogFileRoot
        {
            get
            {
                return Path.Combine(LogFileRoot, "Error");
            }
        }
        public static string FileTempPhysicalPath
        {
            get { return ToFilePhysicalPath(_FileTempPath); }
        }
        private  static  string ToFilePhysicalPath(string path)
        {
            return string.Empty;
        }

        /// <summary>
        /// 保存页面设置的地址
        /// </summary>
        public static string PageConfigPath
        {
            get
            {
                return Path.Combine(SiteRoot, "Resourse", "PageConfig");
            }
        }

        public const string IPFilterFileName = "Filter.bin";

        public static string IPFilterPath { get; set; }

        public const string LisFileName = "Lis.bin";



        public const string VersionFileName = "Version.bin";

        /// <summary>
        /// 数据库是否可以连接
        /// </summary>
        public static bool DBConnection;

        public static string LisFileFullPath { get; set; }

        /// <summary>
        /// 当前服务器的CPUID
        /// </summary>
        public static string CPUID { get; set; }

        /// <summary>
        /// 正在运行的产品版本信息
        /// </summary>
        public static ProductVersion ProductVersion { get; set; }
    }
    public class ProductVersion
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Version { get; set; }
    }
}
