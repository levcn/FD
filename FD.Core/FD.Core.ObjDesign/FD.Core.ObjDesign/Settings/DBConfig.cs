using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCreater.Settings
{
    public static class DBConfigEx
    {
        public static string GetConnStr(this DBConfig config)
        {
            return string.Format("Data Source = {0};Initial Catalog = {1};User Id = {2};Password = {3};"
                    , config.ServerName
                    , config.DataBaseName
                    , config.UID
                    , config.PWD
                    );
        }
        public static string GetConnStrNoDB(this DBConfig config)
        {
            return string.Format("Data Source = {0};User Id = {1};Password = {2};"
                    , config.ServerName
                    , config.UID
                    , config.PWD
                    );
        }
    }
    public class DBConfig
    {
        public string ServerName { get; set; }
        public string UID { get; set; }
        public string PWD { get; set; }

        public string DataBaseName { get; set; }
    }

}
