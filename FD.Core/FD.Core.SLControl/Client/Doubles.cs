using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using SLControls.Editors;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools.Configs;
using STComponse.CFG;
using STComponse.GCode;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.PropertyGrid.Converters;
using Telerik.Windows.Documents.FormatProviders.MsRichTextBoxXaml;
using Selector = Telerik.Windows.Controls.Selector;


namespace SLControls.Client
{
    /// <summary>
    /// 大数据的绑定
    /// </summary>
    public class Doubles
    {
        /// <summary>
        /// 绑定属性到控件
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ui"></param>
        /// <param name="groups"></param>
        public static void SetBinding(ObservableCollection<BigFieldItem> items, UIElement ui, ObservableCollection<BigFieldEditor.Group> groups,string savePath)
        {
            var childs = ControlExtends.ChildrenOfType<FrameworkElement>(ui).ToList();
            var singles = items.Where(w => string.IsNullOrEmpty(w.GroupCode)).ToList();
            var hasGroups = items.Where(w => !string.IsNullOrEmpty(w.GroupCode)).ToList();
            singles.ForEach(w =>
            {
                var item = childs.FirstOrDefault(z => z.Name == w.Code);
                if (item != null)
                {
                    SetBinding(w, item, savePath);
                }
            }); 
            hasGroups.GroupBy(w => w.GroupCode).ForEach(w =>
            {
                var groupName = w.Key;
                var groupTemp = groups.FirstOrDefault(z => z.Name == "_"+groupName);
                if (groupTemp != null)
                {
                    var item = childs.FirstOrDefault(z => z.Name == "_" + groupName) as Panel;
                    if (item != null)
                    {
                        //每一条的模板反序列化
                        var bigFieldItems = w.ToList();
                        var groupItems = bigFieldItems.GroupBy(v => v.GroupIndex).OrderBy(v=>v.Key).ToList();
                        //遍历分组中的每一条
                        var index = 1;
                        groupItems.ForEach(z =>
                        {
                            //便利每一条中的每个控件
                            AddGroupItem(items,new ObservableCollection<BigFieldItem>(z.ToList()), groupTemp.Template, item, index++,savePath);
                        });
                        InitGroupIndex(item);
                    }
                }
            });
        }
        /// <summary>
        /// 每个组的的每一条
        /// </summary>
        /// <param name="z"></param>
        /// <param name="groupTemp"></param>
        /// <param name="_group"></param>
        private static void AddGroupItem(ObservableCollection<BigFieldItem> fullItems, ObservableCollection<BigFieldItem> z, string groupTemp, Panel _group, int index, string savePath)
        {
            var fe = XamlReader.Load(groupTemp) as Panel;
            if (fe != null) // _Add _Del _Edit
            {
                fe.Tag = z;

                var childs1 = ControlExtends.ChildrenOfType<FrameworkElement>(fe).ToList();
                var addButton = childs1.FirstOrDefault(v => v.Name == add) as ButtonBase;
                if (addButton != null)
                    addButton.Click += (s, e) =>
                    {
                        var bigFieldItems = z.JsonClone();
                        bigFieldItems.ForEach(v => v.Value = v.DefaultValue);
                        AddGroupItem(fullItems, bigFieldItems, groupTemp, _group, _group.Children.Count + 1,savePath);
                        fullItems.AddRange(bigFieldItems);
                        InitGroupIndex(_group);
                    };
                var delButton = childs1.FirstOrDefault(v => v.Name == del) as ButtonBase;
                if (delButton != null) delButton.Click += (s, e) =>
                {
                    z.ForEach(v => fullItems.Remove(v));
                    _group.Children.Remove(fe);
                    InitGroupIndex(_group);
                };
                //每一条中的所有控件
                z.ForEach(g =>
                {
                    g.GroupIndex = index;
                    
                    
                    var item1 = childs1.FirstOrDefault(x => x.Name == g.Code);
                    if (item1 != null)
                    {
                        SetBinding(g, item1, savePath);
                    }
                });
            }
            _group.Children.Add(fe);
        }
        const string del = "_Del";
        const string add = "_Add";

        private static void InitGroupIndex(Panel item)
        {
            var panels = item.Children.OfType<Panel>().ToList();
            panels.ForEachIndex((w ,i)=>
            {
                var dataItems = (w.Tag as IList<BigFieldItem>);
                dataItems.ForEach(x => x.GroupIndex = i+1);
                var controls = ControlExtends.ChildrenOfType<ButtonBase>(w);
                var addBtn = controls.FirstOrDefault(x => x.Name == add);
                addBtn.Visibility = (i  < panels.Count - 1) ? Visibility.Collapsed : Visibility.Visible;
            });
        }

        private static void SetBinding(BigFieldItem item, NumberBox fe)
        {
            var b = new Binding("Value") { Source = item, Mode = BindingMode.TwoWay,Converter = new NumericTypeConverter()};
            fe.SetBinding(NumberBox.NumberProperty, b);
        }
        private static void SetBinding(BigFieldItem item, TextBox fe)
        {
            var b = new Binding("Value") {Source = item, Mode = BindingMode.TwoWay};
            fe.SetBinding(TextBox.TextProperty, b);
        }
        private static void SetBinding(BigFieldItem item, FrameworkElement fe,string savePath)
        {
            if (fe is NumberBox) SetBinding(item, fe as NumberBox);
            else if (fe is TextBox) SetBinding(item, fe as TextBox);
            else if (fe is EditItemPicture) SetBinding(item, fe as EditItemPicture, savePath);
            else if (fe is RadComboBox) SetBinding(item, fe as RadComboBox);
            else
            {
                throw  new Exception("未知的控件类型:"+fe.GetType().FullName);
            }
        }

        private static void SetBinding(BigFieldItem item, RadComboBox fe)
        {
            var ddc = item.DDConfig;
            if (ddc != null)
            {
                fe.ItemsSource = ddc.Items.Select(w=>w.DisplayName).ToList();
            }
            var b = new Binding("Value") { Source = item, Mode = BindingMode.TwoWay };
            fe.SetBinding(Selector.SelectedValueProperty, b);
        }
        private static void SetBinding(BigFieldItem item, EditItemPicture fe,string savePath)
        {
            var b = new Binding("Value") { Source = item, Mode = BindingMode.TwoWay };
            fe.SavePath = savePath;
            if (string.IsNullOrEmpty(fe.UploadServerPath)) fe.UploadServerPath = ActionSetting.UploadUri.ToString();
            fe.SetBinding(BaseEditItem.TextProperty, b);
        }
    }
}