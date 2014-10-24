using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Fw.Caches;
using Fw.Extends;
using Fw.IO;
using Fw.Serializer;
using StaffTrain.SVFw.Web;


namespace Fw.Web
{
    /// <summary>
    /// 站点设置操作类
    /// </summary>
    public class WebConfigs
    {
        /// <summary>
        /// 返回web.config中的设置
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConfig(string configName)
        {
            return ConfigurationManager.AppSettings[configName];
        }

        static string webRootPath;
        /// <summary>
        /// 返回当前站点的根目录c:\wwwroot\
        /// </summary>
        public static string WebRootPath
        {
            get
            {
                if (webRootPath == null)
                {
                    if (HttpContext.Current != null)
                    {
                        webRootPath = HttpContext.Current.Server.MapPath("~/");
                    }
                    else
                    {
                        throw new Exception("当前会话为空,无法获取站点根目录.");
                    }
            }
                return webRootPath;
            }
        }
        /// <summary>
        /// 返回<see cref="FWConfigFileName"/>文件中指定的设置
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetFWConfig(string configName)
        {
            var firstOrDefault = FWConfigs.FirstOrDefault(w => string.Equals(w.Key, configName, StringComparison.OrdinalIgnoreCase));
            return firstOrDefault == null ? null : firstOrDefault.Value;
        }

        public static void SetConfig(string key,string value)
        {
            var fwConfigItems = FWConfigs;
            var item = fwConfigItems.FirstOrDefault(w => string.Equals(w.Key, key, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                fwConfigItems.Remove(item);
            }
            fwConfigItems.Add(new FWConfigItem{Key = key,Value = value});
            SaveConfig(fwConfigItems);
        }

        /// <summary>
        /// 返回设置到文件中
        /// </summary>
        /// <param name="configs"></param>
        public static void SaveConfig(List<FWConfigItem> configs)
        {
            FileHelper.WriteAllText(FWConfigFullPath, configs.ToXml());
        }

        /// <summary>
        /// 返回所有的设置
        /// </summary>
        public static List<FWConfigItem> FWConfigs
        {
            get
            {
                return CacheHelper.GetAdd("FWConfigs", () =>
                {
                    var combine = FWConfigFullPath;
                    var fwConfigs = combine;
                    if (File.Exists(fwConfigs))
                    {
                        return XmlHelper.LoadFromFile<List<FWConfigItem>>(fwConfigs) ?? new List<FWConfigItem>();
                    }
                    return new List<FWConfigItem>();
                },TimeSpan.FromMinutes(1));
            }
        }
        /// <summary>
        /// 返回当前的基本设置
        /// </summary>
        public static AppConfigs AppConfigs
        {
            get
            {
                var c = FWConfigs;
                if (c != null)
                {
                    return new AppConfigs(c);
                }
                return new AppConfigs();
            }
        }
        private static string FWConfigFullPath
        {
            get
            {
                return Path.Combine(WebRootPath, FWConfigFileName);
            }
        }

        public const string FWConfigFileName = "fw.config";
    }
}
