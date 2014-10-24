using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FD.Core.SLControl.Data;
using FD.Core.SLControl.NavigatorTools;
using SLControls.Editors;
using SLControls.Threads;
using StaffTrain.FwClass.NavigatorTools;
using StaffTrain.FwClass.Reflectors;
using Telerik.Windows.Controls;


namespace FD.Core.SLControl.Controls
{
    public class SubFrame : MainFrame
    {
        public SubFrame()
        {
            this.DefaultStyleKey = typeof(SubFrame);
        }
    }
    public class MainFrame : BaseMultiControl, IControlContainer, IMenuPage
    {
        public MainFrame()
        {
            this.DefaultStyleKey = typeof(MainFrame);
            Unloaded += MainFrame_Unloaded;
        }

        void MainFrame_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ItemsSource!=null)
            foreach (IMenuItem item in ItemsSource)
            {
                item.PropertyChanged -= item_PropertyChanged;
                item.IsSelected = false;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Navigator = new NavigatorWatcher(this) { AddToItem = false };
            Content = new Button {Content = "123123123"};
            Content = new Button {Content = "3121321312"};
        }

        public override void LoadConfig(string configStr)
        {
            
        }

        public static readonly DependencyProperty ItemsSourceProperty =
                DependencyProperty.Register("ItemsSource", typeof (IEnumerable<IMenuItem>), typeof (MainFrame), new PropertyMetadata(default(IEnumerable<IMenuItem>), (s, e) =>
                {
                    ((MainFrame)s).OnItemsSourceChanged(e.NewValue as IEnumerable<IMenuItem>, e.OldValue as IEnumerable<IMenuItem>);
                }));

        private void OnItemsSourceChanged(IEnumerable<IMenuItem> list, IEnumerable<IMenuItem>  oldList)
        {
            if (oldList != null)
            {
                if (!autoBound)
                {
                    foreach (IMenuItem item in oldList)
                    {
                        item.PropertyChanged -= item_PropertyChanged;
                    }
                }
            }
            if (list != null)
            {
                if (!autoBound)
                {
                    foreach (IMenuItem item in list)
                    {
                        item.PropertyChanged += item_PropertyChanged;
                    }
                }
                if (list.Any() && !selfChange)
                {
                    var ii =  list.FirstOrDefault();
                    ThreadHelper.DelayRun(() =>
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            ii.IsSelected = true;
//                            GotoNewPage(ii, GetMenusByParentID(ii.ID), null);
                        });
                    },500,"123");
                }
