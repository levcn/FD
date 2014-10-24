using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SLControls.Collection;
using SLControls.DataClientTools;
using SLControls.Editors;
using SLControls.ThemeManages;
using StaffTrain.FwClass.DataClientTools;
using STComponse.CFG;
using Telerik.Windows.Controls;


namespace SLControls.PageConfigs
{
    /// <summary>
    /// 页面配置的管理(缓存,加载,保存等)
    /// </summary>
    public class PageConfigManage
    {
        static TList<PageConfig> ThemeFiles = new TList<PageConfig>();

        /// <summary>
        /// 读取页面设置
        /// </summary>
        /// <param name="themeName"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public static async Task<PageConfig> GetPageConfig(string themeName, string pageName)
        {
            
                var page = ThemeFiles.FirstOrDefault(w => w.ThemeCode == themeName && w.PageCode == pageName);
                if (page == null) //如果缓存为空,
                {
                    var text = await ActionHelper.CustomRequest<string>("PageAction", "ReadPageConfig", new object[] {themeName, pageName});
                    lock (ThemeFiles)
                    {
                        page = text.ToObject<PageConfig>();
                        var pageN = ThemeFiles.FirstOrDefault(w => w.ThemeCode == themeName && w.PageCode == pageName);

                        //确保多线程时,不是出现另外一个线程存入之后,此线程又存一次
                        if (pageN==null && page != null) ThemeFiles.Add(page);
                    }
                }
                return page;
            
        }
        
        /// <summary>
        /// 保存页面设置
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> SavePageConfigToServer(PageConfig pc)
        {
            return await ActionHelper.CustomRequest<bool>("PageAction", "SavePageConfig", new object[] { pc });
        }


        /// <summary>
        /// 把配置应用到控件上
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageConfig"></param>
        public static void SetPageConfig(DependencyObject page,PageConfig pageConfig)
        {
            if (pageConfig == null || pageConfig.ControlConfigs == null) return;
            var childs = page.ChildrenOfType<BaseMultiControl>().Where(w=>!w.IsSub).ToList();
            pageConfig.ControlConfigs.ForEach(w =>
            {
                var c = childs.FirstOrDefault(z => z.Name == w.Name);
                if (c == null)
                {
                    c = childs.FirstOrDefault(z => z.GetType().Name == w.Name);
                }
                if (c != null)
                {
//                    c.LoadConfig(w.Content);
                    c.ControlConfig = w;
//                    c.LoadConfig(w.ItemConfig);
                }
            });
        }

        /// <summary>
        /// 保存页面配置到服务器端
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static PageConfig SavePageConfig(BasePage page)
        {
            string themeName = "";
            
            if(page.PageConfig!=null)themeName = page.PageConfig.ThemeCode;
            if (string.IsNullOrWhiteSpace(themeName)) themeName = ThemeFileManage.CurrentTheme.Code;
            string pageCode = "";
            if (page.PageConfig != null) pageCode = page.PageConfig.PageCode ;
            if(string.IsNullOrWhiteSpace(pageCode))pageCode = page.PageCode;
            return SavePageConfig(page, themeName, pageCode);
        }

        /// <summary>
        /// 保存页面配置到服务器端
        /// </summary>
        /// <param name="page"></param>
        /// <param name="themeCode"></param>
        /// <param name="pageCode"></param>
        /// <returns></returns>
        public static PageConfig SavePageConfig(BasePage page, string themeCode, string pageCode)
        {
            PageConfig pc = new PageConfig();
            pc.PageCode = pageCode;
            pc.ThemeCode = themeCode;
            
            var childs = page.ChildrenOfType<BaseMultiControl>().Where(w=>!w.IsSub).ToList();
            pc.ControlConfigs = childs.Select(w =>
            {
//                ControlConfig cc = new ControlConfig {
//                    Content = w.SaveConfig(),
//                    Name = w.Name,
//                };
                return w.ControlConfig;
            }).ToList();
            ThemeFiles.LockBlock(z =>
            {
                z.RemoveAll(w => w.PageCode == pc.PageCode && w.ThemeCode == pc.ThemeCode);
                z.Add(pc);
            });
            
            return pc;
            //            return SavePageConfig(pc);
            //            var tcs = new TaskCompletionSource<PageConfig>();
            //            tcs.SetResult(pc);
            //            return pc;
            //            return await new Task<PageConfig>(() => pc);
        }

    }
}
