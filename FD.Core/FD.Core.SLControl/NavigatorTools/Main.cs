using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.helpers;
using StaffTrain.FwClass.DataClientTools.Configs;
using Telerik.Windows.Controls;


namespace StaffTrain.FwClass.NavigatorTools
{
    public static class Main
    {
        public static string ShortDateTimeFormat = "yyyy年MM月dd日";
        public static string LongDateTimeFormat = "yyyy年MM月dd日 hh:mm:ss";
        /// <summary>
        /// -2 认证信息丢失,-1 正式版本,-3 正式版本但是CPU编号不一样
        /// </summary>
        public static int RemainDay;//

        public static void AddResourse(string key, object _value)
        {
            if (Application.Current.Resources.Contains(key)) Application.Current.Resources.Remove(key);
            Application.Current.Resources.Add(key, _value);
        }

        public static bool HaveResourse(string key)
        {
            return Application.Current.Resources.Contains(key);
        }


        /// <summary>
        /// 确认框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="action"></param>
        public static void Confirm(FrameworkElement fe,bool check, string title, Action action)
        {
            if (check)
            {
                AlertHelper.Alert(fe, title, ()=>
                    {
                            action();
                    });
            }
        }

        /// <summary>
        /// 打开网页
        /// </summary>
        /// <param name="url"></param>
        public static void OpenWebPage(string url)
        {
            if (Application.Current.IsRunningOutOfBrowser)
            {
                HyperlinkButton hb = new HyperlinkButton { NavigateUri = new Uri(url,UriKind.RelativeOrAbsolute) };
                HyperlinkButtonAutomationPeer h = new HyperlinkButtonAutomationPeer(hb);
                h.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
                IInvokeProvider iprovider = h;
                iprovider.Invoke();
            }
            else
            {
                HtmlPage.Window.Navigate(new Uri(url, UriKind.RelativeOrAbsolute), "_blank");
            }
        }
        public static void CloseBrowser()
        {

            {
                HtmlPage.Window.Invoke("CloseWindow", "");
            }
        }
        public static bool IsDesignMode(DependencyObject d)
        {
            return DesignerProperties.GetIsInDesignMode(d);
        }

        public static void FullScreen(Action pAction = null)
        {
            if (Application.Current.Host.Content.IsFullScreen) return;
            Application.Current.Host.Content.IsFullScreen = true;
            if (pAction != null) pAction();
        }
        public static void ExitFullScreen()
        {
            if (Application.Current.Host.Content.IsFullScreen)
            {
                Application.Current.Host.Content.IsFullScreen = false;
            }
        }
    }
}
