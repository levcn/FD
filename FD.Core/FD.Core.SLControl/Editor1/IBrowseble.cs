using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLControls.Editor1
{
    /// <summary>
    /// 可浏览
    /// </summary>
    public interface IBrowseble
    {
        /// <summary>
        /// 返回是否为浏览模式
        /// </summary>
        bool IsBrowsMode { get; set; }
    }
}
