using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fw.Web;


namespace StaffTrain.SVFw.Web
{
    public class FWConfigItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AppConfigs
    {
        internal AppConfigs(List<FWConfigItem> config)
        {
            Init(config);
        }

        private void Init(List<FWConfigItem> fwConfigs)
        {
            IsInstalled = GetValue(fwConfigs, "IsInstalled") == "1";
            SqlServerIP = GetValue(fwConfigs, "SqlServerIP");
            SqlServerUID = GetValue(fwConfigs, "SqlServerUID");
            SqlServerPWD = GetValue(fwConfigs, "SqlServerPWD");
            SqlServerDBName = GetValue(fwConfigs, "SqlServerDBName");
        }

        private string GetValue(List<FWConfigItem> fwConfigs, string isinstalled)
        {
            var item = fwConfigs.FirstOrDefault(w => string.Equals(w.Key, isinstalled, StringComparison.OrdinalIgnoreCase));
            return item == null?null:item.Value;
        }

        internal AppConfigs()
        {
            
        }
        /// <summary>
        /// 是否已经安装
        /// </summary>
        public bool IsInstalled { get; set; }

        /// <summary>
        /// SQL IP
        /// </summary>
        public string SqlServerIP { get; set; }

        /// <summary>
        /// SQL用户名
        /// </summary>
        public string SqlServerUID { get; set; }

        /// <summary>
        /// SQL密码
        /// </summary>
        public string SqlServerPWD { get; set; }
        
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string SqlServerDBName { get; set; }


    }
}
