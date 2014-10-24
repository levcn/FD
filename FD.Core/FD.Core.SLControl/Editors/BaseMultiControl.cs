using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using SLControls.Cache;
using SLControls.Controls;
using SLControls.Extends;
using SLControls.PageConfigs;
using StaffTrain.FwClass.DataClientTools;
using StaffTrain.FwClass.Reflectors;
using StaffTrain.FwClass.Serializer;
using Telerik.Windows.Controls;


namespace SLControls.Editors
{
    /// <summary>
    ///     复合控件的基类
    /// </summary>
    [TemplateVisualState(GroupName = "EditStates", Name = "Normal")] //正常
    [TemplateVisualState(GroupName = "EditStates", Name = "Selectable")] //可以选择
    [TemplateVisualState(GroupName = "EditStates", Name = "Selected")] //已经选择
    public abstract class BaseMultiControl : BaseControl
    {
        public static readonly DependencyProperty IsSubProperty =
                DependencyProperty.Register("IsSub", typeof (bool), typeof (BaseMultiControl), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((BaseMultiControl) s).OnIsSubChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnIsSubChanged(bool list)
        {}

        public bool IsSub
        {
            get
            {
                return (bool) GetValue(IsSubProperty);
            }
            set
            {
                SetValue(IsSubProperty, value);
            }
        }
        /// <summary>
        /// 进入运行模式
        /// </summary>
        /// <param name="owner"></param>
        public static void ToRunMode(UserControl owner)
        {
            owner.MouseMove -= owner_MouseMove;
            CurrentMultiPageControls.ForEach(w =>
            {
                w.EditState = EditState.Run;
//                w.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(EditClick), true);
//                w.AddHandler(mouseev, new MouseButtonEventHandler(EditClick), true);
                owner.RemoveHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(ToFocus));
                w.MouseEnter -= w_MouseEnter;
                w.MouseLeave -= w_MouseLeave;
            });
        }