//                na
            }
        }

        public IEnumerable<IMenuItem> ItemsSource
        {
            get
            {
                return (IEnumerable<IMenuItem>) GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
//
//        public static readonly DependencyProperty ContentProperty =
//                DependencyProperty.Register("Content", typeof (object), typeof (MainFrame), new PropertyMetadata(default(object), (s, e) =>
//                {
//                    ((MainFrame) s).OnContentChanged(e.NewValue as object);
//                }));
//
//        private void OnContentChanged(object list)
//        {}
//
//        public object Content
//        {
//            get
//            {
//                return (object) GetValue(ContentProperty);
//            }
//            set
//            {
//                SetValue(ContentProperty, value);
//            }
//        }
        public static readonly DependencyProperty ChildFrameProperty =
                DependencyProperty.Register("ChildFrame", typeof (MainFrame), typeof (MainFrame), new PropertyMetadata(default(MainFrame), (s, e) =>
                {
                    ((MainFrame) s).OnChildChanged(e.NewValue as MainFrame);
                    
                }));

        private void OnChildChanged(MainFrame list)
        {}
        /// <summary>
        /// 子容器
        /// </summary>
        public MainFrame ChildFrame
        {
            get
            {
                return (MainFrame) GetValue(ChildFrameProperty);
            }
            set
            {
                SetValue(ChildFrameProperty, value);
            }
        }

        public static readonly DependencyProperty ContentPanelProperty =
                DependencyProperty.Register("ContentPanel", typeof (Panel), typeof (MainFrame), new PropertyMetadata(default(Panel), (s, e) =>
                {
                    ((MainFrame) s).OnContentPanelChanged(e.NewValue as Panel);
                }));

        private void OnContentPanelChanged(Panel list)
        {}
        /// <summary>
        /// 包含子页面或ChildFrame子容器
        /// </summary>
        public Panel ContentPanel
        {
            get
            {
                return (Panel) GetValue(ContentPanelProperty);
            }
            set
            {
                SetValue(ContentPanelProperty, value);
            }
        }

        public static readonly DependencyProperty ChildPageProperty =
                DependencyProperty.Register("ChildPage", typeof (UIElement), typeof (MainFrame), new PropertyMetadata(default(UIElement), (s, e) =>
                {
                    ((MainFrame) s).OnChildPageChanged(e.NewValue as UIElement);
                }));

        private void OnChildPageChanged(UIElement list)
        {}
        /// <summary>
        /// 子页面,页面内容部分
        /// </summary>
        public UIElement ChildPage
        {
            get
            {
                return (UIElement) GetValue(ChildPageProperty);
            }
            set
            {
                SetValue(ChildPageProperty, value);
            }
        }
        public static readonly DependencyProperty MenuItemsProperty =
                DependencyProperty.Register("MenuItems", typeof(IEnumerable<IMenuItem>), typeof(MainFrame), new PropertyMetadata(null, (s, e) =>
                {
                    ((MainFrame)s).OnMenuItemsChanged(e.NewValue as IEnumerable<IMenuItem>);
                }));

        private void OnMenuItemsChanged(IEnumerable<IMenuItem> list)
        {
           
        }

        private bool selfChange = false;
        private bool autoBound = false;

        void item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (selfChange) return;
            selfChange = true;
            var list = ItemsSource;
            foreach (IMenuItem item in list)
            {
                if (item != sender && item.IsSelected)
                {
                    item.IsSelected = false;
                }
            }
            
            ItemsSource = null;
//            autoBound = true;
            ItemsSource = list;
//            autoBound = false;
            IMenuItem ii = sender as IMenuItem;
            if (ii != null && ii.IsSelected)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    GotoNewPage(ii, GetMenusByParentID(ii.ID), null);
                });
            }
            selfChange = false;
        }

        public IEnumerable<IMenuItem> MenuItems
        {
            get
            {
                return (IEnumerable<IMenuItem>)GetValue(MenuItemsProperty);
            }
            set
            {
                SetValue(MenuItemsProperty, value);
            }
        }
        public NavigatorWatcher Navigator { get; set; }
        /// <summary>
        /// 选择一级菜单后,转到指定的页面
        /// </summary>
        /// <param name="e"></param>
        /// <param name="last"></param>
        /// <param name="o"></param>
        private void GotoNewPage(IMenuItem e, List<IMenuItem> last = null, object o = null)
        {
            List<IMenuItem> l = null;
            if (last != null) l = last.ToList();
            var childs = GetMenusByParentID(e.ID);
            if (childs.Count == 0) //如果没有子菜单,
            {
                var menuNamespace = GetMenuNamespace(e);
                if (Navigator!=null) Navigator.GoToNewPage(null, menuNamespace, null, true, false, this, null, o);
            }
            else
            {
                SubFrame subFrame = new SubFrame {ItemsSource = childs,MenuItems = childs};
                if (!string.IsNullOrEmpty(e.StyleName))
                {
                    if (Application.Current.Resources.Contains(e.StyleName))
                    {
                        Style s = Application.Current.Resources[e.StyleName] as Style;

                        if (s != null && s.TargetType == typeof(SubFrame))
                        {
                            subFrame.Style = s;
                        }
                    }
                }
                Content = subFrame;
                //                Navigator.GoToNewPage(null, new SubFrame(), e.ID, true, false, this, l, o);
            }
            OnCurrentMenuChanged(e, o); //设置当前位置
        }

        public event TEventHandler<IMenuItem, object> CurrentMenuChanged;

        protected virtual void OnCurrentMenuChanged(IMenuItem sender, object args)
        {
            var handler = CurrentMenuChanged;
            if (handler != null) handler(sender, args);
        }

        /// <summary>
        /// 返回菜单所指向的页面地址
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private string GetMenuNamespace(IMenuItem menu)
        {
            var name = menu.PageUrl;
            if (name.StartsWith("$"))
            {
                name = "FD.Core.Test.Views." + name.Substring(1).Replace("/", ".");
            }
            else
            {
                return name.Replace("/", ".");
            }
            if (name.Length > 5) name = name.Substring(0, name.Length - 5);

//            if (SFAppSetting.ProductVersion != null)
//            {
//                var tmp = name.Replace("Views", "Versions." + SFAppSetting.ProductVersion.Code);
//                var switchable = ReflectionHelper.GetObjectByName(tmp);
//                if (switchable != null)
//                {
//                    return tmp;
//                }
//            }

            return name;
        }

        /// <summary>
        /// 返回指定ID的子菜单列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<IMenuItem> GetMenusByParentID(Guid id)
        {
            if(MenuItems==null)return new List<IMenuItem>();
            var parent = MenuItems.FirstOrDefault(w => w.ID == id);
            if (parent != null)
            {
                if (parent.Children != null)
                {
                    return parent.Children.Cast<IMenuItem>().ToList();
                }
                return new List<IMenuItem>();
                return MenuItems.Where(w => w.Code.StartsWith(parent.Code)).ToList();
            }
            return new List<IMenuItem>();
        }

        public void GotoMenuItem(List<IMenuItem> item, object o)
        {
            var list = item.ToList();
            List<IMenuItem> last = null;
            if (list.Count > 1) last = list.Skip(1).Take(item.Count - 1).ToList();
            GotoNewPage(list[0], last, o);
        }
    }

    public class FDGrid : Grid
    {
        public FDGrid()
        {
            AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(mouseLeftButtonUp), true);
        }

        public static readonly DependencyProperty OnlySelectProperty =
                DependencyProperty.Register("OnlySelect", typeof (bool), typeof (FDGrid), new PropertyMetadata(true, (s, e) =>
                {
                    ((FDGrid) s).OnOnlySelectChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnOnlySelectChanged(bool list)
        {}
        /// <summary>
        /// 是否只能选中,无法手工取消选中状态.
        /// </summary>
        public bool OnlySelect
        {
            get
            {
                return (bool) GetValue(OnlySelectProperty);
            }
            set
            {
                SetValue(OnlySelectProperty, value);
            }
        }
        private void mouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IIsSelected isSelected = DataContext as IIsSelected;
            if (isSelected != null)
            {
                if (!OnlySelect || OnlySelect && !isSelected.IsSelected)
                isSelected.IsSelected = !isSelected.IsSelected;
            }
        }
    }
}
