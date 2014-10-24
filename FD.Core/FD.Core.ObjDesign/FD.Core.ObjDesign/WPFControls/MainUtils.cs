using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFControls.Properties;


namespace WPFControls
{
    public class MainUtils
    {
        public static void SetClipboardText(string text)
        {
            System.Windows.Forms.Clipboard.SetText(text);

        }

        /// <summary>
        /// 项目的图标地址
        /// </summary>
        public static ImageSource IconImageSource
        {
            get
            {
                return Imaging.CreateBitmapSourceFromHIcon(Icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());            
            }
        }

        /// <summary>
        /// 项目的图标
        /// </summary>
        public static Icon Icon
        {
            get
            {
                return Resources._1;
            }
        }

        private static bool? inDesignMode = null;

        /// <summary>
        /// 判断是设计器还是程序运行
        /// </summary>
        public static bool InDesignMode
        {
            get
            {
                if (inDesignMode == null)
                {
#if SILVERLIGHT || SL
                    inDesignMode = DesignerProperties.IsInDesignTool;
#else
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    inDesignMode = (bool) DependencyPropertyDescriptor.FromProperty(prop, typeof (FrameworkElement)).Metadata.DefaultValue;

                    if (!inDesignMode.GetValueOrDefault(false) && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal)) inDesignMode = true;
#endif
                }

                return inDesignMode.GetValueOrDefault(false);
            }
        }
        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="window"></param>
        /// <param name="message"></param>
        public static void ShowMessageBoxError(Window window, string message)
        {
            MessageBox.Show(window, message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="window"></param>
        /// <param name="message"></param>
        public static void ShowMessageBoxInfo(Window window,string message)
        {
            MessageBox.Show(window, message, "消息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult MessageBoxYesNo(UIElement uiElement, string title, string content)
        {
            return MessageBox.Show(Window.GetWindow(uiElement), content, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
