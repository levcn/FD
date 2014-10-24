using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Editors;


namespace SLControls.PageConfigs
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
        public string ThemeCode{ get; set; }

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
