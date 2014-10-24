using System.Collections.Generic;


namespace StaffTrain.SVFw.UIPage
{
    /// <summary>
    /// 页面设置
    /// </summary>
    public class PageConfig
    {
        public PageConfig()
        {
            ControlConfigs = new List<ControlConfig>();
        }
        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageCode { get; set; }

        /// <summary>
        /// 主题名称
        /// </summary>
        public string ThemeCode { get; set; }

        /// <summary>
        /// 页面中控件的配置列表
        /// </summary>
        public List<ControlConfig> ControlConfigs { get; set; }


        /// <summary>
        /// 页面中使用的对象的配置列表
        /// </summary>
        public List<PageObjectConfig> ObjectConfigs { get; set; }
    }
}