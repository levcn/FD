using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using STComponse.CFG;


namespace ProjectCreater.Settings
{
    /// <summary>
    /// 软件设置
    /// </summary>
    public class SettingConfig
    {
        private static SettingConfig current;

        public SettingConfig()
        {
            UserHabit = new UserHabit();
            CodeStyle = new CodeStyle();
        }

        public static SettingConfig Current
        {
            get
            {
                if (current == null)
                {
                    var startPath = StartPath;
                    var re = LoadConfig();
                    if (re == null)
                    {
                        re = new SettingConfig {
                            LogPath = Path.Combine(startPath, "log"),
                            DbConfigs = new List<DBConfig> {
                                new DBConfig {
                                    ServerName = "192.168.32.1",
                                    UID = "sa",
                                    PWD = "zdfd",
                                    DataBaseName = "CQ",
                                }
                            }
                        };
                    }
                    re.CodeFileSavePath = Path.Combine(startPath, "Config");
//                    MessageBox.Show(re.CodeFileSavePath, "");
                    current = re;
                }
                return current;
            }
        }

        public static string StartPath
        {
            get
            {
                return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            }
        }

        private static SettingConfig LoadConfig()
        {
            var path = Path.Combine(StartPath, ConfigFileName);
            if (File.Exists(path)) return File.ReadAllText(path).ToObject<SettingConfig>();
            return null;
        }

        public void Save()
        {
            var path = Path.Combine(StartPath, ConfigFileName);
            File.WriteAllText(path,this.ToJson());
        }

        internal static string ConfigFileName
        {
            get
            {
                return "config.txt";
            }
        }

        public List<DBConfig> DbConfigs { get; set; }
        /// <summary>
        /// C#代码的默认保存地址
        /// </summary>
        public string CodeFileSavePath { get; set; }

//        public string StartPath { get; set; }
        public string LogPath { get; set; }

        public UserHabit UserHabit { get; set; }
        public CodeStyle CodeStyle { get; set; }



        /// <summary>
        /// 当前错误文件地址
        /// </summary>
        public string CurrentErrorLogPath
        {
            get
            {
                
                return Path.Combine(StartPath,"LogPath", DateTime.Now.ToString("Err_yyyy-MM-dd")+".txt");
            }
        }
    }
}
