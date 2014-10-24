using System;
using System.Collections.Generic;
using System.Windows;
using SLControls;
using SLControls.DataClientTools;
using SLControls.Editors;
using SLControls.ThemeManages;
using STComponse.CFG;
using Telerik.Windows.Controls;


namespace FD.Core.Test
{
    public partial class App : Application
    {

        public App()
        {
            var windows8TouchTheme = new Windows8TouchTheme();
            StyleManager.ApplicationTheme = windows8TouchTheme;
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
//            FW.FwConfig = await ActionHelper.CustomRequest<FwConfig>("PageAction", "GetConfig", null);
//            BaseMultiControl.EntitTypes = new List<Type> {
//                typeof(SYS_User),
////                typeof(SYS_Role),
//            };
//            ThemeFileManage.CurrentTheme = new TTheme { Code = "Default", Name = "测试主题" };
//
////            this.RootVisual = new LoginTest();
////            this.RootVisual = new MainPage();
////            this.RootVisual = new CalendarTest();
////            this.RootVisual = new TagSearch();
//            this.RootVisual = new MessageEditor{HeaderText = "新闻"};
//            return;
//            this.RootVisual = new MainPage();
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
