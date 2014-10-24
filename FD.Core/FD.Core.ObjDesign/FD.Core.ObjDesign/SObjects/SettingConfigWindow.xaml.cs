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
using ProjectCreater.DB;
using ProjectCreater.Settings;
using WPFControls;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SettingConfigWindow
    {
        public SettingConfigWindow()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.ToolWindow;
            this.ResizeMode = ResizeMode.NoResize;
            SettingConfig = SettingConfig.Current;
        }

        public static readonly DependencyProperty SettingConfigProperty =
                DependencyProperty.Register("SettingConfig", typeof (SettingConfig), typeof (SettingConfigWindow), new PropertyMetadata(default(SettingConfig)));

        public SettingConfig SettingConfig
        {
            get
            {
                return (SettingConfig) GetValue(SettingConfigProperty);
            }
            set
            {
                SetValue(SettingConfigProperty, value);
            }
        }

        public static readonly DependencyProperty StartIndexProperty =
                DependencyProperty.Register("StartIndex", typeof (int), typeof (SettingConfigWindow), new PropertyMetadata(default(int)));

        /// <summary>
        /// 选项卡的显示位置
        /// </summary>
        public int StartIndex
        {
            get
            {
                return (int) GetValue(StartIndexProperty);
            }
            set
            {
                SetValue(StartIndexProperty, value);
            }
        }
        private void OnResult2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnResult1(object sender, RoutedEventArgs e)
        {
            SettingConfig.Current.Save();
            DialogResult = true;
        }


    }
}
