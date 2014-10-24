using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectCreater.SObjects
{
    /// <summary>
    /// TextMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class TextMessageBox : Window
    {
        public TextMessageBox()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public bool? ShowDialog(UIElement ui)
        {
            Owner = GetWindow(ui);
            return ShowDialog();
        }
        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof (string), typeof (TextMessageBox), new PropertyMetadata(default(string)));

        public string Text
        {
            get
            {
                return (string) GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
