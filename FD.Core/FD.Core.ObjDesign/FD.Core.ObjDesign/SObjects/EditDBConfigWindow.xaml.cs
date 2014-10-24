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
    public partial class EditDBConfig
    {
        public EditDBConfig()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.ToolWindow;
            this.ResizeMode = ResizeMode.NoResize;
        }

        public static readonly DependencyProperty ServerIPProperty =
                DependencyProperty.Register("ServerIP", typeof (string), typeof (EditDBConfig), new PropertyMetadata(default(string)));

        public string ServerIP
        {
            get
            {
                return (string) GetValue(ServerIPProperty);
            }
            set
            {
                SetValue(ServerIPProperty, value);
            }
        }

        public static readonly DependencyProperty ServerUIDProperty =
                DependencyProperty.Register("ServerUID", typeof (string), typeof (EditDBConfig), new PropertyMetadata(default(string)));

        public string ServerUID
        {
            get
            {
                return (string) GetValue(ServerUIDProperty);
            }
            set
            {
                SetValue(ServerUIDProperty, value);
            }
        }

        public static readonly DependencyProperty ServerPWDProperty =
                DependencyProperty.Register("ServerPWD", typeof (string), typeof (EditDBConfig), new PropertyMetadata(default(string)));

        public string ServerPWD
        {
            get
            {
                return (string) GetValue(ServerPWDProperty);
            }
            set
            {
                SetValue(ServerPWDProperty, value);
            }
        }

        public static readonly DependencyProperty DBNameProperty =
                DependencyProperty.Register("DBName", typeof (string), typeof (EditDBConfig), new PropertyMetadata(default(string)));

        public string DBName
        {
            get
            {
                return (string) GetValue(DBNameProperty);
            }
            set
            {
                SetValue(DBNameProperty, value);
            }
        }
        public static readonly DependencyProperty VersionNumberProperty =
                DependencyProperty.Register("VersionNumber", typeof(int), typeof(EditDBConfig), new PropertyMetadata(default(int)));

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
                DependencyProperty.Register("Remark", typeof(string), typeof(EditDBConfig), new PropertyMetadata(default(string)));

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
                DependencyProperty.Register("VersionName", typeof(string), typeof(EditDBConfig), new PropertyMetadata(default(string)));

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
                MainUtils.ShowMessageBoxInfo(this, "版本名称已经存在．");
            }
            else
            {
                DialogResult = true;
            }
        }

        private async void TestConnect()
        {
            if (string.IsNullOrWhiteSpace(ServerIP))
            {
                TB_ServerIP.Focus();
                MainUtils.ShowMessageBoxInfo(this, "请输入服务器地址！");
                return;
            }
            if (string.IsNullOrWhiteSpace(ServerUID))
            {
                TB_ServerUID.Focus();
                MainUtils.ShowMessageBoxInfo(this, "请输入登陆名！"); 
                return;

            }
            if (string.IsNullOrWhiteSpace(ServerPWD))
            {
                TB_ServerPWD.Focus();
                MainUtils.ShowMessageBoxInfo(this, "请输入密码！");
                return;
            }
            bool haveError = false;
            B_test.Content = "测试中．．．";
            try
            {
                TB_ServerIP.IsEnabled = false;
                TB_ServerUID.IsEnabled = false;
                TB_ServerPWD.IsEnabled = false;
                TB_DBName.IsEnabled = false;
                var tables = await DBHelper.GetDBsAsyn(new DBConfig {
                    ServerName = ServerIP,
                    PWD = ServerPWD,
                    UID = ServerUID,
                });
                TB_DBName.ItemsSource = tables;
            }
            catch (Exception e)
            {
                MainUtils.ShowMessageBoxError(this,e.Message);
                haveError = true;
            }
            TB_ServerIP.IsEnabled = true;
            TB_ServerUID.IsEnabled = true;
            TB_ServerPWD.IsEnabled = true;
            TB_DBName.IsEnabled = true;
            B_test.Content = "测试链接";
            if (!haveError)
            {
                MainUtils.ShowMessageBoxInfo(this, "链接成功．");
            }
        }
        private void TestConnect_Click(object sender, RoutedEventArgs e)
        {
            TestConnect();
        }

        private async void T_CreateDB_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                T_CreateDB.IsEnabled = false;
                T_CreateDB.Content = "执行中．";
                var tables = await DBHelper.GetDBsAsyn(new DBConfig {
                    ServerName = ServerIP,
                    PWD = ServerPWD,
                    UID = ServerUID,
                });
                var dbName = TB_DBName.Text;
                if (tables.Any(w => w.Equals(dbName, StringComparison.OrdinalIgnoreCase)))
                {
                    MainUtils.ShowMessageBoxError(this, "数据库已经存在．");
                    return;
                }
                await DBHelper.CheckDBExistAndCreateAsyn(new DBConfig {
                    ServerName = ServerIP,
                    PWD = ServerPWD,
                    UID = ServerUID,
                }, dbName);
                T_CreateDB.Content = "新建数据库";
            }
            catch (Exception ee)
            {
                MainUtils.ShowMessageBoxError(this,ee.Message);
            }
        }

        private async void TB_DBName_OnKeyUp(object sender, KeyEventArgs e)
        {
            await CheckCreateDB();
        }

        private async Task CheckCreateDB()
        {
            var tables = await DBHelper.GetDBsAsyn(new DBConfig {
                ServerName = ServerIP,
                PWD = ServerPWD,
                UID = ServerUID,
            });
            T_CreateDB.IsEnabled = tables.All(w => !w.Equals(TB_DBName.Text, StringComparison.OrdinalIgnoreCase));
        }

        private async void TB_DBName_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await CheckCreateDB();
        }
    }
}
