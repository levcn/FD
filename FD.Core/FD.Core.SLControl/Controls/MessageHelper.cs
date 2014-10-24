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

namespace SLControls.Controls
{
    /// <summary>
    /// 消息框帮助类
    /// </summary>
    public class MessageHelper
    {
        /// <summary>
        /// 显示一般消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public static void Info(string message,string title = "提示消息")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }
    }
}
