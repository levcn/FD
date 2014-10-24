using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SLControls.ActiveMethod;
using SLControls.DataClientTools;
using SLControls.Editors;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools;
using StaffTrain.FwClass.Reflectors;
using StaffTrain.FwClass.Serializer;
using STComponse.CFG;


namespace SLControls.Controls
{
  [TemplateVisualState(GroupName = "CommonStates", Name = "MouseNormal")]
  [TemplateVisualState(GroupName = "CommonStates", Name = "MouseOver")]
    public class LoginPanel : BaseMultiControl
    {
//      ScaleTransform scaleTransform = new ScaleTransform();
        public LoginPanel()
        {
            this.DefaultStyleKey = typeof(LoginPanel);
//                scaleTransform.ScaleX = 0.8;
//                scaleTransform.ScaleY = 0.8;
//                RenderTransform = scaleTransform;
            this.MouseEnter += LoginPanel_MouseEnter;
            this.MouseLeave += LoginPanel_MouseLeave;
        }

        void LoginPanel_MouseLeave(object sender, MouseEventArgs e)
        {
//            scaleTransform.ScaleX = 0.8;
//            scaleTransform.ScaleY = 0.8;
//            VisualStateManager.GoToState(this, "MouseNormal", true);
        }

        void LoginPanel_MouseEnter(object sender, MouseEventArgs e)
        {
//            scaleTransform.ScaleX = 1;
//            scaleTransform.ScaleY = 1;
//            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        private Button CTL_LoginButton;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CTL_LoginButton = GetTemplateChild("CTL_LoginButton") as Button;
            CTL_UserName = GetTemplateChild("CTL_UserName") as TextBox;
            CTL_Password = GetTemplateChild("CTL_Password") as TextBox;
            var Border = GetTemplateChild("Border") as Border;
            if (CTL_LoginButton != null) CTL_LoginButton.Click += CTL_LoginButton_Click;
            Border.BorderThickness = new Thickness(10);
            var sdfere = this.BorderThickness;
            Genercode();
        }

        public override List<string> EditablePropters
        {
            get
            {
                return new List<string>{"ShowValidCode"};
            }
        }

        public static readonly DependencyProperty SysValidCodeProperty =
                DependencyProperty.Register("SysValidCode", typeof (string), typeof (LoginPanel), new PropertyMetadata(default(string)));

        public string SysValidCode
        {
            get
            {
                return (string) GetValue(SysValidCodeProperty);
            }
            set
            {
                SetValue(SysValidCodeProperty, value);
            }
        }

        public static readonly DependencyProperty ValidImageProperty =
                DependencyProperty.Register("ValidImage", typeof (Image), typeof (LoginPanel), new PropertyMetadata(default(Image)));

