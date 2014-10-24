using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;
using WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation;


namespace SLControls.Editors
{
    /// <summary>
    /// 默认的窗口控件
    /// </summary>
    public class BaseWindow : RadWindow
    {
        public BaseWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ModalBackground = new SolidColorBrush(Color.FromArgb(50, 50, 50, 50));
        }
    }
}
