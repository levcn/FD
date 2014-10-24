using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ProjectCreater.Settings;
using ProjectCreater.SObjects;
using ProjectCreater.Threading;


namespace ProjectCreater
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            PCThreadPool.Start();
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
//            MessageBox.Show(SettingConfig.StartPath, "");
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exceptionObject = e.ExceptionObject;
            var erwr = exceptionObject.GetType();
            var exception = (exceptionObject as Exception);
            WriteException(exception.GetBaseException().ToString());
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            WriteException(e.Exception.GetBaseException().ToString());
        }

        private static void WriteException(string errorStr)
        {
            try
            {
                MessageBox.Show(errorStr);
                var logPath = SettingConfig.Current.CurrentErrorLogPath;
                var dir = Path.GetDirectoryName(logPath);

                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.AppendAllText(logPath, string.Format(@"
----------------------------------------------
{0};
{1}", DateTime.Now, errorStr));
            }
            catch (Exception eee)
            {
                Application.Current.MainWindow.Dispatcher.BeginInvoke(new SObjectBaseEdit.tt(() =>
                {
                    Application.Current.Shutdown();
//                    MessageBox.Show(errorStr);
//                    MessageBox.Show(eee.GetBaseException().ToString());
                }));
                
            }
        }
    }
}
