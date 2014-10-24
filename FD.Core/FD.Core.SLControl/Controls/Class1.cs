using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Threads;
using StaffTrain.FwClass.NavigatorTools;


namespace FD.Core.SLControl.Controls
{
    /// <summary>
    /// 所有用户控件的基类
    /// </summary>
    public class LevcnUserControl : UserControl, ISwitchable, IDisposable
    {
        //        public static List<LevcnUserControl> AllControls = new List<LevcnUserControl>();
        public static readonly DependencyProperty BrowserVisibilityProperty =
                DependencyProperty.Register("BrowserVisibility", typeof(Visibility), typeof(LevcnUserControl), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 浏览页面需要隐藏的设置
        /// </summary>
        public Visibility BrowserVisibility
        {
            get
            {
                return (Visibility)GetValue(BrowserVisibilityProperty);
            }
            set
            {
                SetValue(BrowserVisibilityProperty, value);
            }
        }
        #region 属性



        #endregion
        public LevcnUserControl()
        {
            //            AllControls.Add(this);
            Loaded += (s, e) =>
            {
                //                              if (!DesignerProperties.GetIsInDesignMode(this))
                //                              {
                //                                  UpdateLayout();
                //                              }
                //SetAllChildFontFamily(this);
                //                              MouseLeftButtonDown += click1;

                InitPage();
            };
//            if (AppSetting.IsDebug)
//            {
//                AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(click1), true);
//            }
            Unloaded += LevcnUserControl_Unloaded;
//            this.MouseRightButtonDown += (sender, e) =>
//            {
//
//                e.Handled = true;
//
//            };

        }

        private void click1(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) &&
                Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) &&
                Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                MessageBox.Show(GetType().FullName);
        }
        ~LevcnUserControl()
        {
            Debug.WriteLine(this.GetType().FullName);
            //            AllControls.Remove(this);
        }

        //        void click1(object sender, RoutedEventArgs e)
        //        {
        //            MessageBox.Show(GetType().FullName);
        //        }
        void LevcnUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }

        private void SetAllChildFontFamily(DependencyObject button)
        {
            var c = VisualTreeHelper.GetChildrenCount(button);
            List<DependencyObject> ll = new List<DependencyObject>();
            for (int i = 0; i < c; i++)
            {
                var o = VisualTreeHelper.GetChild(button, i);
                ll.Add(o);
                if (o is TextBlock)
                {
                    (o as TextBlock).FontFamily = new FontFamily("NSimsun");
                };
            }
            //ll.ForEach(SetAllChildFontFamily);这句话应该没什么用吧？先注释掉 chenjinju20130820
        }

        /// <summary>
        /// 页面加载以后执行,初始化页面数据
        /// </summary>
        protected virtual void InitPage()
        {
        }
        /// <summary>
        /// 延迟执行指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="s"></param>
        public void DelayRun(Action action, int s = 100, string name = null, bool invoke = true)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            ThreadHelper.DelayRun(() =>
            {
                if (invoke) Dispatcher.BeginInvoke(action);
                else
                {
                    action();
                }
            }, s, name);
        }
        /// <summary>
        /// 延迟执行指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="s"></param>
        public void DelayRun(Func<bool> action, int s = 1)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            ThreadHelper.DelayRun(() =>
            {
                var re = true;
                //                Dispatcher.BeginInvoke(() =>
                //                {
                return action();
                //                });
                //                return re;
            }, s);
        }
        /// <summary>
        /// 返回状态控件的状态
        /// </summary>
        /// <returns></returns>
        protected virtual object GetStateData()
        {
            return null;
        }
        /// <summary>
        /// 根据对象初始化控件的状态
        /// </summary>
        /// <param name="data"></param>
        protected virtual void SetStateData(object data)
        {

        }
        #region Implementation of ISwitchable

        public object StateData
        {
            get
            {
                return GetStateData();
            }
            set
            {
                SetStateData(value);
            }
        }

        public IControlContainer ParentControl { get; set; }
        public object Args { get; set; }

        /// <summary>
        /// 切换当前页面为指定的页面
        /// </summary>
        /// <param name="page"></param>
        public void ParentSwitchPage(ISwitchable page)
        {
            if (ParentControl != null)
            {
                page.ParentControl = ParentControl;
                ParentControl.Navigator.Switch(page);
            }
            else
            {
                throw new Exception("上级控件为空");
            }
        }
        /// <summary>
        /// 关闭当前页面,打开当前页面之前的页面
        /// </summary>
        public void CloseCurrentPage(NavigatorItem item = null)
        {
            if (ParentControl != null)
            {
                if (item != null)
                {
                    ParentControl.Navigator.CloseCurrentPage(item);
                }
                else
                {
                    ParentControl.Navigator.CloseCurrentPage();
                }
            }
        }
        #endregion

        //        /// <summary>
        //        /// 打开一个子窗口
        //        /// </summary>
        //        /// <typeparam name="T">参数的类型</typeparam>
        //        /// <param name="entity">要给编辑窗口传递的参数</param>
        //        /// <param name="editpanel">子窗口的内容</param>
        //        /// <param name="formSize">窗口大小</param>
        //        /// <param name="saveMethod">在子窗口中点击保存处理的事件</param>
        //        /// <param name="title"> </param>
        //        protected void OpenChildWindow<T>(T entity, LevcnEditControl editpanel, Size formSize, Action<T> saveMethod = null,string title = null)
        //        {
        //            editpanel.Saved += (s, e) => { if (saveMethod != null) saveMethod(entity); };
        //            //editpanel.Method = method;
        //            editpanel.Entity = entity;
        //            var win = new EditChildWindow1 { Width = formSize.Width, Height = formSize.Height, EditControl = editpanel };
        //            win.ParentLayoutRoot = Application.Current.RootVisual as Panel;
        //            if (title!=null) win.Title = title;
        //            win.Show();
        //        }

        public virtual void Dispose()
        {

        }
    }
}
