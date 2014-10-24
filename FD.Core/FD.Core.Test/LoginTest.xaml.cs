using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FD.Core.Test;
using FD.Core.Test.Entity;
using SLControls.Controls;
using SLControls.Editors;
using SLControls.ThemeManages;
using StaffTrain.FwClass.Reflectors;
using Telerik.Windows.Controls;


namespace SLTest
{
    public partial class LoginTest
    {
        public static LoginTest Current;

        public LoginTest()
        {
            Current = this;
            InitializeComponent();
            Loaded += LoginTest_Loaded;
        }

        public void SwitchPage(string name)
        {
             var type = GetType().Assembly.GetTypes().FirstOrDefault(w => w.Name == name);
            var usercontrol = ReflectionHelper.GetObject(type) as UserControl;
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(usercontrol);
        }
        async void LoginTest_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = await ThemeFileManage.GetThemeControl<Grid>("Default", "LoginTest");
            InitMultiControl(grid);
            await ApplyReadServerPageConfig(grid);
            var loginPanel = grid.FindChildByType<LoginPanel>();
            loginPanel.SetBinding(DataContextProperty, new Binding { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath("Object1") });
            loginPanel.SetBinding(LoginPanel.UserNameProperty, new Binding { Source = Object1, Mode = BindingMode.TwoWay, Path = new PropertyPath("Account") });
            loginPanel.SetBinding(LoginPanel.PasswordProperty, new Binding { Source = Object1, Mode = BindingMode.TwoWay, Path = new PropertyPath("Passwords") });
            loginPanel.SetBinding(LoginPanel.RealPasswordsProperty, new Binding { Source = Object1, Mode = BindingMode.TwoWay, Path = new PropertyPath("Passwords") });
            
//            loginPanel.ControlConfig = new ControlConfig { 
//                DataConfig = new ControlDataConfig {
//                    DataType = 2,
//                    SPName = "SYS_UserLogin",
//                    Parametes = new List<STParamete> {
//                        new STParamete("InputAccount",""),
//                        new STParamete("InputPasswords",""),
//                    }
//                }
//            };
            Object1 = new SYS_User{Account = "THT",Passwords = "123"};
            LayoutRoot.Children.Add(grid);
//            var loginPanel = grid.FindName("loginPanel") as LoginPanel;
            loginPanel.LoginSuccess += LoginSuccess;
        }

        private void LoginSuccess(object sender, EventArgs args)
        {
            App.SetCurrentPage(new MainPage());
        }

        private void LoginPanel_OnLogin(object sender, EventArgs args)
        {
//            MessageBox.Show("登陆中。。");
        }

        public override string PageCode
        {
            get
            {
                return "LoginTest";
            }
        }

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            var childs = this.ChildrenOfType<BaseMultiControl>().ToList();
            childs.ForEach(w =>
            {
                w.EditState = EditState.Run;
            });
        }
        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var childs = this.ChildrenOfType<BaseMultiControl>().ToList();
            childs.ForEach(w =>
            {
                w.EditState = EditState.Normal;
            });
        }

    }
}
