using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FD.Core.Test.Controls;
using FD.Core.Test.Entity;
using FD.Core.Test.Test;
using FD.Core.Test.Views.Sys;
using SLControls;
using SLControls.Controls;
using SLControls.DataClientTools;
using SLControls.Editors;
using SLControls.ThemeManages;
using SLTest;
using STComponse.CFG;
using Telerik.Windows.Controls;


namespace FD.Core.Test
{
    public partial class App : Application
    {
        static private App Current;
        static Grid g = new Grid();
        public App()
        {
            Current = this;
            StyleManager.ApplicationTheme = new Windows8TouchTheme();
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            ThemeFileManage.CurrentTheme = new TTheme { Code = "Default", Name = "测试主题" };
            FW.FwConfig = await ActionHelper.CustomRequest<FwConfig>("PageAction", "GetConfig", null);
            BaseMultiControl.EntitTypes = new List<Type> {
                typeof(SYS_User),
                typeof(SYS_RoleType),
            };

            RootVisual = g;
            SetCurrentPage(new LoginTest());
//            this.RootVisual = new TestControl1();
//            this.RootVisual = new RoleType();
//            this.RootVisual = new 保存成功提示();
//            this.RootVisual = new 动态添加测试();
//            this.RootVisual = new TestMControl2();
//                        this.RootVisual = new MainPage();
//            this.RootVisual = new CalendarTest();
//            this.RootVisual = new TagSearch();
//            this.RootVisual = new FD.Core.SLControl.Controls.UserSelector { };
            return;
            this.RootVisual = new MainPage();
        }

        public static void SetCurrentPage(UIElement ui)
        {
            g.Children.Clear();
            g.Children.Add(ui);
        }
        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // 如果应用程序是在调试器外运行的，则使用浏览器的
            // 异常机制报告该异常。在 IE 上，将在状态栏中用一个 
            // 黄色警报图标来显示该异常，而 Firefox 则会显示一个脚本错误。
            if (System.Diagnostics.Debugger.IsAttached)
            {

                // 注意: 这使应用程序可以在已引发异常但尚未处理该异常的情况下
                // 继续运行。 
                // 对于生产应用程序，此错误处理应替换为向网站报告错误
                // 并停止应用程序。
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");
                MessageBox.Show(errorMsg);
                //                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
