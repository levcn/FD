using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Fw.Extends;
using ProjectCreater.Commands;
using ProjectCreater.Settings;
using STComponse.CFG;
//using STComponse.ObjectConfig;
using STComponse.GCode;
using WPFControls;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// SBigFieldPropList.xaml 
    /// </summary>
    public partial class SBigFieldPropList 
    {
        public static readonly DependencyProperty EDataObjectProperty =
                DependencyProperty.Register("EDataObject", typeof (EDataObject), typeof (SBigFieldPropList), new PropertyMetadata(default(EDataObject)));

        public EDataObject EDataObject
        {
            get
            {
                return (EDataObject) GetValue(EDataObjectProperty);
            }
            set
            {
                SetValue(EDataObjectProperty, value);
            }
        }

        public static readonly DependencyProperty IsVirtualProperty =
                DependencyProperty.Register("IsVirtual", typeof (bool), typeof (SBigFieldPropList), new PropertyMetadata(default(bool)));

        public bool IsVirtual
        {
            get
            {
                return (bool) GetValue(IsVirtualProperty);
            }
            set
            {
                SetValue(IsVirtualProperty, value);
            }
        }
        public SBigFieldPropList()
        {
            InitializeComponent();
            dataGrid.CanUserAddRows = false;
            AddOrderPropCommand = new DelegateCommand(o => !CheckHaveOrderProp(), o => AddOrderProp());
            AddCancelPropCommand = new DelegateCommand(o => !CheckHaveCancelProp(), o => AddCancelProp());
            AddNamePropCommand = new DelegateCommand(o => !CheckHaveNameProp(), o => AddNameProp());
            dataGrid.RowEditEnding += dataGrid_RowEditEnding;
            dataGrid.BeginningEdit += dataGrid_BeginningEdit;
            dataGrid.CellEditEnding += dataGrid_CellEditEnding;
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;
            dataGrid.SourceUpdated += dataGrid_SourceUpdated;
            dataGrid.TargetUpdated += dataGrid_TargetUpdated;
            dataGrid.PreviewMouseDown += dtgSchemeApp_PreviewMouseDown;
            dataGrid.PreviewMouseLeftButtonDown+=dataGrid_PreviewMouseLeftButtonDown;
//            BigFieldConfigItem p = new BigFieldConfigItem();
//            p.PropertyChanged += p_PropertyChanged;
        }
        private void dtgSchemeApp_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsUnderTabHeader(e.OriginalSource as DependencyObject))
                CommitTables(this.dataGrid);
        }

        private bool IsUnderTabHeader(DependencyObject control)
        {
            if (control is TabItem)
                return true;
            DependencyObject parent = VisualTreeHelper.GetParent(control);
            if (parent == null)
                return false;
            return IsUnderTabHeader(parent);
        }

        private void CommitTables(DependencyObject control)
        {
            if (control is DataGrid)
            {
                DataGrid grid = control as DataGrid;
                grid.CommitEdit(DataGridEditingUnit.Row, true);
                return;
            }
            int childrenCount = VisualTreeHelper.GetChildrenCount(control);
            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
                CommitTables(VisualTreeHelper.GetChild(control, childIndex));
        }

        public DataGridCell GetDataGridCell(DataGrid dataGird, int index, int index1)
        {
            return GetDataGridCell(dataGird,GetDataGridRow(dataGird, index), index1);
        }
        public DataGridCell GetDataGridCell(DataGrid dataGird, DataGridRow rowContainer, int index1)
        {
//            DataGridRow rowContainer = GetDataGridRow(dataGird, index);
            DataGridCell cell = null;
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(index1);
                if (cell == null)
                {
                    dataGird.ScrollIntoView(rowContainer, dataGird.Columns[index1]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(index1);

                }

            }
            return cell;
        }

        public DataGridRow GetDataGridRow(DataGrid dataGird, int index)
        {
            DataGridRow row = (DataGridRow)dataGird.ItemContainerGenerator.ContainerFromIndex(0);

            if (row == null)
            {

                dataGird.UpdateLayout();

                row = (DataGridRow)dataGird.ItemContainerGenerator.ContainerFromIndex(0);

            }
            return row;
            
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T childContent = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                childContent = v as T;
                if (childContent == null)
                {
                    childContent = GetVisualChild<T>(v);
                }
                if (childContent != null)
                {
                    break;
                }
            }
            return childContent;
        }


        private void dataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
