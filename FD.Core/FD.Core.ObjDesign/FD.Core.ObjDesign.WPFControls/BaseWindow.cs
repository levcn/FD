using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace WPFControls
{
    public class BaseWindow : Window
    {
        public BaseWindow()
        {
            Icon = MainUtils.IconImageSource;
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ShowInTaskbar = false;
            //            this.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!canClose) e.Cancel = true;
            base.OnClosing(e);
            IsClosing = true;
        }

        public bool IsClosing = false;
        /// <summary>
        /// 弹出模态窗口
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public bool? ShowDialog(Window ui)
        {
            Owner = ui;
            return ShowDialog();
        }

        private bool canClose = true;
        public void SetCanClose(bool value)
        {
            canClose = value;
        }
        /// <summary>
        /// 弹出模态窗口
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public bool? ShowDialog(UIElement ui)
        {
            Owner = GetWindow(ui);
            return ShowDialog();
        }
    }
}
