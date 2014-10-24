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
using System.Windows.Threading;
using SLControls.Threads;
using Telerik.Windows.Controls;


namespace FD.Core.SLControl.Extends
{
    public static class ButtonExtends
    {
        /// <summary>
        /// 显示当前控件上级的RadBusy控件
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="text"></param>
        public static void ShowBusyMessageLayer(this DependencyObject owner, string text = "正在保存...")
        {
            var p = owner.GetVisualParent<RadBusyIndicator>();
            if (p != null)
            {
                p.BusyContent = text;
                p.IsBusy = true;
            }
        }

        /// <summary>
        /// 显示当前控件上级的RadBusy控件
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="text"></param>
        public static void HiddenBusyMessageLayer(this DependencyObject owner)
        {
            var p = owner.GetVisualParent<RadBusyIndicator>();
            if (p != null)
            {
                p.IsBusy = false;
            }
        }

        /// <summary>
        /// 显示当前控件上级的RadBusy控件
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="text"></param>
        /// <param name="msec"></param>
        /// <param name="closeAfterAction"></param>
        public static
        void ShowTipMessageLayer(this DependencyObject owner, string text = "保存成功。", int msec = 1000, Action closeAfterAction = null)
        {
            var p = owner.GetVisualParent<RadBusyIndicator>();
            if (p != null)
            {
                p.BusyContent = text;
                p.IsBusy = true;
                ThreadHelper.DelayRun(() =>
                {
//                    PopupPlacement.GetPopupPlacement()
                    owner.Dispatcher.BeginInvoke(() =>
                    {

                        p.IsBusy = false;
                        ThreadHelper.DelayRun(() =>
                        {
                            owner.Dispatcher.BeginInvoke(() =>
                            {

                                if (closeAfterAction != null) closeAfterAction();
                            });
                        }, 300, "sdewe123");
                    });
                }, msec, "sdewea9");
            }
        }
    }
}