        public Image ValidImage
        {
            get
            {
                return (Image) GetValue(ValidImageProperty);
            }
            set
            {
                SetValue(ValidImageProperty, value);
            }
        }
        void Genercode()
        {
            SysValidCode = CreateIndentifyCode(6);
//            CreatImage(SysValidCode, ValidImage, 150, 30);
        }
        private void CTL_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                MessageHelper.Info("请输入用户名。");
                if (CTL_UserName != null) CTL_UserName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                MessageHelper.Info("请输入密码。");
                if (CTL_Password != null) CTL_Password.Focus();
                return;
            }
            OnLogin();
        }

        public static readonly DependencyProperty ValidCodeVisibilityProperty =
                DependencyProperty.Register("ValidCodeVisibility", typeof(Visibility), typeof(LoginPanel), new PropertyMetadata(Visibility.Collapsed));

        public Visibility ValidCodeVisibility
        {
            get
            {
                return (Visibility) GetValue(ValidCodeVisibilityProperty);
            }
            set
            {
                SetValue(ValidCodeVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty ShowValidCodeProperty =
                DependencyProperty.Register("ShowValidCode", typeof (bool), typeof (LoginPanel), new PropertyMetadata(false,
                        (s, e) =>
                        {
                            ((LoginPanel) s).ValidCodeVisibility = ((bool) e.NewValue) ? Visibility.Visible : Visibility.Collapsed;
                        }));

        [Editable(GroupName = "基本属性", DisplayName = "是否启用验证码", Description = "设置是否启用验证码登陆的功能。")]
        public bool ShowValidCode
        {
            get
            {
                return (bool) GetValue(ShowValidCodeProperty);
            }
            set
            {
                SetValue(ShowValidCodeProperty, value);
            }
        }

        public static readonly DependencyProperty LoginSuccessObjectProperty =
                DependencyProperty.Register("LoginSuccessObject", typeof (Object), typeof (LoginPanel), new PropertyMetadata(default(Object)));

        [Editable(GroupName = "基本属性", DisplayName = "登陆成功结果", Description = "登陆成功后服务器返回的结果。")]
        public Object LoginSuccessObject
        {
            get
            {
                return (Object) GetValue(LoginSuccessObjectProperty);
            }
            set
            {
                SetValue(LoginSuccessObjectProperty, value);
            }
        }

        public static readonly DependencyProperty LoginSuccessPageProperty =
                DependencyProperty.Register("LoginSuccessPage", typeof (string), typeof (LoginPanel), new PropertyMetadata(default(string)));


        [Editable(GroupName = "基本属性", DisplayName = "登陆成功页面", Description = "登陆成功后要跳转的主页面。")]
        public string LoginSuccessPage
        {
            get
            {
                return (string) GetValue(LoginSuccessPageProperty);
            }
            set
            {
                SetValue(LoginSuccessPageProperty, value);
            }
        }
        public static readonly DependencyProperty ELoginProperty =
                DependencyProperty.Register("ELogin", typeof (string), typeof (LoginPanel), new PropertyMetadata(null));

        [Editable(GroupName = "事件", DisplayName = "登陆事件", Description = "登陆事件的设置。")]
        public string ELogin
        {
            get
            {
                return (string) GetValue(ELoginProperty);
            }
            set
            {
                SetValue(ELoginProperty, value);
            }
        }
        public static readonly DependencyProperty ValidCodeProperty =
                DependencyProperty.Register("ValidCode", typeof (String), typeof (LoginPanel), new PropertyMetadata(default(string)));

        public string ValidCode
        {
            get
            {
                return (string) GetValue(ValidCodeProperty);
            }
            set
            {
                SetValue(ValidCodeProperty, value);
            }
        }
        public static readonly DependencyProperty UserNameProperty =
                DependencyProperty.Register("UserName", typeof (string), typeof (LoginPanel), new PropertyMetadata(default(string), 
                    (s, e) =>
                {
                    
                }));

        public string UserName
        {
            get
            {
                return (string) GetValue(UserNameProperty);
            }
            set
            {
                SetValue(UserNameProperty, value);
            }
        }

        public static readonly DependencyProperty PasswordProperty =
                DependencyProperty.Register("Password", typeof (string), typeof (LoginPanel), new PropertyMetadata(default(string)));

        public string Password
        {
            get
            {
                return (string) GetValue(PasswordProperty);
            }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        /// <summary>
        /// 登陆事件
        /// </summary>
        public event TEventHandler<object, EventArgs> Login;

        protected virtual async void OnLogin()
        {
//            if (!string.IsNullOrEmpty(ELogin))
//            var rewe =  await ActiveHelper.Execute(this, ELogin);
//            Dispatcher.BeginInvoke(() =>
//            {
//                MessageBox.Show(rewe.ToString());
//            });
            await Login1();
            var handler = Login;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        public string CreateIndentifyCode(int count)
        {
            string allchar = "1,2,3,4,5,6,7,8,9,0,A,a,B,b,C,c,D,d,E,e,F,f," +
                "G,g,H,h,I,i,J,j,K,k,L,l,M,m,N,n,O,o,P,p,Q,q,R,r,S,s," +
                "T,t,U,u,V,v,W,w,X,x,Y,y,Z,z";
            string[] allchararray = allchar.Split(',');
            string randomcode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(61);
                if (temp == t)
                {
                    return CreateIndentifyCode(count);
                }
                temp = t;
                randomcode += allchararray[t];
            }
            return randomcode;
        }
        Random r = new Random(DateTime.Now.Millisecond);
        public void CreatImage(string Text, Image imgsource, int iw, int ih)
        {
            Grid Gx = new Grid();
            Canvas cv1 = new Canvas();
            for (int i = 0; i < 6; i++)
            {
                Polyline p = new Polyline();
                for (int ix = 0; ix < r.Next(3, 6); ix++)
                {
                    p.Points.Add(new Point(r.NextDouble() * iw,
                        r.NextDouble() * ih));
                }
                byte[] Buffer = new byte[3];
                r.NextBytes(Buffer);
                SolidColorBrush SC = new SolidColorBrush(Color.FromArgb(255,
                    Buffer[0], Buffer[1], Buffer[2]));
                p.Stroke = SC;
                p.StrokeThickness = 0.5;
                cv1.Children.Add(p);
            }
            Canvas cv2 = new Canvas();
            int y = 0;
            int lw = 6;
            double w = (iw - lw) / Text.Length;
            int h = (int)ih;
            foreach (char x in Text)
            {
                byte[] Buffer = new byte[3];
                r.NextBytes(Buffer);
                SolidColorBrush SC = new SolidColorBrush(Color.FromArgb(255,
                    Buffer[0], Buffer[1], Buffer[2]));
                TextBlock t = new TextBlock();
                t.TextAlignment = TextAlignment.Center;
                t.FontSize = r.Next(h - 3, h);
                t.Foreground = SC;
                t.Text = x.ToString();
                t.Projection = new PlaneProjection()
                {
                    RotationX = r.Next(-30, 30),
                    RotationY = r.Next(-30, 30),
                    RotationZ = r.Next(-10, 10)
                };
                cv2.Children.Add(t);
                Canvas.SetLeft(t, lw / 2 + y * w);
                Canvas.SetTop(t, 0);
                y++;
            }
            Gx.Children.Add(cv1);
            Gx.Children.Add(cv2);
            WriteableBitmap W = new WriteableBitmap(Gx, new TransformGroup());
            W.Render(Gx, new TransformGroup());
            imgsource.Source = W;
        }

        public override void LoadConfig(string configStr)
        {
            var config  = JsonHelper.JsonDeserialize<LoginPanelConfig>(configStr);
            if (config != null)
            {
                this.ShowValidCode = config.ShowValidCode;
                ELogin = config.ELogin;
            }
        }

        public async Task Login1()
        {
            if(DataContext==null)throw new Exception("未指定登陆控件的数据对象.");
            var method = typeof (ActionHelper).GetMethod("ExecSTCheck").MakeGenericMethod(DataContext.GetType());
            var task = method.Invoke(null, new object[] { ControlConfig.DataConfig.SPName, DataContext,GetParameter(), GetOurputValue(),true }) as Task;
//            await task;
            var obj1 = await ReflectionHelper.AwaitExecute<ResultDataItem>(task);

            DataContext = obj1.Entity;
            if (obj1.OutputValues != null)
            {
                foreach (var o in obj1.OutputValues)
                {
                    SetValue(o);
                }
            }
            if (!string.IsNullOrEmpty(RealPasswords))
            {
                if (RealPasswords == Password)
                {
//                    MessageBox.Show("登陆成功,跳转页面.");
                    OnLoginSuccess();
                    //                    TabNavigation
                    //                    LoginSuccessPage
                }
                else
                {
                    MessageBox.Show("密码不正确.");
                }
            }
            else
            {
                MessageBox.Show("账号不存在.");
            }
        }

      public event TEventHandler<object, EventArgs> LoginSuccess;

      protected virtual void OnLoginSuccess()
      {
          var handler = LoginSuccess;
          if (handler != null) handler(this, EventArgs.Empty);
      }

      public static readonly DependencyProperty RealPasswordsProperty =
                DependencyProperty.Register("RealPasswords", typeof (string), typeof (LoginPanel), new PropertyMetadata(default(string)));

        private TextBox CTL_UserName;
        private TextBox CTL_Password;

        public string RealPasswords
        {
            get
            {
                return (string) GetValue(RealPasswordsProperty);
            }
            set
            {
                SetValue(RealPasswordsProperty, value);
            }
        }
        private List<OutputValue> GetOurputValue()
        {
            var re = ControlConfig.DataConfig.Parametes.Select(w=>new OutputValue{From = w.Value,Name = w.Name}).ToList();
            return re;
//            return new List<OutputValue> {
//                new OutputValue{From = "#Page1",Name = "UserID"},
//            };
        }
    }

    public class LoginPanelConfig
    {
        public bool ShowValidCode { get; set; }

        public string ELogin { get; set; }
    }
}