        public static readonly DependencyProperty RelEditProperty =
                DependencyProperty.Register("RelEdit", typeof (bool), typeof (BaseMultiControl), new PropertyMetadata(true, (s, e) =>
                {
                    ((BaseMultiControl) s).OnEditableChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnEditableChanged(bool list)
        {}
        [Editable(GroupName = "基本属性", DisplayName = "和父控件一起编辑", Description = "和父控件一起编辑。")]
        public bool RelEdit
        {
            get
            {
                return (bool) GetValue(RelEditProperty);
            }
            set
            {
                SetValue(RelEditProperty, value);
            }
        }
        public static readonly DependencyProperty SubControlNameListProperty =
                DependencyProperty.Register("SubControlNameList", typeof(List<string>), typeof(BaseMultiControl), new PropertyMetadata(null, (s, e) =>
                {
                    ((BaseMultiControl)s).OnSubControlNameListChanged(e.NewValue as List<string>);
                }));

        private void OnSubControlNameListChanged(List<string> list)
        {}

        public List<string> SubControlNameList
        {
            get
            {
                return (List<string>)GetValue(SubControlNameListProperty);
            }
            set
            {
                SetValue(SubControlNameListProperty, value);
            }
        }

        static void owner_MouseMove(object sender, MouseEventArgs e)
        {
            var current = GetCurrentMultiControl(e);
            
            if (current != null)
            {
                if (current.EditState == EditState.Normal)current.EditState = EditState.Hover;
            }
            CurrentMultiPageControls.Where(w=>w != current).ForEach(w =>
            {
                if (w.EditState == EditState.Hover) w.EditState = EditState.Normal;
            });
        }

        private static BaseMultiControl GetCurrentMultiControl(MouseEventArgs e)
        {
            var ins = CurrentMultiPageControls.Where(w =>
            {
                var p = e.GetPosition(w);
                if (p.X >= 0 && p.Y >= 0 && p.X <= w.ActualWidth && p.Y <= w.ActualHeight &&
                    (!w.IsSub || w.IsSub && !w.RelEdit)) return true;
                return false;
            }).ToList();
            var current = ins.OrderBy(w => w.ActualWidth*w.ActualHeight).FirstOrDefault();
            return current;
        }

        static void RestoreState()
        {
            CurrentMultiPageControls.ForEach(w=>w.EditState = EditState.Normal);
        }
        private static List<BaseMultiControl> CurrentMultiPageControls = new List<BaseMultiControl>();
        /// <summary>
        /// 进入编辑模式
        /// </summary>
        /// <param name="owner"></param>
        public static void ToEditMode(UserControl owner)
        {
            CurrentMultiPageControls = ControlExtends.ChildrenOfType<BaseMultiControl>(owner).ToList();
            owner.MouseMove += owner_MouseMove;
            owner.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(ToFocus), true);
//            CurrentMultiPageControls.ForEach(w =>
//            {
//                w.EditState = EditState.Normal;
//                w.MouseEnter += w_MouseEnter;
//                w.MouseLeave += w_MouseLeave;
//            });
        }

        private static void ToFocus(object sender, MouseButtonEventArgs e)
        {
            RestoreState();
            var current = GetCurrentMultiControl(e);
            if (current != null)
            {
                current.EditState = EditState.Focus;
            }
        }

        private static void w_MouseLeave(object sender, MouseEventArgs e)
        {
            //            CurrentMultiPageControls.ForEach(w => w.EditState = EditState.Normal);
            BaseMultiControl baseMultiControl = (sender as BaseMultiControl);
            sss.Text = baseMultiControl.GetType().Name + "离开";

            baseMultiControl.EditState = EditState.Normal;
        }

        static void w_MouseEnter(object sender, MouseEventArgs e)
        {
            BaseMultiControl baseMultiControl = (sender as BaseMultiControl);
            sss.Text = baseMultiControl.GetType().Name + "进来";
            baseMultiControl.EditState = EditState.Hover;
        }

        public static readonly DependencyProperty IsBusyProperty =
                DependencyProperty.Register("IsBusy", typeof (bool), typeof (BaseMultiControl), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((BaseMultiControl) s).OnIsBusyChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnIsBusyChanged(bool list)
        {}

        public bool IsBusy
        {
            get
            {
                return (bool) GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }
        public static readonly DependencyProperty SelectBorderWidthProperty =
                DependencyProperty.Register("SelectBorderWidth", typeof (Thickness), typeof (BaseMultiControl), new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty SelectBorderColorProperty =
                DependencyProperty.Register("SelectBorderColor", typeof (Brush), typeof (BaseMultiControl), new PropertyMetadata(null));

        public static readonly DependencyProperty HoverBorderColorProperty =
                DependencyProperty.Register("HoverBorderColor", typeof (Brush), typeof (BaseMultiControl), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public static readonly DependencyProperty HoverBorderWidthProperty =
                DependencyProperty.Register("HoverBorderWidth", typeof (Thickness), typeof (BaseMultiControl), new PropertyMetadata(new Thickness(1)));

        public static readonly DependencyProperty EditBorderWidthProperty =
                DependencyProperty.Register("EditBorderWidth", typeof (Thickness), typeof (BaseMultiControl), new PropertyMetadata(new Thickness(2)));

        public static readonly DependencyProperty EditBorderColorProperty =
                DependencyProperty.Register("EditBorderColor", typeof (Brush), typeof (BaseMultiControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty EditStateProperty =
                DependencyProperty.Register("EditState", typeof (EditState), typeof (BaseMultiControl), new PropertyMetadata(EditState.Normal, (s, e) =>
                {
                    var control = ((BaseMultiControl) s);
                    var editState = (EditState) e.NewValue;
                    switch (editState)
                    {
                        case EditState.Normal:
                            control.SelectBorderWidth = default(Thickness);
                            control.SelectBorderColor = null;
                            VisualStateManager.GoToState(control, "Normal", true);
                            break;
                        case EditState.Hover:
                            control.SelectBorderWidth = control.HoverBorderWidth;
                            control.SelectBorderColor = control.HoverBorderColor;
                            VisualStateManager.GoToState(control, "Selectable", true);

                            break;
                        case EditState.Focus:
                            control.SelectBorderWidth = control.EditBorderWidth;
                            control.SelectBorderColor = control.EditBorderColor;
                            VisualStateManager.GoToState(control, "Selected", true);

                            break;
                        case EditState.Run:
                            control.SelectBorderWidth = default(Thickness);
                            control.SelectBorderColor = null;
                            VisualStateManager.GoToState(control, "Normal", true);
                            break;
                    }
                    control.OnEditStateChanged(null);
                }));

        private Button EditButton;
        private ControlConfig _controlConfig;
        protected BaseMultiControl()
        {
            DefaultStyleKey = typeof (BaseControl);
            DataContextChanged += BaseMultiControl_DataContextChanged;
//            MouseEnter += BaseControl_MouseEnter;
//            MouseLeave += BaseControl_MouseLeave;
//            AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(EditClick), true);
            Edit += BaseMultiControl_Edit;
            SubControlNameList = new List<string>();
        }

        void BaseMultiControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (BasePage != null) BasePage.Post(this);
        }
        //返回当前控件的Name,如果Name为空,返回类型名
        public string GetName()
        {
            if (!string.IsNullOrEmpty(Name)) return Name;
            return GetType().Name;
        }

        public string MultiControlName
        {
            get
            {
                return GetName();
            }
        }
        /// <summary>
        /// 控件的数据操作事件(查询,更新,删除,添加)
        /// </summary>
        public event TEventHandler<object, MultiControlDataEventArgs> DataOperator;

        protected virtual void OnDataOperator(MultiControlDataEventArgs args)
        {
            var handler = DataOperator;
            if (handler != null) handler(this, args);
        }
        /// <summary>
        /// 实体类的所有类型
        /// </summary>
        public static List<Type> EntitTypes;

        /// <summary>
        /// 控件编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseMultiControl_Edit(object sender, MouseButtonEventArgs e)
        {
            BaseWindow rw = new BaseWindow();
            rw.Height = 600;
            rw.Width = 800;
            rw.Header = "控件属性";
            ControlEditPanel1 content = new ControlEditPanel1 {
                EntityProps = EntitTypes.Select(w=>w.Name).ToList(),
//                PropItems = ControlConfig.ItemConfig,
//                ControlDataConfig = ControlConfig.DataConfig,
//                ValidateConfigs = ControlConfig.ValidateConfigs,
//                EntityCode = ControlConfig.EntityCode,
                
            };
            Binding b1 = new Binding("ItemConfig") { Mode = BindingMode.TwoWay };
            content.SetBinding(ControlEditPanel1.PropItemsProperty, b1);

            Binding b2 = new Binding("DataConfig") { Mode = BindingMode.TwoWay};
            content.SetBinding(ControlEditPanel1.ControlDataConfigProperty, b2);

            Binding b3 = new Binding("ValidateConfigs") { Mode = BindingMode.TwoWay };
            content.SetBinding(ControlEditPanel1.ValidateConfigsProperty, b3);

            Binding b4 = new Binding("EntityCode") { Mode = BindingMode.TwoWay };
            content.SetBinding(ControlEditPanel1.EntityCodeProperty, b4);
            
            var list = GetSubControlList();
            list.Insert(0,this);
            content.SubControls = list;
            content.DataContext = ControlConfig;
            rw.Content = content;
            rw.ShowDialog();
            rw.Closed += rw_Closed;
        }

        private List<BaseMultiControl> GetSubControlList()
        {
            return SubControlNameList.Select(w=>FindName(w) as BaseMultiControl).Where(w => w != null).ToList();
        }

        public static readonly DependencyProperty BasePageProperty =
                DependencyProperty.Register("BasePage", typeof (BasePage), typeof (BaseMultiControl), new PropertyMetadata(default(BasePage)));

        public static TextBlock sss;

        /// <summary>
        /// 添加当前页面的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddPageCache(string key, object value)
        {
            CacheHelper.AddPageCache(BasePage.GetPageCacheName(), key, value);
        }

        /// <summary>
        /// 移除当前页面的一个缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemovePageCache(string key)
        {
            CacheHelper.RemovePageCache(BasePage.GetPageCacheName(), key);
        }
        /// <summary>
        /// 添加Navigator的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddNavigatorCache(string key, object value)
        {
            CacheHelper.AddNavigatorCache(key, value);
        }

        /// <summary>
        /// 移除Navigator的一个缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemoveNavigatorCache(string key)
        {
            CacheHelper.RemoveNavigatorCache(key);
        }
        /// <summary>
        /// 添加App的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddAppCache(string key, object value)
        {
            CacheHelper.AddAppCache( key, value);
        }

        /// <summary>
        /// 移除App的一个缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemoveAppCache(string key)
        {
            CacheHelper.RemoveAppCache(key);
        }
        public BasePage BasePage
        {
            get
            {
                return (BasePage) GetValue(BasePageProperty);
            }
            set
            {
                SetValue(BasePageProperty, value);
            }
        }
        void rw_Closed(object sender, WindowClosedEventArgs e)
        {
            var bw = sender as BaseWindow;
            var ce = bw.Content as ControlEditPanel1;
            if (ce != null) ce.SubControls.Skip(1).ForEach(w =>
            {
                w.OnEditEnd(null);
            });
            this.EditState = EditState.Normal;
            OnEditEnd(null);
        }
        /// <summary>
        ///     选择边框的宽度
        /// </summary>
        public Thickness SelectBorderWidth
        {
            get
            {
                return (Thickness) GetValue(SelectBorderWidthProperty);
            }
            set
            {
                SetValue(SelectBorderWidthProperty, value);
            }
        }

        /// <summary>
        ///     选择边框的颜色
        /// </summary>
        public Brush SelectBorderColor
        {
            get
            {
                return (Brush) GetValue(SelectBorderColorProperty);
            }
            set
            {
                SetValue(SelectBorderColorProperty, value);
            }
        }

        /// <summary>
        ///     编辑状态时,鼠标移动到控件上方时,边框的颜色
        /// </summary>
        public Brush HoverBorderColor
        {
            get
            {
                return (Brush) GetValue(HoverBorderColorProperty);
            }
            set
            {
                SetValue(HoverBorderColorProperty, value);
            }
        }

        /// <summary>
        ///     编辑状态时,鼠标移动到控件上方时,边框的宽度
        /// </summary>
        public Thickness HoverBorderWidth
        {
            get
            {
                return (Thickness) GetValue(HoverBorderWidthProperty);
            }
            set
            {
                SetValue(HoverBorderWidthProperty, value);
            }
        }

        /// <summary>
        ///     编辑选择中边框宽度
        /// </summary>
        public Thickness EditBorderWidth
        {
            get
            {
                return (Thickness) GetValue(EditBorderWidthProperty);
            }
            set
            {
                SetValue(EditBorderWidthProperty, value);
            }
        }

        /// <summary>
        ///     编辑选择中边框颜色
        /// </summary>
        public Brush EditBorderColor
        {
            get
            {
                return (Brush) GetValue(EditBorderColorProperty);
            }
            set
            {
                SetValue(EditBorderColorProperty, value);
            }
        }

        /// <summary>
        ///     是否选择进行编辑
        /// </summary>
        public EditState EditState
        {
            get
            {
                return (EditState) GetValue(EditStateProperty);
            }
            set
            {
                SetValue(EditStateProperty, value);
            }
        }

        /// <summary>
        ///     可以编辑的属性列表
        /// </summary>
        public virtual List<string> EditablePropters
        {
            get
            {
                return new List<string> {"Width", "Height"};
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EditButton = GetTemplateChild("EditButton") as Button;
            if (EditButton != null) EditButton.Click += EditButton_Click;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            OnEdit(null);
        }

        public event MouseButtonEventHandler Edit;
        public event MouseButtonEventHandler EditEnd;

        protected virtual void OnEditEnd(MouseButtonEventArgs e)
        {
            ControlConfig = ControlConfig;
            var handler = EditEnd;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnEdit(MouseButtonEventArgs e)
        {
            MouseButtonEventHandler handler = Edit;
            if (handler != null) handler(this, e);
        }

        public event MouseButtonEventHandler EditStateChanged;

        protected virtual void OnEditStateChanged(MouseButtonEventArgs e)
        {
            MouseButtonEventHandler handler = EditStateChanged;
            if (handler != null) handler(this, e);
        }

        private void EditClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
//            if (EditState == EditState.Hover)
//            {
//                RetoreState();
//                EditState = EditState.Focus;
//            }
//            mouseButtonEventArgs
        }

        private void BaseControl_MouseLeave(object sender, MouseEventArgs e)
        {
//            if (EditState == EditState.Hover)
//            {
//                EditState = EditState.Normal;
//            }
        }

        private void BaseControl_MouseEnter(object sender, MouseEventArgs e)
        {
//            if (EditState == EditState.Normal)
//            {
//                EditState = EditState.Hover;
//            }
        }

        /// <summary>
        ///     加载设置
        /// </summary>
        /// <param name="configStr"></param>
        public abstract void LoadConfig(string configStr);

        public virtual void LoadPropConfig()
        {
            LoadPropConfig(_controlConfig.ItemConfig);
        }
        
        /// <summary>
        /// 加载属性设置
        /// </summary>
        /// <param name="itmes"></param>
        public virtual void LoadPropConfig(List<PropItem> itmes)
        {
            var ps = GetEditableProps();
            var ps1 = ps.Select(w => new {A = ReflectionHelper.GetAttribute<EditableAttribute>(w), P = w}).ToList();
            itmes.ForEach(w =>
            {
                var attr = ps1.FirstOrDefault(z=>z.A!=null&&z.A.Code == w.PropName);
                if (attr == null) attr = ps1.FirstOrDefault(z => z.A != null && z.P.Name == w.PropName);
                if (attr != null)
                {
                    setPropValue(attr.P,w.From);
                }
            });
        }

        /// <summary>
        /// 根据设置返回存储过程的参数
        /// </summary>
        /// <returns></returns>
        protected List<STParamete> GetParameter()
        {
            return ControlConfig.DataConfig.Parametes.Select(w =>
            {
                STParamete sp = new STParamete();
                if (!string.IsNullOrEmpty(w.Value))
                {
                    sp.Value = GetValue1(w.Value).ToString2();
                }
                sp.Name = w.Name;
                return sp;
            }).ToList();
        }
        /// <summary>
        /// 把值设置到APP中,或导航中,或页面中.
        /// </summary>
        /// <param name="item"></param>
        public void SetValue(OutputValue item)
        {
            var _from = item.From;
            if (string.IsNullOrEmpty(_from)) return;
            if (_from.StartsWith("####")) //,###APP中查找 
            {
                var key = _from.Substring(3);
                CacheHelper.AddAppCache(key,item.Value);
            }
            else if (_from.StartsWith("###")) //,##导航中查找,
            {
                var key = _from.Substring(2);
                CacheHelper.AddNavigatorCache(key, item.Value);
                
            }
            else if (_from.StartsWith("##")) //页面中查找
            {
                var key = _from.Substring(1);
                BasePage.AddPageCache(key,item.Value);//可以使子页面重写该方法,
//                CacheHelper.AddPageCache(this.BasePage.GetPageCacheName(), key, item.Value);
            }
            if (_from.StartsWith("#")) //当前控件中查找
            {
                var key = _from.Substring(1);
                var p = GetType().GetProperties().FirstOrDefault(w => w.Name == key);
                if (p != null)
                {
                    p.SetValue(this, item.Value, null);
                }
            }
//            else if (_from != "")
//            {
//                return _from;
//            }
//            return null; 
        }
//        public void SetValue(List<OutputValue> list)
//        {
//            list.ForEach(SetValue);
//        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="_from"></param>
        /// <returns></returns>
        public object GetValue1(string _from)
        {
            if (string.IsNullOrEmpty(_from)) return null;
            if (_from.StartsWith("####")) //,###APP中查找 
            {
                var key = _from.Substring(3);
                var val = CacheHelper.GetAppCache(key);
                return val;
            }
            else if (_from.StartsWith("###")) //,##导航中查找,
            {
                var key = _from.Substring(2);
                var val = CacheHelper.GetNavigatorCache(key);
                return val;
            }
            else if (_from.StartsWith("##")) //页面中查找
            {
                var key = _from.Substring(1);
                var val = CacheHelper.GetPageCache(key);
                return val;
            }
            else if (_from.StartsWith("#")) //控件属性中
            {
                var key = _from.Substring(1);
                var p = GetType().GetProperties().FirstOrDefault(w => w.Name == key);
                if (p != null)
                {
                    return p.GetValue(this, null);
                }
                return null;
            }
            else if (_from != "")
            {
                return _from;
            }
            return null;
        }
        /// <summary>
        /// 设置当前属性的值
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="_from"></param>
        private void setPropValue(PropertyInfo propertyInfo, string _from)
        {
            if (string.IsNullOrEmpty(_from)) return;
            if (_from.StartsWith("###")) //,###APP中查找 
            {
                var key = _from.Substring(3);
                var val = CacheHelper.GetAppCache(key);
                if (val != null)
                {
                    propertyInfo.SetValue(this, val, null);
                }
            }
            else if (_from.StartsWith("##")) //,##导航中查找,
            {
                var key = _from.Substring(2);
                var val = CacheHelper.GetNavigatorCache(key);
                if (val != null)
                {
                    propertyInfo.SetValue(this, val, null);
                }
            }
            else if (_from.StartsWith("#")) //页面中查找
            {
                var key = _from.Substring(1);
                var val = CacheHelper.GetPageCache(key);
                if (val != null)
                {
                    propertyInfo.SetValue(this, val, null);
                }
            }
            else if(_from != "")
            {
                var val = JsonHelper.JsonDeserialize(propertyInfo.PropertyType, _from);
                propertyInfo.SetValue(this,val,null);
            }
        }

        //        /// <summary>
//        ///     保存设置
//        /// </summary>
//        public string SaveConfig()
//        {
//            
//        }

        /// <summary>
        /// 自动和数据源绑定
        /// </summary>
        internal void Bind()
        {
            var dc = DataContext;
            if (dc == null) return;
            var dps = dc.GetType().GetProperties();
            var bs = GetType().GetProperties(BindingFlags.Static);
            var ps = GetEditableProps();
            
            ps.ForEach(w =>
            {
                var p = dps.FirstOrDefault(z => z.Name == w.Name);
                if (p != null)
                {
                    var n = p.Name + "Property";
                    var bs1 = bs.FirstOrDefault(z => z.Name == n);
                    Binding b = new Binding(p.Name);
                    b.Source = DataContext;
                    b.Mode = BindingMode.TwoWay;
                    var dp = bs1.GetValue(null, null) as DependencyProperty;
                    this.SetBinding(dp, b);
                }
            });
        }

        private List<PropertyInfo> GetEditableProps()
        {
            var ps = ReflectionHelper.GetPropertyInfoByCustomAttribute<EditableAttribute>(GetType());
            return ps;
        }

        /// <summary>
        /// 返回当前控件的设置
        /// </summary>
        public ControlConfig ControlConfig
        {
            get
            {
                if (_controlConfig == null)
                {
                    _controlConfig = new ControlConfig();
                    if (string.IsNullOrEmpty(Name))
                    {
                        _controlConfig.Name = this.GetType().Name;
                    }
                    else
                    {
                        _controlConfig.Name = Name;
                    }
                    _controlConfig.ObjectCode = _controlConfig.Name;
                    AddOrUpdateItemConfig();
                }
                GatherSubControlConfig(ref _controlConfig);
                return _controlConfig;
            }
            set
            {
                _controlConfig = value;
                LoadPropConfig(_controlConfig.ItemConfig);
                SetSubControlConfig(ref _controlConfig);
                OnControlConfigChanged(_controlConfig);
                
                AddOrUpdateItemConfig();
            }
        }

        /// <summary>
        /// 收集子控件的设置
        /// </summary>
        /// <param name="controlConfig"></param>
        private void GatherSubControlConfig(ref ControlConfig controlConfig)
        {
            var list = GetSubControlList();
            List<ControlConfig> subList = new List<ControlConfig>();
            list.ForEach(w =>
            {
                subList.Add(w.ControlConfig);
            });
            controlConfig.SubControlConfigs.Clear();
            controlConfig.SubControlConfigs.AddRange(subList);
        }
        /// <summary>
        /// 把设置应用到子控件中
        /// </summary>
        /// <param name="controlConfig"></param>
        private void SetSubControlConfig(ref ControlConfig controlConfig)
        {
            var list = GetSubControlList();
            List<ControlConfig> subList = controlConfig.SubControlConfigs;
            list.ForEach(w =>
            {
                var n = w.MultiControlName;
                var cItem = subList.FirstOrDefault(z => z.Name == n);
                if (cItem != null)
                {
                    w.ControlConfig = cItem;
                }
            });
            controlConfig.SubControlConfigs.AddRange(subList);
        }
        /// <summary>
        /// 控件的"设置属性"变化
        /// </summary>
        /// <param name="cc"></param>
        protected virtual void OnControlConfigChanged(ControlConfig cc)
        {
            
        }

        /// <summary>
        /// 返回控件的设置,或更新控件的设置
        /// </summary>
        private void AddOrUpdateItemConfig()
        {
            var types = GetEditableProps();
            types.ForEach(w =>
            {
                var cCode = string.IsNullOrEmpty(w.Name) ? w.GetType().Name : w.Name;
                var attr = ReflectionHelper.GetAttribute<EditableAttribute>(w);
                var propName = attr.Code ?? cCode;
                var displayName = attr.DisplayName ?? cCode;
                var item = _controlConfig.ItemConfig.FirstOrDefault(z => z.PropName == propName);
                if (item == null)
                {
                    PropItem propItem = new PropItem {
                        PropName = propName,
                        DisplayName = displayName,
                    };
//                    if (propItem.From == null)//如果没有设置
                    {
                        var v = w.GetValue(this, null);
                        if (v != null)
                        {
                            propItem.From = v.ToString();
                        }
                    }
                    _controlConfig.ItemConfig.Add(propItem);
                }
                else
                {
                    if (item.From == null)//如果没有默认值
                    {
                        var v = w.GetValue(this, null);
                        if (v != null)
                        {
                            item.From = v.ToString();
                        }
                    }
                    item.DisplayName = displayName;
                }
            });
        }

//        //验证_controlConfig中的属性设置是否完整
//        private void CheckItems()
//        {
//            if (_controlConfig != null)
//            {
//                if(_controlConfig.ItemConfig == null)_controlConfig.ItemConfig = new List<PropItem>();
//            }
//        }
    }
}