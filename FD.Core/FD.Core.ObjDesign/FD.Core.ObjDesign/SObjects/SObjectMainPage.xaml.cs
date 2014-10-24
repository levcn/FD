using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using ProjectCreater.Commands;
using ProjectCreater.DB;
using ProjectCreater.Properties;
using ProjectCreater.Settings;
using ProjectCreater.Test;
using ProjectCreater.Threading;
using STComponse;
using STComponse.CFG;
using STComponse.DB;
using STComponse.GCode;
using STComponse.StringData;
using WPFControls;
using WPFControls.Ex;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// SObjectMainPage.xaml
    /// </summary>
    public partial class SObjectMainPage
    {
        public SObjectMainPage()
        {
            if (SettingConfig.Current.UserHabit == null) throw new Exception("SettingConfig.Current.UserHabit为空啦");
            if (Application.Current.Resources.Contains("Habit"))
            {
                Application.Current.Resources["Habit"] = SettingConfig.Current.UserHabit;
            }
            else
            {
                Application.Current.Resources.Add("Habit", SettingConfig.Current.UserHabit);
                
            }
            ShowInTaskbar = true;
            InitializeComponent();
            Loaded += SObjectMainPage_Loaded;
//            Test.TestCompareDB();
            InitCommands();

            //启动自动保存任务
            PCThreadPool.AddAction("AutoSaveTemplate",AutoSaveConfig);
        }

        /// <summary>
        /// 自动保存
        /// </summary>
        private void AutoSaveConfig()
        {
            if (LastVersionConfig != null)
            {
                var fileConfig = VersionManage.Current.GetConfigFileContent(LastVersionConfig);
                if (fileConfig != LastVersionConfig.ToJson())//设置修改过
                {
                    VersionManage.Current.SaveFwConfig(LastVersionConfig, true);
                }
            }
            SettingConfig.Current.Save();
        }
        private void InitCommands()
        {
            ExitCommand = new DelegateCommand(w => true, w => Application.Current.Shutdown());
            SaveCommand = new DelegateCommand(w => true, w => SaveMethod());
            GenerateSqlCommand = new DelegateCommand(w => CB_VersionList.SelectedItem != null, w => GenerateDBSql());
            GenerateCodeCommand = new DelegateCommand(w => CB_VersionList.SelectedItem != null, w => GenerateCode());
            CreateNewVersionCommand = new DelegateCommand(w => true, w => CreateNewVersion());
            RefreshVersionListCommand = new DelegateCommand(w => true, w => RefreshVersionList());
            GenerateUpdateSqlCommand = new DelegateCommand(w => CB_VersionList.SelectedItem != null, w => GenerateUpdateSql());
            OpenSettingCommand = new DelegateCommand(w => true, w => OpenSetting());
            DBConfigCommand = new DelegateCommand(w => true, w => OpenDBConfig());
            EditSPCommand = new DelegateCommand(w => true, w => OpenSP());
            
        }

        private void OpenSP()
        {
            SPList sp = new SPList();
            sp.Editable = true;
            sp.ShowDialog(this);
        }

        private void OpenDBConfig()
        {
            ShowDBSetting(this);
        }

        public static void ShowSetting(UIElement ui, int type = 0)
        {
            SettingConfigWindow c = new SettingConfigWindow();
            c.StartIndex = type;
            c.ShowDialog(ui);
        }
        private void OpenSetting()
        {
//            SettingConfigWindow c = new SettingConfigWindow();
//            c.ShowDialog(this);
            ShowSetting(this);
        }
        public static readonly DependencyProperty EditSPCommandProperty =
                DependencyProperty.Register("EditSPCommand", typeof(DelegateCommand), typeof(SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

public DelegateCommand EditSPCommand
        {
            get
            {
                return (DelegateCommand)GetValue(EditSPCommandProperty);
            }
            set
            {
                SetValue(EditSPCommandProperty, value);
            }
        }
        public static readonly DependencyProperty DBConfigCommandProperty =
                DependencyProperty.Register("DBConfigCommand", typeof (DelegateCommand), typeof (SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand DBConfigCommand
        {
            get
            {
                return (DelegateCommand) GetValue(DBConfigCommandProperty);
            }
            set
            {
                SetValue(DBConfigCommandProperty, value);
            }
        }
        public static readonly DependencyProperty OpenSettingCommandProperty =
                DependencyProperty.Register("OpenSettingCommand", typeof (DelegateCommand), typeof (SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand OpenSettingCommand
        {
            get
            {
                return (DelegateCommand) GetValue(OpenSettingCommandProperty);
            }
            set
            {
                SetValue(OpenSettingCommandProperty, value);
            }
        }
        public static readonly DependencyProperty GenerateUpdateSqlCommandProperty =
                DependencyProperty.Register("GenerateUpdateSqlCommand", typeof(DelegateCommand), typeof(SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand GenerateUpdateSqlCommand
        {
            get
            {
                return (DelegateCommand)GetValue(GenerateUpdateSqlCommandProperty);
            }
            set
            {
                SetValue(GenerateUpdateSqlCommandProperty, value);
            }
        }
        public static readonly DependencyProperty RefreshVersionListCommandProperty =
                DependencyProperty.Register("RefreshVersionListCommand", typeof(DelegateCommand), typeof(SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand RefreshVersionListCommand
        {
            get
            {
                return (DelegateCommand)GetValue(RefreshVersionListCommandProperty);
            }
            set
            {
                SetValue(RefreshVersionListCommandProperty, value);
            }
        }
        public static readonly DependencyProperty CreateNewVersionCommandProperty =
                DependencyProperty.Register("CreateNewVersionCommand", typeof(DelegateCommand), typeof(SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand CreateNewVersionCommand
        {
            get
            {
                return (DelegateCommand)GetValue(CreateNewVersionCommandProperty);
            }
            set
            {
                SetValue(CreateNewVersionCommandProperty, value);
            }
        }
        public static readonly DependencyProperty GenerateCodeCommandProperty =
                DependencyProperty.Register("GenerateCodeCommand", typeof (DelegateCommand), typeof (SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand GenerateCodeCommand
        {
            get
            {
                return (DelegateCommand) GetValue(GenerateCodeCommandProperty);
            }
            set
            {
                SetValue(GenerateCodeCommandProperty, value);
            }
        }
        public static readonly DependencyProperty GenerateSqlCommandProperty =
                DependencyProperty.Register("GenerateSqlCommand", typeof (DelegateCommand), typeof (SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));


        public DelegateCommand GenerateSqlCommand
        {
            get
            {
                return (DelegateCommand) GetValue(GenerateSqlCommandProperty);
            }
            set
            {
                SetValue(GenerateSqlCommandProperty, value);
            }
        }
        public static readonly DependencyProperty SaveCommandProperty =
                DependencyProperty.Register("SaveCommand", typeof (DelegateCommand), typeof (SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand SaveCommand
        {
            get
            {
                return (DelegateCommand) GetValue(SaveCommandProperty);
            }
            protected set
            {
                SetValue(SaveCommandProperty, value);
            }
        }
        public static readonly DependencyProperty ExitCommandProperty =
                DependencyProperty.Register("ExitCommand", typeof (DelegateCommand), typeof (SObjectMainPage), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand ExitCommand
        {
            get
            {
                return (DelegateCommand) GetValue(ExitCommandProperty);
            }
            protected set
            {
                SetValue(ExitCommandProperty, value);
            }
        }
        public static readonly DependencyProperty EditableProperty =
                DependencyProperty.Register("Editable", typeof (bool), typeof (SObjectMainPage), new PropertyMetadata(true));

        public bool Editable
        {
            get
            {
                return (bool) GetValue(EditableProperty);
            }
            set
            {
                SetValue(EditableProperty, value);
            }
        }
        void SObjectMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitVersionList();
//            FwConfig = ReadText().ToObject<FwConfig>();
//            if (FwConfig == null)
//                FwConfig = new FwConfig {
//                    DataObjects = new List<EDataObject> {
//                        new EDataObject {
//                            ObjectName = "人员",
//                            KeyTableName = "SYS_User",
//                            ObjectCode = "SYS_User",
//                            Property = new List<Property> {
//                                new Property {
//                                    Code = "Name",
//                                }
//                            }
//                        }
//                    }
//                };
        }

        private void InitVersionList(bool isRefresh = false)
        {
            var readOnlyCollection = VersionManage.Current.Versions;
            if (readOnlyCollection.Count > 0) LastVersionConfig = readOnlyCollection[0];
            var tempConfigs = VersionManage.Current.GetTempConfigs();
            if (tempConfigs.Count > 0 && LastVersionConfig != null)
            {
                var last = tempConfigs.FirstOrDefault(w => w.VersionNumber == LastVersionConfig.VersionNumber);
                if (last != null)
                {
                    MessageBoxResult re = MessageBoxResult.Yes;
                    if (isRefresh)
                    {
                        re = MessageBox.Show(this, "上次意外退出未保存文件,是否重新加载?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);
                    }
                    if (re == MessageBoxResult.Yes)
                    {
                        VersionManage.Current.ReplaceConfig(last);
                        LastVersionConfig = readOnlyCollection[0];

                    }
                    else
                    {
                        VersionManage.Current.DeleteAllTemp();
                    }
                }
                else
                {
                    VersionManage.Current.DeleteAllTemp();
                }
            }
            CB_VersionList.ItemsSource = readOnlyCollection;
            CB_VersionList.DisplayMemberPath = "VersionName";
            CB_VersionList.SelectedValuePath = "VersionNumber";
            CB_VersionList.SelectedIndex = 0;
            
            //            if (readOnlyCollection.Count > 0)
            //            {
            //                InitCurrentVersionInfo(readOnlyCollection[0]);
            //            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (VersionManage.Current.GetTempConfigs().Count != 0)
            {
                var re = MessageBox.Show(this, "未保存文件,是否保存?", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                if (re == MessageBoxResult.Cancel)//取消退出
                {
                    e.Cancel = true;
                }
                else if (re == MessageBoxResult.Yes)//保存退出
                {
                    VersionManage.Current.SaveFwConfig(LastVersionConfig);
                    VersionManage.Current.DeleteAllTemp();
                }
                else if (re == MessageBoxResult.No)//不保存退出
                {
                    VersionManage.Current.DeleteAllTemp();
                }
            }
            base.OnClosing(e);
        }

        private void InitCurrentVersionInfo(FwConfig fwConfig)
        {
            FwConfig = fwConfig;
        }
        public static readonly DependencyProperty FwConfigProperty =
                DependencyProperty.Register("FwConfig", typeof(FwConfig
                ), typeof(SObjectMainPage), new PropertyMetadata(default(FwConfig)));

        public FwConfig FwConfig
        {
            get
            {
                return (FwConfig)GetValue(FwConfigProperty);
            }
            set
            {
                
                VersionManage.Current.CurrentVersion = value;
                SetValue(FwConfigProperty, value);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SaveMethod();
        }

        private void SaveMethod()
        {
            string json = FwConfig.ToJson();
            VersionManage.Current.SaveFwConfig(FwConfig);
            VersionManage.Current.DeleteAllTemp();
            MessageBox.Show("保存成功。");
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            GenerateDBSql();
        }

        public string DefaultConnectionStr
        {
            get
            {
                if (SettingConfig.Current.DbConfigs == null || SettingConfig.Current.DbConfigs.Count == 0)
                {
                    throw new Exception("未设置数据链接.");
                }
                return SettingConfig.Current.DbConfigs[0].GetConnStr();
            }
        }
        private void GenerateDBSql()
        {
            try
            {
                string commandStr = FwConfig.ToDB();
                System.Windows.Forms.Clipboard.SetText(commandStr);
                commandStr = commandStr.Replace("GO", "");
                DBHelper.ExecuteScalar(DefaultConnectionStr, commandStr);
                                MessageBox.Show("执行完成.");
//                                MessageBox.Show("已经复制到剪切板.");
            }
            catch (PException eee)
            {
                MessageBox.Show(eee.Message);
            }
            catch (Exception ee)
            {
                //throw ee;
                MessageBox.Show(ee.GetBaseException().ToString());
            }
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            GenerateCode();
        }

        private void GenerateCode()
        {
            FwConfigCodeEx.TagPix = SettingConfig.Current.CodeStyle.FilterTagPix?SettingConfig.Current.CodeStyle.TagPix.ToList():new List<string>();
            var t = @"using System;
using System.Collections.Generic;
using BaseEntity;

namespace StaffTrain.Entity
{
    public class $CLASSNAME$ : BaseListEntity
    {
    $CONSTRUCT$
	$RPOPERTIES$
    }
}";
            string path = @"D:\2\3\1";
            path = SettingConfig.Current.CodeStyle.SavePath;
            if (!Directory.Exists(path))
            {
                MainUtils.ShowMessageBoxError(this,"文件保存目录存在！请重新设置目录！");
                ShowSetting(this,1);
                return;
            }
            try
            {
                FwConfig.SaveCodeFile(t, path);
            }
            catch (PException eee)
            {
                MessageBox.Show(eee.Message);
                return;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.GetBaseException().ToString());
                return;
            }
            Process.Start(path);
        }

        public FwConfig LastVersionConfig;
        private void CB_VersionList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var config = CB_VersionList.SelectedItem as FwConfig;
            if (config == null) return;
            InitCurrentVersionInfo(config);
            Editable = CB_VersionList.SelectedIndex == 0;
            GenerateSqlCommand.InvalideCanExecute(null);
            GenerateCodeCommand.InvalideCanExecute(null);
            GenerateUpdateSqlCommand.InvalideCanExecute(null);
        }

        private void BT_CreateVersion(object sender, RoutedEventArgs e)
        {
            CreateNewVersion();
        }

        private void CreateNewVersion()
        {
            var tempConfigs = VersionManage.Current.GetTempConfigs();
            if (tempConfigs.Count > 0 && LastVersionConfig != null)
            {
                var last = tempConfigs.FirstOrDefault(w => w.VersionNumber == LastVersionConfig.VersionNumber);
                if (last != null)
                {
                    var re = MessageBox.Show(this, "配置已经修改,是否保存?", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Yes);
                    if (re == MessageBoxResult.Yes)
                    {
                        VersionManage.Current.SaveFwConfig(LastVersionConfig);
                    }
                    else if (re == MessageBoxResult.No)
                    {
                        VersionManage.Current.DeleteAllTemp();
                        InitVersionList();
                    }
                    else if (re == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
            }

            var version = VersionManage.Current.GetNewVersion();
            AddNewVersionWindow adw = new AddNewVersionWindow {
                Height = 200,
                Width = 400,
            };
            adw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            adw.VersionName = version.VersionName;
            adw.Remark = version.Remark;
            var showDialog = adw.ShowDialog(this);
            if (showDialog.HasValue && showDialog.Value)
            {
                version.VersionName = adw.VersionName;
                version.Remark = adw.Remark;
                VersionManage.Current.AddNewVersion(version);
                InitVersionList();
            }
        }

        private void GenerateUpdateSql()
        {
            GenerateUpdateSqlWindow w = new GenerateUpdateSqlWindow();
            w.ShowDialog(this);
        }

//        private void RefreshVersionList(object sender, RoutedEventArgs e)
//        {
//            RefreshVersionList();
//        }

        private void RefreshVersionList()
        {
            VersionManage.Current.RefreshVersionList();
            InitVersionList();
        }

        public static void ShowDBSetting(UIElement ui)
        {
            if (SettingConfig.Current.DbConfigs == null || SettingConfig.Current.DbConfigs.Count == 0)
            {
                SettingConfig.Current.DbConfigs = new List<DBConfig> { new DBConfig() };
            }
            var config = SettingConfig.Current.DbConfigs[0];
            EditDBConfig c = new EditDBConfig();
            c.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            c.ServerIP = config.ServerName;
            c.ServerUID = config.UID;
            c.ServerPWD = config.PWD;
            c.DBName = config.DataBaseName;
            if (c.ShowDialog(ui) == true)
            {
                config.DataBaseName = c.DBName;
                config.PWD = c.ServerPWD;
                config.UID = c.ServerUID;
                config.ServerName = c.ServerIP;
                //                SettingConfig.Current.DbConfigs[0] = config;
                SettingConfig.Current.Save();
            }    
        }
        private async void Aaaaa_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var table = await DBHelper.GetTablesAsyn(SettingConfig.Current.DbConfigs[0]);
                EDB source = new EDB { ETables = table };
                var target = FwConfig.ToDB();
                var changed = EDBCompare.Compare(source, target);
                
                if (changed.ETables.Count == 0)
                {
                    MainUtils.ShowMessageBoxInfo(this,"数据库已经和配置相同.");
                }
                else
                {

                    if (MessageBox.Show(this, changed.GetChangeText(), "修改说明", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        string commandStr = changed;
                        System.Windows.Forms.Clipboard.SetText(commandStr);
                        commandStr = commandStr.Replace("GO", "");
                        DBHelper.ExecuteScalar(DefaultConnectionStr, commandStr);
                        MainUtils.ShowMessageBoxInfo(this, "执行完成.");
                    }
                }
                //                                MessageBox.Show("已经复制到剪切板.");
            }
            catch (PException eee)
            {
                MainUtils.ShowMessageBoxInfo(this,eee.Message);
            }
            catch (Exception ee)
            {
                //throw ee;
                MainUtils.ShowMessageBoxInfo(this, ee.GetBaseException().ToString());
            }
            //            aaaaa.Content = "等待中...";
            //            var rere =  await DBHelper.GetTablesAsyn(SettingConfig.Current.DbConfigs[0]);
            //            aaaaa.Content = "收到";
            //            rere = await DBHelper.GetTablesAsyn(SettingConfig.Current.DbConfigs[0]);
            //            aaaaa.Content = "完成";

        }

        async Task<string> getstr()
        {
            
             await Task.Delay(5000);
            return "123";
        }
    }
}
