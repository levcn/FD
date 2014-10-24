using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using StaffTrain.FwClass.DataClientTools;


namespace SLControls.ThemeManages
{
    public class ThemeFileManage
    {
        private const int ThemeFileOverTime = 1000;//单位秒
        /// <summary>
        /// 主题页面文件
        /// </summary>
        public class PageThemeFile
        {
            public PageThemeFile()
            {
                LoadTime = DateTime.Now;
            }

            /// <summary>
            /// 主题名称
            /// </summary>
            public string ThemeName { get; set; }

            /// <summary>
            /// 页面的内容
            /// </summary>
            public string PageName { get; set; }

            /// <summary>
            /// 页面的内容代码
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// 该页面的加载时间
            /// </summary>
            public DateTime LoadTime { get; set; }

            /// <summary>
            /// 内容反序列化后的对象
            /// </summary>
            public object DObject { get; set; }
        }

        /// <summary>
        /// 当前正在使用的主题名称
        /// </summary>
        public static TTheme CurrentTheme { get; set; }


        static List<PageThemeFile> ThemeFiles = new List<PageThemeFile>();

        /// <summary>
        /// 读取主题文件内容
        /// </summary>
        /// <param name="themeName"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public static async Task<PageThemeFile> GetThemeContent(string themeName, string pageName)
        {
            var page = ThemeFiles.FirstOrDefault(w => w.ThemeName == themeName && w.PageName == pageName);
            if (page == null || (DateTime.Now - page.LoadTime).TotalSeconds > ThemeFileOverTime)//如果缓存为空,或缓存过期
            {
                var text = await DataAccess.AsyncReadServerTxtFile(string.Format("Resourse/themes/{0}/{1}.txt", themeName, pageName));
                page = new PageThemeFile {
                    Content = text,
                    PageName = pageName,
                    ThemeName = themeName,
                };
                ThemeFiles.Add(page);
            }
            return page;
        }

        /// <summary>
        /// 读取主题控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="themeName"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public static async Task<T> GetThemeControl<T>(string themeName,string pageName) where T : class
        {
            try
            {
                var themeFile = await GetThemeContent(themeName, pageName);
                object obj = themeFile.DObject;
                if (obj == null)
                {
                    obj = XamlReader.Load(themeFile.Content);
                    themeFile.DObject = obj;
                }
                var grid = obj as T;
                return grid;
            }
            catch (Exception ee)
            {
                throw new Exception("读取主题控件文件出错:" + ee.Message);
            }
            return null;
        }
    }
}