//            e.GetPosition()
            DataGridCell cell = sender as DataGridCell;
            if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {
                if (!cell.IsFocused)
                {
                    cell.Focus();
                }
                DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                if (dataGrid != null)
                {
                    if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
                    {
                        if (!cell.IsSelected)
                            cell.IsSelected = true;
                    }
                    else
                    {
                        DataGridRow row = FindVisualParent<DataGridRow>(cell);
                        if (row != null && !row.IsSelected)
                        {
                            row.IsSelected = true;
                        }
                    }
                }
            }
        }    
        static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        } 
        void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
//            (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(0);
//            e.Row
            var ddd = e.EditingElement.GetBindingExpression(TextBox.TextProperty);
            var list = ItemSource.OfType<BigFieldItem>().ToList();
            if (ddd != null && oldCode != null)
            {
                var tb = e.EditingElement as TextBox;
                var newCode = tb.Text;
                var p = e.Row.DataContext as BigFieldItem;

                var path = ddd.ParentBinding.Path.Path;
                if (path == "Code")
                {
                    var item = list.FirstOrDefault(w => w.Code == newCode);
                    if (item != null && item != p)
                    {
                        tb.Text = oldCode.Code;
                        MessageBox.Show(string.Format("您输入的代码有重名！"), "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    VersionManage.Current.CurrentVersion.AttributeRename(EDataObject.ObjectCode, oldCode.Code, newCode);
                }
                if (path == "Name")
                {
                    var item = list.FirstOrDefault(w => w.DisplayName == newCode);
                    if (item != null && item != p)
                    {
                        tb.Text = oldCode.DisplayName;
                        MessageBox.Show(string.Format("您输入的名称有重名！"), "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }
        }

        private BigFieldItem oldCode = null;
        void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            oldCode = StaticEx.ToJson((e.Row.DataContext as BigFieldItem)).ToObject<BigFieldItem>();
        }

        void dataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            
        }

        void dataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            
        }

//        void p_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            
//        }

        void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddOrderPropCommand.InvalideCanExecute();
            AddCancelPropCommand.InvalideCanExecute();
            AddNamePropCommand.InvalideCanExecute();
        }

        void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
//            Validation.GetErrors()
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var p = e.Row.DataContext as BigFieldItem;
            }
            AddOrderPropCommand.InvalideCanExecute();
            AddCancelPropCommand.InvalideCanExecute();
            AddNamePropCommand.InvalideCanExecute();
        }
        private bool CheckHaveNameProp()
        {
            return ItemSource != null && ItemSource.OfType<BigFieldItem>().Any(w => "Name".Equals(w.Code, StringComparison.OrdinalIgnoreCase));
        }
        private bool CheckHaveOrderProp()
        {
            return ItemSource != null && ItemSource.OfType<BigFieldItem>().Any(w => "OrderID".Equals(w.Code, StringComparison.OrdinalIgnoreCase));
        }
        private bool CheckHaveCancelProp()
        {
            return ItemSource != null && ItemSource.OfType<BigFieldItem>().Any(w => "IsCancel".Equals(w.Code, StringComparison.OrdinalIgnoreCase));
        }
        private void AddNameProp()
        {
//            edata
            if (CheckHaveNameProp()) return;
            AddItem(new BigFieldItem
            {
                Code = "Name",
                ItemType = "字符串",
//                IsShow = true,
                ColumnSize = "20",
                //                        InitValue = "newid()",
                //                        IsPrimaryKey = true,
                //                        IsOutKey = true,
                ID = Guid.NewGuid(),
                DisplayName = "名称",
            });
        }
        private void AddOrderProp()
        {
            if (CheckHaveOrderProp()) return;
            AddItem(new BigFieldItem
            {
                Code = "OrderID",
                ItemType = Property.整数,
                //                        InitValue = "newid()",
                //                        IsPrimaryKey = true,
                //                        IsOutKey = true,
                ID = Guid.NewGuid(),
                DisplayName = "排序字段",
            });
        }
        private void AddCancelProp()
        {
            if (CheckHaveCancelProp()) return;
            AddItem(new BigFieldItem
            {
                Code = "IsCancel",
                ItemType = Property.整数,
                //                        InitValue = "newid()",
                //                        IsPrimaryKey = true,
                //                        IsOutKey = true,
                ID = Guid.NewGuid(),
                DisplayName = "是否注销",
            });
        }

        public static readonly DependencyProperty AddNamePropCommandProperty =
                DependencyProperty.Register("AddNamePropCommand", typeof (DelegateCommand), typeof (SBigFieldPropList), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand AddNamePropCommand
        {
            get
            {
                return (DelegateCommand) GetValue(AddNamePropCommandProperty);
            }
            set
            {
                SetValue(AddNamePropCommandProperty, value);
            }
        }
        public static readonly DependencyProperty AddCancelPropCommandProperty =
                DependencyProperty.Register("AddCancelPropCommand", typeof (DelegateCommand), typeof (SBigFieldPropList), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand AddCancelPropCommand
        {
            get
            {
                return (DelegateCommand) GetValue(AddCancelPropCommandProperty);
            }
            set
            {
                SetValue(AddCancelPropCommandProperty, value);
            }
        }
        public static readonly DependencyProperty AddOrderPropCommandProperty =
                DependencyProperty.Register("AddOrderPropCommand", typeof (DelegateCommand), typeof (SBigFieldPropList), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand AddOrderPropCommand
        {
            get
            {
                return (DelegateCommand) GetValue(AddOrderPropCommandProperty);
            }
            set
            {
                SetValue(AddOrderPropCommandProperty, value);
            }
        }
        public static readonly DependencyProperty EditableProperty =
                DependencyProperty.Register("Editable", typeof (bool), typeof (SBigFieldPropList), new PropertyMetadata(true));

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
        public static readonly DependencyProperty ItemSourceProperty =
                DependencyProperty.Register("ItemSource", typeof (IEnumerable), typeof (SBigFieldPropList), new PropertyMetadata(default(IEnumerable), (s, e) =>
                {
                    var sp = ((SBigFieldPropList) s);

                    sp.ItemSourceChanged(e.NewValue as IEnumerable);
                }));

        

        public IEnumerable ItemSource
        {
            get
            {
                return (IEnumerable) GetValue(ItemSourceProperty);
            }
            set
            {
                SetValue(ItemSourceProperty, value);
            }
        }
        private void BT_Edit_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private bool notifyChange = true;
        private void ItemSourceChanged(IEnumerable enumerable)
        {
            if (notifyChange) BindSource(enumerable);
            AddOrderPropCommand.InvalideCanExecute();
            AddCancelPropCommand.InvalideCanExecute();
            AddNamePropCommand.InvalideCanExecute();
        }

        private void BindSource(IEnumerable enumerable)
        {
            dataGrid.ItemsSource =enumerable;
        }

        BigFieldItem GetCurrentItem()
        {
            return dataGrid.SelectedItem as BigFieldItem;
        }

        private void BT_Delete_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteItem();
        }

        public override DataGrid GridView
        {
            get
            {
                return dataGrid;
            }
        }

        public override void AddItem()
        {
            //            notifyChange = false;
//            var list = GetDataGridItemList();
            var newItem = new BigFieldItem {
                ItemType = Property.字符串,
                ColumnSize = "50",
            };

            AddItem(newItem);
            //            BindSource(list);
//            ItemSource = list;
            //            notifyChange = true;
        }

        void AddItem(BigFieldItem p)
        {
            var list = dataGrid.ItemsSource as IList;
            list.Add(p);
            //            BindSource(list);
            ItemSource = list;
        }
        public override void DeleteItem()
        {
//            var list = GetDataGridItemList();
            if (dataGrid.SelectedItems.Count >= 0)
            {
                var p = dataGrid.SelectedItems[0];
                DeleteBeforeEventArgs eventArgs = new DeleteBeforeEventArgs(p);
                OnDeleteBefore(eventArgs);

                if (!eventArgs.AllowDelete.HasValue || eventArgs.AllowDelete.Value)
                {
                    if (eventArgs.AllowDelete.HasValue && eventArgs.AllowDelete.Value //如果已经确认过
                        || MainUtils.MessageBoxYesNo(this, "删除确认", "您确定要删除选定的属性吗？") == MessageBoxResult.Yes)
                    {
                        var list = ItemSource as IList;
                        //                    list.Remove(p);
                        dataGrid.SelectedItems.OfType<BigFieldItem>().ToList().ForEach(w => list.Remove(w));
                        //                    ItemSource = list;    
                    }
                }
            }
//            var list = GetDataGridItemList();
//            var properties = dataGrid.SelectedItems
//                    .OfType<BigFieldConfigItem>()
//                    .ToList()
//                    .Where(w => w != null)
//                    .ToList();
//            if (properties.Count >= 0)
//            {
//                var p = properties[0];
//                DeleteBeforeEventArgs eventArgs = new DeleteBeforeEventArgs(p);
//                OnDeleteBefore(eventArgs);
//                if (eventArgs.AllowDelete)
//                {
//                    properties.ForEach(w => list.Remove(w));
//                    ItemSource = list;
//                }
//            }
        }

        //下移
        private void BT_Down_OnClick(object sender, RoutedEventArgs e)
        {
            var item = GetCurrentItem();
            if (item != null)
            {
                var list = dataGrid.ItemsSource as IList;
                var index = list.IndexOf(item);
                if (index >= 0 && index < list.Count - 1)
                {
                    list.Remove(item);
                    list.Insert(index+1,item);
                    ItemSource = list;
                }
            }
        }
        //上移
        private void BT_Up_OnClick(object sender, RoutedEventArgs e)
        {
            var item = GetCurrentItem();
            if (item != null)
            {
                var list = dataGrid.ItemsSource as IList;
                var index = list.IndexOf(item);
                if (index >= 1 && index < list.Count)
                {
                    list.Remove(item);
                    list.Insert(index - 1, item);
                    ItemSource = list;
                }
            }
        }

        private void BT_Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddItem();
        }

//        private List<BigFieldConfigItem> GetDataGridItemList()
//        {
//            if (dataGrid.ItemsSource==null)return new List<BigFieldConfigItem>();
//            return dataGrid.ItemsSource.Cast<BigFieldConfigItem>().ToList();
//        }

        private void BT_Validate_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        /// <summary>
        /// 数据类型列表
        /// </summary>
        public List<string> DataTypes
        {
            get
            {
//                return Enum.GetValues(typeof (BigFieldItemType)).Cast<BigFieldItemType>().ToList();
                return Property.PropertyTypes.ToList().Where(w=>w!="大字段").ToList();
            }
        }
        /// <summary>
        /// 控件类型列表
        /// </summary>
        public List<string> ControlTypes
        {
            get
            {
                return new List<string> {
                    "文本框",
                    "数字框",
                    "单选框",
                    "多选框",
                    "下拉框",
                    "图片",
                };
            }
        }
        /// <summary>
        /// 组编号列表
        /// </summary>
        public List<string> GroupList
        {
            get
            {
                //                return Enum.GetValues(typeof (BigFieldItemType)).Cast<BigFieldItemType>().ToList();
                var groupList = Enumerable.Range(1, 10).Select(w => "Group" + w).ToList();
                groupList.Insert(0,"");
                return groupList;
//                return Property.PropertyTypes.ToList().Where(w=>w!="大字段").ToList();
            }
        }

        private void EditDetail_OnClick(object sender, RoutedEventArgs e)
        {
            var list = dataGrid.SelectedItem as BigFieldItem;
            if (list == null) return;
            EditDDLContent c = new EditDDLContent();
            var dropDownConfig = list.DDConfig;
            var str =  ExtensionUtil.Serialize(dropDownConfig.Items.Select(w => w.DisplayName).ToList(),"\r\n");
            c.Text = str;
            var showDialog = c.ShowDialog(this);
            if (showDialog.HasValue && showDialog.Value)
            {
                dropDownConfig.Items = c.Text.Split('\r', '\n').Where(w => !string.IsNullOrEmpty(w)).Select(w => new BigFieldItem.DropDownItem {DisplayName = w, Value = w}).ToList();
                list.DDConfig = dropDownConfig;
            }
        }
    }
}
