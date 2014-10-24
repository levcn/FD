using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FD.Core.SLControl.Data;
using SLComponse.Validate;
using SLControls.Controls;
using SLControls.DataClientTools;
using SLControls.Editors;
using SLControls.helpers;
using SLControls.PageConfigs;
using StaffTrain.FwClass.Reflectors;
using Telerik.Windows.Controls;


namespace SLControls.Editor1
{
    /// <summary>
    /// 列表控件
    /// </summary>
    [TemplatePart(Name = "PART_Pager", Type = typeof (LevcnPager))]
    public class ListPanel : BaseMultiControl
    {
        public static readonly DependencyProperty EditItemCommandProperty =
                DependencyProperty.Register("EditItemCommand", typeof (DelegateCommand), typeof (ListPanel), new PropertyMetadata(default(DelegateCommand), (s, e) => { ((ListPanel) s).OnAddCancelPropCommandChanged(e.NewValue as DelegateCommand); }));

        public static readonly DependencyProperty ItemsSourceProperty =
                DependencyProperty.Register("ItemsSource", typeof (IEnumerable), typeof (ListPanel), new PropertyMetadata(default(IEnumerable), (s, e) => { ((ListPanel) s).OnItemSourceChanged(e.NewValue as IEnumerable); }));

        public static readonly DependencyProperty IsBrowsModeProperty =
                DependencyProperty.Register("IsBrowsMode", typeof (bool), typeof (ListPanel), new PropertyMetadata(default(bool), (s, e) => { ((ListPanel) s).OnIsBrowsModeChanged(e.NewValue is bool ? (bool) e.NewValue : false); }));

        public static readonly DependencyProperty EditorTemplateProperty =
                DependencyProperty.Register("EditorTemplate", typeof (DataTemplate), typeof (ListPanel), new PropertyMetadata(default(DataTemplate), (s, e) => { ((ListPanel) s).OnEditorTemplateChanged(e.NewValue as DataTemplate); }));

        public static readonly DependencyProperty UseModalWindowProperty =
                DependencyProperty.Register("UseModalWindow", typeof (bool), typeof (ListPanel), new PropertyMetadata(default(bool), (s, e) => { ((ListPanel) s).OnUseModalWindowChanged(e.NewValue is bool ? (bool) e.NewValue : false); }));

        public static readonly DependencyProperty ItemTemplateProperty =
                DependencyProperty.Register("ItemTemplate", typeof (DataTemplate), typeof (ListPanel), new PropertyMetadata(default(DataTemplate), (s, e) => { ((ListPanel) s).OnItemTemplateChanged(e.NewValue as DataTemplate); }));

        public static readonly DependencyProperty ItemsPanelProperty =
                DependencyProperty.Register("ItemsPanel", typeof (ItemsPanelTemplate), typeof (ListPanel), new PropertyMetadata(default(ItemsPanelTemplate), (s, e) => { ((ListPanel) s).OnItemsPanelTemplateChanged(e.NewValue as ItemsPanelTemplate); }));

        public static readonly DependencyProperty WindowFollowItemProperty =
                DependencyProperty.Register("WindowFollowItem", typeof (bool), typeof (ListPanel), new PropertyMetadata(default(bool), (s, e) => { ((ListPanel) s).OnWindowFollowItemChanged(e.NewValue is bool ? (bool) e.NewValue : false); }));

        public static readonly DependencyProperty PageNumberCountProperty =
                DependencyProperty.Register("PageNumberCount", typeof (int), typeof (ListPanel), new PropertyMetadata(5, (s, e) => { ((ListPanel) s).OnPageNumberCountChanged(e.NewValue is int ? (int) e.NewValue : 0); }));

        public static readonly DependencyProperty RecordCountProperty =
                DependencyProperty.Register("RecordCount", typeof (int), typeof (ListPanel), new PropertyMetadata(default(int), (s, e) => { ((ListPanel) s).OnRecordCountChanged(e.NewValue is int ? (int) e.NewValue : 0); }));

