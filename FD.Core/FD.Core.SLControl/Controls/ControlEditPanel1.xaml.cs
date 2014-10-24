using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using SLComponse.Validate;
using SLControls.Editors;
using SLControls.helpers;
using SLControls.PageConfigs;
using STComponse.CFG;
using Telerik.Windows.Controls;
using SelectionChangedEventArgs = Telerik.Windows.Controls.SelectionChangedEventArgs;


namespace SLControls.Controls
{
    /// <summary>
    ///     第二版本的复合控件属性编辑器
    /// </summary>
    public partial class ControlEditPanel1
    {
        public static readonly DependencyProperty SubControlsProperty =
                DependencyProperty.Register("SubControls", typeof(List<BaseMultiControl>), typeof(ControlEditPanel1), new PropertyMetadata(default(List<BaseMultiControl>), (s, e) =>
                {
                    ((ControlEditPanel1)s).OnSubControlsChanged(e.NewValue as List<BaseMultiControl>);
                }));

        private void OnSubControlsChanged(List<BaseMultiControl> list)
        {
            
        }

        public List<BaseMultiControl> SubControls
        {
            get
            {
                return (List<BaseMultiControl>)GetValue(SubControlsProperty);
            }
            set
            {
                SetValue(SubControlsProperty, value);
            }
        }
        public static readonly DependencyProperty PropItemsProperty =
                DependencyProperty.Register("PropItems", typeof (List<PropItem>), typeof (ControlEditPanel1), new PropertyMetadata(default(List<PropItem>), (s, e) => ((ControlEditPanel1) s).OnPropItemsChanged(e.NewValue as List<PropItem>)));

        public static readonly DependencyProperty ControlDataConfigProperty =
                DependencyProperty.Register("ControlDataConfig", typeof (ControlDataConfig), typeof (ControlEditPanel1), new PropertyMetadata(default(ControlDataConfig)));

        public ControlEditPanel1()
        {
            InitializeComponent();
            Loaded += ControlEditPanel1_Loaded;
        }

        void ControlEditPanel1_Loaded(object sender, RoutedEventArgs e)
        {
            SubControlList.ItemsSource = SubControls.Select(w => w.MultiControlName).ToList();
        }

