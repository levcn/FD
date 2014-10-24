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
using Telerik.Windows.Controls;


namespace SLControls.helpers
{
    public class AlertHelper
    {
        public static void Alert(FrameworkElement _this, string content, Action ok = null, Action cancel = null, string title = "提示")
        {
            RadAlert radConfirm = new RadAlert { };
            bool executed = false;
            radConfirm.Ok += (s, ee) =>
            {
                executed = true;
                if (ok != null) ok();
            };
//            radConfirm.Cancel += (s, ee) =>
//            {
//                executed = true;
//                if (cancel != null) cancel();
//            };
            radConfirm.Unloaded += (s, e) =>
            {
                if (!executed)
                {
                    if (cancel != null) cancel();
                }
            };
            RadWindow.ConfigureModal(radConfirm, new DialogParameters { Content = content, Header = title, CancelButtonContent = "取消", OkButtonContent = "确定", Owner = _this.ParentOfType<ContentControl>() });
        }
        public static void Confirm(FrameworkElement _this,string content, Action ok, Action cancel=null, string title = "提示")
        {
            RadConfirm radConfirm = new RadConfirm { };
            bool executed = false;
            radConfirm.Ok += (s, ee) =>
            {
                executed = true;
                ok();
            };
            radConfirm.Cancel += (s, ee) =>
            {
                executed = true;
                if (cancel != null) cancel();
            };
            radConfirm.Unloaded += (s, e) =>
            {
                if (!executed)
                {
                    if (cancel!=null) cancel();
                }
            };
            RadWindow.ConfigureModal(radConfirm, new DialogParameters { ModalBackground = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100)), Content = content, Header = title, CancelButtonContent = "取消", OkButtonContent = "确定", Owner = _this.ParentOfType<ContentControl>() });
        }
    }
}
