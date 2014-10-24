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

namespace StaffTrain.FwClass.NavigatorTools
{
    /// <summary>
    /// 可以切换页面,并保存页面的状态
    /// </summary>
    public interface ISwitchable
    {
        object StateData { get; set; }
        IControlContainer ParentControl { get; set; }
        object Args { get; set; }
    }
}