        public static readonly DependencyProperty PageCountProperty =
                DependencyProperty.Register("PageCount", typeof (int), typeof (ListPanel), new PropertyMetadata(default(int), (s, e) => { ((ListPanel) s).OnPageCountChanged(e.NewValue is int ? (int) e.NewValue : 0); }));

        public static readonly DependencyProperty PageSizeProperty =
                DependencyProperty.Register("PageSize", typeof (int), typeof (ListPanel), new PropertyMetadata(20, (s, e) => { ((ListPanel) s).OnPageSizeChanged(e.NewValue is int ? (int) e.NewValue : 20); }));

        public static readonly DependencyProperty CurrentPageProperty =
                DependencyProperty.Register("CurrentPage", typeof (int), typeof (ListPanel), new PropertyMetadata(1, (s, e) => { ((ListPanel) s).OnCurrentPageChanged(e.NewValue is int ? (int) e.NewValue : 0); }));

        private LevcnPager PART_Pager;
        private EItemsControl eItemsControl;

        public ListPanel()
        {
            DefaultStyleKey = typeof (ListPanel);
        }
        [Editable(GroupName = "基本属性", DisplayName = "分页按钮数量", Description = "分页按钮数量。")]
        public int PageNumberCount
        {
            get
            {
                return (int) GetValue(PageNumberCountProperty);
            }
            set
            {
                SetValue(PageNumberCountProperty, value);
            }
        }

        public int RecordCount
        {
            get
            {
                return (int) GetValue(RecordCountProperty);
            }
            protected set
            {
                SetValue(RecordCountProperty, value);
            }
        }

        public int PageCount
        {
            get
            {
                return (int) GetValue(PageCountProperty);
            }
            private set
            {
                SetValue(PageCountProperty, value);
            }
        }
        [Editable(GroupName = "基本属性", DisplayName = "每页记录数", Description = "每页记录数。")]
        public int PageSize
        {
            get
            {
                return (int) GetValue(PageSizeProperty);
            }
            set
            {
                SetValue(PageSizeProperty, value);
            }
        }

        public static readonly DependencyProperty DeleteCommandProperty =
                DependencyProperty.Register("DeleteCommand", typeof (DelegateCommand), typeof (ListPanel), new PropertyMetadata(default(DelegateCommand), (s, e) =>
                {
                    ((ListPanel) s).OnDeleteCommandChanged(e.NewValue as DelegateCommand);
                }));

        private void OnDeleteCommandChanged(DelegateCommand list)
        {}

        public DelegateCommand DeleteCommand
        {
            get
            {
                return (DelegateCommand) GetValue(DeleteCommandProperty);
            }
            set
            {
                SetValue(DeleteCommandProperty, value);
            }
        }
        public static readonly DependencyProperty AddCommandProperty =
                DependencyProperty.Register("AddCommand", typeof (DelegateCommand), typeof (ListPanel), new PropertyMetadata(default(DelegateCommand), (s, e) =>
                {
                    ((ListPanel) s).OnAddCommandChanged(e.NewValue as DelegateCommand);
                }));

        private void OnAddCommandChanged(DelegateCommand list)
        {}

        public DelegateCommand AddCommand
        {
            get
            {
                return (DelegateCommand) GetValue(AddCommandProperty);
            }
            set
            {
                SetValue(AddCommandProperty, value);
            }
        }
        public DelegateCommand EditItemCommand
        {
            get
            {
                return (DelegateCommand) GetValue(EditItemCommandProperty);
            }
            set
            {
                SetValue(EditItemCommandProperty, value);
            }
        }

        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable) GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        [Editable(GroupName = "基本属性", DisplayName = "是否启用浏览模式", Description = "是否启用浏览模式。")]
        public bool IsBrowsMode
        {
            get
            {
                return (bool) GetValue(IsBrowsModeProperty);
            }
            set
            {
                SetValue(IsBrowsModeProperty, value);
            }
        }

        public static readonly DependencyProperty BrowserTemplateProperty =
                DependencyProperty.Register("BrowserTemplate", typeof (DataTemplate), typeof (ListPanel), new PropertyMetadata(default(DataTemplate), (s, e) =>
                {
                    ((ListPanel) s).OnBrowsTemplateChanged(e.NewValue as DataTemplate);
                }));

