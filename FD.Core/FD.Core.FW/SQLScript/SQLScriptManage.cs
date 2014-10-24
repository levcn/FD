using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fw;
using Fw.Web;


namespace StaffTrain.SVFw.SQLScript
{
    public class SQLScriptManage
    {
        /// <summary>
        /// 数据库脚本存放的目录名 eg:DBScript
        /// </summary>
        public const string ScriptFolderName = "DBScript";

        /// <summary>
        /// 数据库脚本存放的目录c:\WebRoot\DBScript
        /// </summary>
        public string ScriptFolderFullName
        {
            get
            {
                return Path.Combine(WebConfigs.WebRootPath, ScriptFolderName);
            }
        }
        public static SQLScriptManage Current
        {
            get
            {
                return new SQLScriptManage();
            }
        }
        /// <summary>
        /// 返回安装需要的所有脚本
        /// </summary>
        /// <returns></returns>
        public List<string> GetScripts()
        {
            var scriptFolderFullName = ScriptFolderFullName;
            if (Directory.Exists(scriptFolderFullName))
            {
                var files = Directory.GetFiles(scriptFolderFullName);
                return files.Where(w => w.EndsWith(".sql", StringComparison.OrdinalIgnoreCase)).ToList().OrderBy(w=>w).ToList();
            }
            return new List<string>();
        }
    }
}
