using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using STComponse.CFG;
using WPFControls.Ex;


namespace ProjectCreater.Settings
{
    /// <summary>
    /// 版本管理类
    /// </summary>
    public class VersionManage
    {
        VersionManage()
        {
            
        }
        public static VersionManage Current
        {
            get
            {
                if (current==null)current = new VersionManage();
                return current;
            }
        }
        private List<FwConfig> _versions;
        private FwConfig currentVersion;
        private static VersionManage current;

        /// <summary>
        /// 使用指定的设置把内存中的设置替换
        /// </summary>
        /// <param name="config"></param>
        public void ReplaceConfig(FwConfig config)
        {
            var ver = _versions.FirstOrDefault(w => w.VersionNumber == config.VersionNumber);
            if (ver != null)
            {
                var index = _versions.IndexOf(ver);
//                _versions.Remove(ver);
                _versions[index] = (config);
            }
        }
        /// <summary>
        /// 版本列表
        /// </summary>
        public ReadOnlyCollection<FwConfig> Versions
        {
            get
            {
                if (_versions == null)
                {
                    _versions = GetVersions();
                }
                return _versions.AsReadOnly();
            }
        }

        /// <summary>
        /// 反回所有的版本列表
        /// </summary>
        /// <returns></returns>
        private List<FwConfig> GetVersions()
        {
            var dirStr = SettingConfig.Current.CodeFileSavePath;
            if (Directory.Exists(dirStr))
            {
                var re = Directory.GetFiles(dirStr)
                        .Select(w => new FileInfo(w))
                        .Where(w => w.Extension.Equals(".cfg", StringComparison.OrdinalIgnoreCase))
                        .Select(w => File.ReadAllText(w.FullName).ToObject<FwConfig>())
                        .Where(w => w != null)
                        .OrderByDescending(w => w.VersionNumber)
                        .ToList();
                re.ForEach(w =>
                {
                    w.DataObjects.ForEach(z => {
                                                   if (z.ID == Guid.Empty) z.ID = Guid.NewGuid();
                        z.Property.ForEach(x =>
                        {
                            if (x.ID == Guid.Empty) x.ID = Guid.NewGuid();
                        });
//                        z.Relation.ForEach(x =>
//                        {
//                            if (x.ID == Guid.Empty) x.ID = Guid.NewGuid();
//                        });
                    });
                    w.DictObject.ForEach(z =>
                    {
                        if (z.ID == Guid.Empty) z.ID = Guid.NewGuid();
                        z.Property.ForEach(x =>
                        {
                            if (x.ID == Guid.Empty) x.ID = Guid.NewGuid();
                        });
                    });
                });
                return re;
            }
            return new List<FwConfig>();
        }

        /// <summary>
        /// 返回最新版本
        /// </summary>
        /// <returns></returns>
        public FwConfig GetLastVersion()
        {
            return Versions.FirstOrDefault();
        }

        /// <summary>
        /// 返回一个新创建的版本
        /// </summary>
        /// <returns></returns>
        public FwConfig GetNewVersion()
        {
            var fwConfig = (GetLastVersion() ?? new FwConfig { VersionNumber = 0, VersionName = "1.0版本" });
            var newVersion = fwConfig.ToJson().ToObject<FwConfig>();
            newVersion.Remark = "";
            newVersion.VersionNumber++;
            return newVersion;
        }

        public void AddNewVersion(FwConfig _newVersion)
        {
            _versions.Add(_newVersion);
            _versions = _versions.OrderByDescending(w => w.VersionNumber).ToList();
            SaveFwConfig(_newVersion);
        }

        public string GetConfigFileContent(FwConfig config)
        {
            var filePath = Path.Combine(SettingConfig.Current.CodeFileSavePath, GetConfigFileName(config));
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return null;
        }
        /// <summary>
        /// 保存一个版本文件
        /// </summary>
        /// <param name="config"></param>
        public void SaveFwConfig(FwConfig config,bool isTemp = false)
        {
            CheckAndCreateFolder();
            string json = config.ToJson();
            var fileName = GetConfigFileName(config, isTemp);
            var path = Path.Combine(SettingConfig.Current.CodeFileSavePath, fileName);
            File.WriteAllText(path, json);
        }

        private string GetConfigFileName(FwConfig config, bool isTemp=false)
        {
            var format = "{0}.cfg";
            if (isTemp) format += "_";
            var fileName = string.Format(format, config.VersionNumber);
            return fileName;
        }

        /// <summary>
        /// 删除所有临时文件
        /// </summary>
        public void DeleteAllTemp()
        {
            Directory.GetFiles(SettingConfig.Current.CodeFileSavePath)
                .Where(w => w.EndsWith("_"))
                .ToList()
                .ForEach(w =>
                {
                    File.Delete(w);
                });
        }
        /// <summary>
        /// 删除所有临时文件
        /// </summary>
        public List<FwConfig> GetTempConfigs()
        {
            if (Directory.Exists(SettingConfig.Current.CodeFileSavePath))
            {
                return Directory.GetFiles(SettingConfig.Current.CodeFileSavePath)
                        .Select(w => new FileInfo(w))
                        .Where(w => w.FullName.EndsWith("_"))
                        .Select(w => File.ReadAllText(w.FullName).ToObject<FwConfig>())
                        .Where(w => w != null)
                        .OrderByDescending(w => w.VersionNumber)
                        .ToList();
            }
            return new List<FwConfig>();
        }
        /// <summary>
        /// 返回配置所对应的临时文件的配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public  FwConfig GetTempConfig(FwConfig config)
        {
            var file = GetConfigFileName(config, true);
            if (File.Exists(file))
            {
                return File.ReadAllText(file).ToObject<FwConfig>();
            }
            return null;
        }
        /// <summary>
        /// 验证版本文件保存目录是否存在并创建
        /// </summary>
        private static void CheckAndCreateFolder()
        {
            var dirStr = SettingConfig.Current.CodeFileSavePath;
            if (!Directory.Exists(dirStr))
            {
                Directory.CreateDirectory(dirStr);
            }
        }

        public void RefreshVersionList()
        {
            _versions = null;
        }

        /// <summary>
        /// 当前正在查看的版本
        /// </summary>
        public FwConfig CurrentVersion
        {
            get
            {
                return currentVersion;
            }
            set
            {
                currentVersion = value;
            }
        }
    }
}