        public static readonly DependencyProperty EntityCodeProperty =
                DependencyProperty.Register("EntityCode", typeof(string), typeof(ControlEditPanel1), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((ControlEditPanel1)s).OnEntityCodeChanged(e.NewValue as string);
                }));

        private void OnEntityCodeChanged(string list)
        {}

        public string EntityCode
        {
            get
            {
                return (string)GetValue(EntityCodeProperty);
            }
            set
            {
                SetValue(EntityCodeProperty, value);
            }
        }
        public static readonly DependencyProperty EntityPropsProperty =
                DependencyProperty.Register("EntityProps", typeof(List<string>), typeof(ControlEditPanel1), new PropertyMetadata(default(List<string>), (s, e) =>
                {
                    ((ControlEditPanel1)s).OnEntityTypesChanged(e.NewValue as List<string>);
                }));

        private void OnEntityTypesChanged(List<string> list)
        {
                
        }

        public List<string> EntityProps
        {
            get
            {
                return (List<string>)GetValue(EntityPropsProperty);
            }
            set
            {
                SetValue(EntityPropsProperty, value);
            }
        }
        
        public static readonly DependencyProperty ValidateConfigsProperty =
                DependencyProperty.Register("ValidateConfigs", typeof(List<ColumnValidataConfig>), typeof(ControlEditPanel1), new PropertyMetadata(default(List<ColumnValidataConfig>), (s, e) =>
                {
                    ((ControlEditPanel1)s).OnValidateConfigsChanged(e.NewValue as List<ColumnValidataConfig>);
                }));

        private void OnValidateConfigsChanged(List<ColumnValidataConfig> list)
        {}

        public List<ColumnValidataConfig> ValidateConfigs
        {
            get
            {
                return (List<ColumnValidataConfig>)GetValue(ValidateConfigsProperty);
            }
            set
            {
                SetValue(ValidateConfigsProperty, value);
            }
        }
        /// <summary>
        ///     控件的数据源设置
        /// </summary>
        public ControlDataConfig ControlDataConfig
        {
            get
            {
                return (ControlDataConfig) GetValue(ControlDataConfigProperty);
            }
            set
            {
                SetValue(ControlDataConfigProperty, value);
            }
        }

        /// <summary>
        ///     控件的属性设置
        /// </summary>
        public List<PropItem> PropItems
        {
            get
            {
                return (List<PropItem>) GetValue(PropItemsProperty);
            }
            set
            {
                SetValue(PropItemsProperty, value);
            }
        }

        private void OnPropItemsChanged(List<PropItem> propItems)
        {
            InitControlSource();
        }

        private void InitControlSource()
        {
//            content.Children.Clear();
//            PropItems.ForEach(w =>
//            {
//                var p = new PropTextEdit();
//                //                Binding b = new Binding();
//                //                b.Source = w;
//                //                b.Mode = BindingMode.TwoWay;
//                //                p.SetBinding(PropTextEdit.ConfigProperty, b);
//                p.Config = w;
//                content.Children.Add(p);
//            });
        }

        //        internal void OnConfigChanged(PropItem propItem)
        //        {
        //            throw new NotImplementedException();
        //        }
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (sender as RadComboBox);
            if (cb == null) return;
            var type1 = cb.SelectedValue as string;
            if (type1 == null) return;
            if (!string.IsNullOrEmpty(EntityCode) && type1 != EntityCode)
            {
                AlertHelper.Confirm(this, "重新设置对象会覆盖原来的设置，是否继续？", () => getDefaultEntityProps(type1),()=>cb.SelectedValue = EntityCode);
//                RadConfirm radConfirm = new RadConfirm {  };
//                radConfirm.Ok += (s, ee) =>
//                {
//                    getDefaultEntityProps(type1);
//                };
//                radConfirm.Cancel += (s, ee) =>
//                {
//                    cb.SelectedValue = EntityCode;
//                };
//                
//                RadWindow.ConfigureModal(radConfirm, new DialogParameters { Content = "重新设置对象会覆盖原来的设置，是否继续？", Header = "提示", CancelButtonContent = "取消", OkButtonContent = "确定", Owner = this.ParentOfType<ContentControl>() });
//                RadWindow.Confirm("重新设置对象会覆盖原来的设置，是否继续？", (ss, ee) =>
//                {
//                    if (ee.DialogResult.HasValue && ee.DialogResult.Value)
//                    {
//                        getDefaultEntityProps(type1);
//                    }
//                });
            }
            else
            {
                if (type1 != EntityCode)
                getDefaultEntityProps(type1);
            }
        }

        private void getDefaultEntityProps(string type1)
        {
            var type = BaseMultiControl.EntitTypes.FirstOrDefault(w => w.Name == type1);
            var entity = FW.FwConfig.FindObject(w => w.ObjectCode == type.Name);
            ValidateConfigs = getValidConfig(type, entity);
        }

        
        private List<ColumnValidataConfig> getValidConfig(Type type,EDataObject entity)
        {
            EntityCode = entity.ObjectCode;
            PropertyInfo[] type1 = null;
            if (type.BaseType != null)
            {
                type1 = type.BaseType.GetProperties();
            }
            var props = type.GetProperties().ToList();
            if (type1 != null) props = props.Where(w => type1.All(z => z.Name != w.Name)).ToList();
            var re = props.Select(w =>
            {
                var c = entity.Property.FirstOrDefault(z => z.Code == w.Name);
                if (c != null)
                {
                    return new ColumnValidataConfig(c.Code)
                    {
                        DisplayName = c.Name,
                    };
                }
                return null;
            }).Where(w=>w!=null).ToList();
            re[0].RegexPattern = "123123123123123";

            return re;
        }

        private void Selector_OnSelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            string s = SubControlList.SelectedItem as string;
            var current = SubControls.FirstOrDefault(w => w.MultiControlName == s); ;
            if (current != null)
            {
                this.DataContext = current.ControlConfig;
                //                UpdateBindingSource(current, PropItemsProperty);
                //                UpdateBindingSource(current, ControlDataConfigProperty);
                //                UpdateBindingSource(current, ValidateConfigsProperty);
                //                UpdateBindingSource(current, EntityCodeProperty);
            }
        }

        private void UpdateBindingSource(BaseMultiControl current, DependencyProperty propItemsProperty)
        {
            var items = GetBindingExpression(propItemsProperty);
            var parentBinding = items.ParentBinding;
            parentBinding.Source = current;
        }
    }
}