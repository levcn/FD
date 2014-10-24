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
    /// 导航的条目
    /// </summary>
    public class NavigatorItem
    {
        public ISwitchable SwitchableItem { get; set; }
        public object StateData { get; set; }
    }
}