        private void OnBrowsTemplateChanged(DataTemplate list)
        {}

        public DataTemplate BrowserTemplate
        {
            get
            {
                return (DataTemplate) GetValue(BrowserTemplateProperty);
            }
            set
            {
                SetValue(BrowserTemplateProperty, value);
            }
        }
        public DataTemplate EditorTemplate
        {
            get
            {
                return (DataTemplate) GetValue(EditorTemplateProperty);
            }
            set
            {
                SetValue(EditorTemplateProperty, value);
            }
        }

        [Editable(GroupName = "基本属性", DisplayName = "是否启用模态窗口编辑", Description = "是否启用模态窗口编辑。")]
        public bool UseModalWindow
        {
            get
            {
                return (bool) GetValue(UseModalWindowProperty);
            }
            set
            {
                SetValue(UseModalWindowProperty, value);
            }
        }

        public static readonly DependencyProperty ControlPanelTemplateProperty =
                DependencyProperty.Register("ControlPanelTemplate", typeof (DataTemplate), typeof (ListPanel), new PropertyMetadata(default(DataTemplate), (s, e) =>
                {
                    ((ListPanel) s).OnControlPanelTemplateChanged(e.NewValue as DataTemplate);
                }));

        private void OnControlPanelTemplateChanged(DataTemplate list)
        {}

        public static readonly DependencyProperty DelButtonProperty =
                DependencyProperty.Register("DelButton", typeof (Button), typeof (ListPanel), new PropertyMetadata(default(Button), (s, e) =>
                {
                    ((ListPanel) s).OnDelButtonChanged(e.NewValue as Button);
                }));

        private void OnDelButtonChanged(Button list)
        {}

        public Button DelButton
        {
            get
            {
                return (Button) GetValue(DelButtonProperty);
            }
            set
            {
                SetValue(DelButtonProperty, value);
            }
        }
        public static readonly DependencyProperty EditButtonProperty =
                DependencyProperty.Register("EditButton", typeof (Button), typeof (ListPanel), new PropertyMetadata(default(Button), (s, e) =>
                {
                    ((ListPanel) s).OnEditButtonChanged(e.NewValue as Button);
                }));

        private void OnEditButtonChanged(Button list)
        {}

        public Button EditButton
        {
            get
            {
                return (Button) GetValue(EditButtonProperty);
            }
            set
            {
                SetValue(EditButtonProperty, value);
            }
        }
        public static readonly DependencyProperty AddButtonTemplateProperty =
                DependencyProperty.Register("AddButtonTemplate", typeof(DataTemplate), typeof(ListPanel), new PropertyMetadata(default(DataTemplate), (s, e) =>
                {
                    ((ListPanel)s).OnAddButtonChanged(e.NewValue as DataTemplate);
                }));

        private void OnAddButtonChanged(DataTemplate list)
        {}

        public static readonly DependencyProperty DeleteButtonTemplateProperty =
                DependencyProperty.Register("DeleteButtonTemplate", typeof (DataTemplate), typeof (ListPanel), new PropertyMetadata(default(DataTemplate), (s, e) =>
                {
                    ((ListPanel) s).OnDeleteButtonTemplateChanged(e.NewValue as DataTemplate);
                }));

        private void OnDeleteButtonTemplateChanged(DataTemplate list)
        {}

        public DataTemplate DeleteButtonTemplate
        {
            get
            {
                return (DataTemplate) GetValue(DeleteButtonTemplateProperty);
            }
            set
            {
                SetValue(DeleteButtonTemplateProperty, value);
            }
        }
        public static readonly DependencyProperty EditButtonTemplateProperty =
                DependencyProperty.Register("EditButtonTemplate", typeof (DataTemplate), typeof (ListPanel), new PropertyMetadata(default(DataTemplate), (s, e) =>
                {
                    ((ListPanel) s).OnEditButtonTemplateChanged(e.NewValue as DataTemplate);
                }));

        private void OnEditButtonTemplateChanged(DataTemplate list)
        {}

