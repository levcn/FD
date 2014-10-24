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
using FD.Core.SLControl.Data;


namespace FD.Core.SLControl.NavigatorTools
{
    /// <summary>
    /// 菜单条目
    /// </summary>
    public interface IMenuItem : INode
    {
        Guid ID { get; set; }
        string PageUrl { get; set; }
        string ImageUrl { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        int OrderID { get; set; }
        string Memo { get; set; }
        string StyleName { get; set; }
    }
}
