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
using ProjectCreater.Settings;
using WPFControls;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// AddNewVersionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddNewVersionWindow
    {
        public AddNewVersionWindow()
        {
            InitializeComponent();
            Title = "版本属性";
        }

        public static readonly DependencyProperty VersionNumberProperty =
                DependencyProperty.Register("VersionNumber", typeof (int), typeof (AddNewVersionWindow), new PropertyMetadata(default(int)));

        public int VersionNumber
        {
            get
            {
                return (int) GetValue(VersionNumberProperty);
            }
            set
            {
                SetValue(VersionNumberProperty, value);
            }
        }
        public static readonly DependencyProperty RemarkProperty =
                DependencyProperty.Register("Remark", typeof (string), typeof (AddNewVersionWindow), new PropertyMetadata(default(string)));

        public string Remark
        {
            get
            {
                return (string) GetValue(RemarkProperty);
            }
            set
            {
                SetValue(RemarkProperty, value);
            }
        }
        public static readonly DependencyProperty VersionNameProperty =
                DependencyProperty.Register("VersionName", typeof (string), typeof (AddNewVersionWindow), new PropertyMetadata(default(string)));

        public string VersionName
        {
            get
            {
                return (string) GetValue(VersionNameProperty);
            }
            set
            {
                SetValue(VersionNameProperty, value);
            }
        }

        private void OnResult2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnResult1(object sender, RoutedEventArgs e)
        {
            var item = VersionManage.Current.Versions.FirstOrDefault(w => w.VersionName == VersionName);
            if (item != null)
            {
                MainUtils.ShowMessageBoxInfo(this,"版本名称已经存在.");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