        public DataTemplate EditButtonTemplate
        {
            get
            {
                return (DataTemplate) GetValue(EditButtonTemplateProperty);
            }
            set
            {
                SetValue(EditButtonTemplateProperty, value);
            }
        }
        public DataTemplate AddButtonTemplate
        {
            get
            {
                return (DataTemplate)GetValue(AddButtonTemplateProperty);
            }
            set
            {
                SetValue(AddButtonTemplateProperty, value);
            }
        }
        public DataTemplate ControlPanelTemplate
        {
            get
            {
                return (DataTemplate) GetValue(ControlPanelTemplateProperty);
            }
            set
            {
                SetValue(ControlPanelTemplateProperty, value);
            }
        }
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate) GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

        public ItemsPanelTemplate ItemsPanel
        {
            get
            {
                return (ItemsPanelTemplate) GetValue(ItemsPanelProperty);
            }
            set
            {
                SetValue(ItemsPanelProperty, value);
            }
        }

        [Editable(GroupName = "基本属性", DisplayName = "是否启用窗口跟随", Description = "是否启用窗口跟随。")]
        public bool WindowFollowItem
        {
            get
            {
                return (bool) GetValue(WindowFollowItemProperty);
            }
            set
            {
                SetValue(WindowFollowItemProperty, value);
            }
        }

        public int CurrentPage
        {
            get
            {
                return (int) GetValue(CurrentPageProperty);
            }
            set
            {
                SetValue(CurrentPageProperty, value);
            }
        }

        private void OnPageNumberCountChanged(int list)
        {}

        private void OnRecordCountChanged(int list)
        {}

        private void OnPageCountChanged(int list)
        {}

        private void OnPageSizeChanged(int list)
        {}

        protected override void OnEditEnd(MouseButtonEventArgs e)
        {
            base.OnEditEnd(e);
            refreshValid();
        }

        /// <summary>
        ///     把验证设置刷入实体类,使验证生效
        /// </summary>
        private void refreshValid()
        {
            if (ItemsSource == null) return;
            foreach (object v in ItemsSource)
            {
                var item = v as BaseListEntity;
                if (item != null)
                {
                    item.ValidataConfig = new ValidataConfig {ColumnValidata = ControlConfig.ValidateConfigs};
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            eItemsControl = GetTemplateChild("PART_List") as EItemsControl;
            PART_Pager = GetTemplateChild("PART_Pager") as LevcnPager;
            if (eItemsControl != null)
            {
                EditItemCommand = new DelegateCommand(o => eItemsControl.EditCommand.Execute(o), o => eItemsControl.EditCommand.CanExecute(o));
                DeleteItemCommand = new DelegateCommand(o => DeleteItem(o), o => DeleteItemCanExecute(o));
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
            }
            AddCommand = new DelegateCommand(o => AddCommandExecute(o), o => true);
            DeleteCommand = new DelegateCommand(o => DeleteCommandExecute(o), o => DeleteCommandCanExecute(o));
            if (PART_Pager != null) PART_Pager.PageChanged += PART_Pager_PageChanged;
        }

        private bool DeleteItemCanExecute(object o)
        { 
            return true;
        }

        private void DeleteItem(object o)
        {
            AlertHelper.Confirm(this,"您确定要删除吗？", async () =>
            {
                await _DeleteItem(new List<object> { o });
            });
        }

        private async Task<object> _DeleteItem(List<object> o)
        {
            if (o == null || o.Count == 0) return null;
            IsBusy = true;
            
            var list = o.Select(w => ReflectionHelper.GetPropertyValue("ID", w).ToString()).ToList();
            await ActionHelper.Delete(o[0].GetType(), list);
            var re= await RefreshDataSource();
            IsBusy = false;
            return re;
        }

        public static readonly DependencyProperty DeleteItemCommandProperty =
                DependencyProperty.Register("DeleteItemCommand", typeof (DelegateCommand), typeof (ListPanel), new PropertyMetadata(default(DelegateCommand), (s, e) =>
                {
                    ((ListPanel) s).OnDeleteItemCommandChanged(e.NewValue as DelegateCommand);
                }));

        private void OnDeleteItemCommandChanged(DelegateCommand list)
        {}

        public DelegateCommand DeleteItemCommand
        {
            get
            {
                return (DelegateCommand) GetValue(DeleteItemCommandProperty);
            }
            set
            {
                SetValue(DeleteItemCommandProperty, value);
            }
        }
        public static readonly DependencyProperty DeleteButtonVisibilityProperty =
                DependencyProperty.Register("DeleteButtonVisibility", typeof(Visibility), typeof(ListPanel), new PropertyMetadata(Visibility.Visible, (s, e) =>
                {
                    ((ListPanel)s).OnDeleteButtonVisibilityChanged(e.NewValue is Visibility ? (Visibility) e.NewValue : Visibility.Visible);
                }));

        private void OnDeleteButtonVisibilityChanged(Visibility list)
        {}

        [Editable(GroupName = "基本属性", DisplayName = "删除按钮可见", Description = "删除按钮可见。")]
        public Visibility DeleteButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(DeleteButtonVisibilityProperty);
            }
            set
            {
                SetValue(DeleteButtonVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty EditButtonVisibilityProperty =
                DependencyProperty.Register("EditButtonVisibility", typeof(Visibility), typeof(ListPanel), new PropertyMetadata(Visibility.Visible, (s, e) =>
                {
                    ((ListPanel)s).OnEditButtonVisibilityChanged(e.NewValue is Visibility ? (Visibility) e.NewValue : Visibility.Visible);
                }));

        private void OnEditButtonVisibilityChanged(Visibility list)
        {}

        [Editable(GroupName = "基本属性", DisplayName = "编辑按钮可见", Description = "编辑按钮可见。")]
        public Visibility EditButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(EditButtonVisibilityProperty);
            }
            set
            {
                SetValue(EditButtonVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty AddButtonVisibilityProperty =
                DependencyProperty.Register("AddButtonVisibility", typeof(Visibility), typeof(ListPanel), new PropertyMetadata(Visibility.Visible, (s, e) =>
                {
                    ((ListPanel) s).OnAddButtonVisibilityChanged(e.NewValue is Visibility ? (Visibility) e.NewValue : Visibility.Visible);
                }));

        private void OnAddButtonVisibilityChanged(Visibility list)
        {}

        [Editable(GroupName = "基本属性", DisplayName = "添加按钮可见", Description = "添加按钮可见。")]
        public Visibility AddButtonVisibility
        {
            get
            {
                return (Visibility) GetValue(AddButtonVisibilityProperty);
            }
            set
            {
                SetValue(AddButtonVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty OrderByProperty =
                DependencyProperty.Register("OrderBy", typeof (string), typeof (ListPanel), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((ListPanel) s).OnOrderByChanged(e.NewValue as string);
                }));

        private void OnOrderByChanged(string list)
        {}

        [Editable(GroupName = "基本属性", DisplayName = "排序字段", Description = "排序字段。")]
        public string OrderBy
        {
            get
            {
                return (string) GetValue(OrderByProperty);
            }
            set
            {
                SetValue(OrderByProperty, value);
            }
        }

        private void DeleteCommandExecute(object o)
        {
            var list = ItemsSource.Cast<BaseListEntity>().ToList().Where(w => w.IsSelected).ToList();
            if (list.Count > 0)
            {
                AlertHelper.Confirm(this,"您确定要删除吗？", async () =>
                {
                    await _DeleteItem(list.Cast<object>().ToList());
                });
            }
        }

        /// <summary>
        /// 验证删除功能是否可以使用
        /// </summary>
        public void InvalidateDeleteEnabled()
        {
            DeleteCommand.InvalidateCanExecute();
        }
        /// <summary>
        /// 验证删除是否可以使用
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool DeleteCommandCanExecute(object o)
        {
            return ItemsSource != null && ItemsSource.Cast<BaseListEntity>().ToList().Any(w => w.IsSelected);
        }

        private void AddCommandExecute(object o)
        {
            var b = GetEntityType();
            var obj = ReflectionHelper.GetObject(b) as BaseListEntity;
            obj.ValidataConfig = new ValidataConfig {ColumnValidata = ControlConfig.ValidateConfigs};
            OnAddBefore(obj);
            eItemsControl.EditItem(obj, new Point(0, 0), obj1 =>
            {
                OnInsertBefore(obj1);
                Insert(obj1);
            });
        }

        public event TEventHandler<object, object> AddBefore;
        public event TEventHandler<object, object> InsertBefore;

        protected virtual void OnInsertBefore(object args)
        {
            var handler = InsertBefore;
            if (handler != null) handler(this, args);
        }

        protected virtual void OnAddBefore(object args)
        {
            var handler = AddBefore;
            if (handler != null) handler(this, args);
        }

        private void OnCurrentPageChanged(int list)
        {}

        private void PART_Pager_PageChanged(object sender, LevcnPageChangedEventArgs e)
        {
            RefreshDataSource();
        }

        private void OnAddCancelPropCommandChanged(DelegateCommand list)
        {}

        private void OnItemSourceChanged(IEnumerable list)
        {
            refreshSelectEvent();
            refreshValid();
        }

        private void refreshSelectEvent()
        {
            foreach (var v in ItemsSource)
            {
                if (v is BaseListEntity)
                {
                    var entity = (v as BaseListEntity);
//                    entity.PropertyChanged += entity_PropertyChanged;
//                    entity.IsSelected
                }
            }
        }

        void entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
//            if (e.PropertyName == "IsSelected")
//            {
//                var entity = sender as BaseListEntity;
//            }
        }

        private void OnIsBrowsModeChanged(bool list)
        {}

        private void OnEditorTemplateChanged(DataTemplate list)
        {}

        private void OnUseModalWindowChanged(bool list)
        {}

        private void OnItemTemplateChanged(DataTemplate list)
        {}

        private void OnItemsPanelTemplateChanged(ItemsPanelTemplate list)
        {}

        private void OnWindowFollowItemChanged(bool list)
        {}

        public override void LoadConfig(string configStr)
        {}

        protected override void OnControlConfigChanged(ControlConfig cc)
        {
            refreshValid();
            BindDataSource();
        }

        private void BindDataSource()
        {
            if (!string.IsNullOrEmpty(ControlConfig.DataConfig.PageObject))
            {
                SetBinding(ItemsSourceProperty, new Binding {Source = BasePage, Mode = BindingMode.TwoWay, Path = new PropertyPath(ControlConfig.DataConfig.PageObject)});
                RefreshDataSource();
            }
        }

        private async void Insert(object o)
        {
            IsBusy = true;
            var re = await ActionHelper.Insert(this.GetEntityType(), o, false);
            IsBusy = false;
            await RefreshDataSource();
            
        }
        private async Task<object> RefreshDataSource()
        {
            var b = GetEntityType();
            if (b == null) return null;
            IsBusy = true;
            object re = await ActionHelper.Select(b, null, OrderBy, CurrentPage, PageSize, null, null, null); //as PagerListResult<SYS_User>;
            IsBusy = false;
            var itemList = ReflectionHelper.GetPropertyValue("ListData", re) as IEnumerable;
            var pageInfo = ReflectionHelper.GetPropertyValue("PageInfo", re) as PageInfo;
            //            var listDataProp = p.GetProperty("ListData");
            //            var pageInfoProp = p.GetProperty("PageInfo");
            //            var itemList = listDataProp.GetValue(re, null) as IEnumerable;
            if (string.IsNullOrEmpty(ControlConfig.DataConfig.PageObject))
            {
                AlertHelper.Alert(this, "未设置对象保存到页面的哪个对象");
                //                throw new Exception("未设置对象保存到页面的哪个对象.");
                return null;
            }
            ReflectionHelper.SetPropertyValue(ControlConfig.DataConfig.PageObject, BasePage, itemList);
            PART_Pager.SetPageInfo(pageInfo);
            return null;
        }

        private Type GetEntityType()
        {
            Type b = EntitTypes.FirstOrDefault(w => w.Name == ControlConfig.DataConfig.ObjectName);
            return b;
        }
    }
}