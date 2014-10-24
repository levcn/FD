using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using SLControls.Extends;
using SLFW.DB;
using StaffTrain.FwClass.Reflectors;
using STComponse.CFG;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;


namespace SLControls.Editors
{
    public class UserGridView : BaseMultiControl
    {
        public UserGridView()
        {
            this.DefaultStyleKey = typeof(UserGridView);
            ItemsPanel = new ItemsPanelTemplate();
            if (IsDesign()) return;

            Columns = new GridViewColumnCollection();
            InitCommand();
        }

        

        private RadGridView GridView;
        private LevcnPager Pager;
        private Button ItemEditButton;
        private Button DeleteButton;
        private Button SearchButton;
        private Panel SearchItemsPanel;
        private RadListBox ListBox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
//            if (IsDesign()) return;
//            return;
            GridView = GetTemplateChild("GridView") as RadGridView;
            Pager = GetTemplateChild("Pager") as LevcnPager;
            DeleteButton = GetTemplateChild("DeleteButton") as Button;
            ItemEditButton = GetTemplateChild("ItemEditButton") as Button;
            SearchButton = GetTemplateChild("SearchButton") as Button;
            ListBox = GetTemplateChild("ListBox") as RadListBox;
            SearchItemsPanel = GetTemplateChild("SearchItemsPanel") as Panel;
            
//            GridView.ItemsSource = new List<string> {"a", "b"};
            if (GridView != null)
            {
                GridView.BeginningEdit += GridView_BeginningEdit;
                GridView.RowLoaded += GridView_RowLoaded;
                //            GridView.RowResized += GridView_RowResized;
                GridView.ColumnWidthChanged += GridView_ColumnWidthChanged;
                GridView.MouseLeftButtonDown += GridView_MouseLeftButtonDown;
            }
            if (DeleteButton != null) DeleteButton.Click += DeleteButton_Click;
            if (ItemEditButton != null) ItemEditButton.Click += ItemEditButtonClick;
            if (SearchButton != null) SearchButton.Click += SearchButton_Click;
            if (Pager != null) Pager.PageChanged += Pager_PageChanged;
            ApplyColumns();
            ApplySearchPanel();
            SetGridViewType();
        }

        void Pager_PageChanged(object sender, LevcnPageChangedEventArgs e)
        {
            OnLoadSource(new UserGridViewSearchEventArgs { SearchEntities = GetUISearchEntity() });
        }

        public object SelectedItem
        {
            get
            {
                return GridView.SelectedItem;
            }
        }

        #region 命令
        private void InitCommand()
        {
//            this.EditCommand = new DelegateCommand(this.EditCommandExecuted, this.EditCommandCanExecute);
        }

//        private bool EditCommandCanExecute(object obj)
//        {
//
//        }
//
//        private void EditCommandExecuted(object obj)
//        {
//            
//        }
        public DelegateCommand EditCommand { get;private set; } 

        #endregion

        #region 事件


        /// <summary>
        /// 加载数据源
        /// </summary>
        public event TEventHandler<object, UserGridViewSearchEventArgs> LoadSource;

        protected virtual void OnLoadSource(UserGridViewSearchEventArgs args)
        {
            //tht修改,时间2014-21-03 09:51,原因:更新为新版数据查询
            if (ItemsSource!=null)
            {
                var type = ReflectionHelper.GetGenericType(ItemsSource.GetType());
                if (type != null)
                {
                    //todo 数据操作比较复杂有些参数需要设置,待定
                    base.OnDataOperator(new MultiControlDataEventArgs(type.Name,DataOperatorType.Select));
                }
            }
            //修改结束

            TEventHandler<object, UserGridViewSearchEventArgs> handler = LoadSource;
            if (handler != null) handler(this, args);
        }

        #endregion

        /// <summary>
        /// 设置列表数据和分页数据
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pi"></param>
        public void SetSource(object items, PageInfo pi)
        {
            ItemsSource = items;
            Pager.SetPageInfo(pi);
        }
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            OnLoadSource(new UserGridViewSearchEventArgs { SearchEntities = GetUISearchEntity() });
        }

        /// <summary>
        /// 返回页面中的搜索条件
        /// </summary>
        /// <returns></returns>
        List<SearchEntity> GetUISearchEntity()
        {
            List<SearchEntity> re = new List<SearchEntity>();
            if (ControlConfig == null || ControlConfig.SearchConfig == null || SearchItemsPanel==null) return re;
            var tName = ControlConfig.SearchConfig.ObjectName;
            var controls = SearchItemsPanel.Children.OfType<FrameworkElement>().ToList();
            ControlConfig.SearchConfig.Items.Where(w=>!w.IsCancel).ForEach(w =>
            {
                var controlName = prx + w.PropertyName;
                var c = controls.FirstOrDefault(z => z.Name == controlName);
                if (c != null)
                {
                    if (c is TextBox)
                    {
                        var tb = c as TextBox;
                        if (!string.IsNullOrEmpty(tb.Text))
                        {
                            SearchEntity se = new SearchEntity {
                                ColumnName = tName+"."+w.PropertyName,
                                Flag = "like",
                                value = tb.Text,
                            };
                            re.Add(se);
                        }
                    }
                }
            });
            return re;
        }
        /// <summary>
        /// 根据设置生成搜索区域
        /// </summary>
        private void ApplySearchPanel()
        {
            if (SearchVisibility == Visibility.Visible)
            {
                if (ControlConfig == null || ControlConfig.SearchConfig == null || ControlConfig.SearchConfig.Items == null || SearchItemsPanel == null) return;
                var items =  ControlConfig.SearchConfig.Items.Where(w=>!w.IsCancel).OrderBy(w => w.DisplayIndex).ToList();
                SearchItemsPanel.Children.Clear();
                items.ForEach(w =>
                {
                    AddSearchItem(w);
                });
            }
        }
        const string prx = "_SearchItem_";
        private void AddSearchItem(SearchItem searchItem)
        {
//            SearchItemsPanel;
            TextBlock tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Text = searchItem.DisplayName + "：";
            SearchItemsPanel.Children.Add(tb);
            
            if (searchItem.SearchType == 1) //文本
            {
                TextBox textBox = new TextBox();
                textBox.VerticalAlignment = VerticalAlignment.Center;
                textBox.Name = prx + searchItem.PropertyName;
                if (searchItem.Width.HasValue) textBox.Width = searchItem.Width.Value;
                SearchItemsPanel.Children.Add(textBox);
            }
            else if (searchItem.SearchType == 2) //下拉框
            {

            }
            else if (searchItem.SearchType == 3) //日期[带范围]
            {

            }
            else if (searchItem.SearchType == 4) //数字[带范围]
            {

            }
            
        }

        void ItemEditButtonClick(object sender, RoutedEventArgs e)
        {
//            return;
            if (GridView.SelectedItem != null)
            {
                ObjectEditorPanel oep = new ObjectEditorPanel();
                oep.EditObject = GridView.SelectedItem;
                oep.EditorConfig = ControlConfig.EditorConfig;
                oep.Width = 800;
                oep.Height = 500;
                oep.ShowDialog();
            }
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void GridView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
//            e.GetPosition()
        }

        void GridView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            
        }

        void GridView_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
//            var ttt = e.GridViewDataControl.;
        }

        void GridView_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!RowEditable)e.Cancel = true;
        }
        

        public static readonly DependencyProperty CurrentPageProperty =
                DependencyProperty.Register("CurrentPage", typeof (int), typeof (UserGridView), new PropertyMetadata(1));

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
        public static readonly DependencyProperty PageSizeProperty =
                DependencyProperty.Register("PageSize", typeof (int), typeof (UserGridView), new PropertyMetadata(20));

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

        public static readonly DependencyProperty ItemsPanelProperty =
                DependencyProperty.Register("ItemsPanel", typeof (ItemsPanelTemplate), typeof (UserGridView), new PropertyMetadata(default(ItemsPanelTemplate)));

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
        public static readonly DependencyProperty ItemTemplateProperty =
                DependencyProperty.Register("ItemTemplate", typeof (DataTemplate), typeof (UserGridView), new PropertyMetadata(default(DataTemplate)));

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
//        dep
        public static readonly DependencyProperty RowEditableProperty =
                DependencyProperty.Register("RowEditable", typeof (bool), typeof (UserGridView), new PropertyMetadata(default(bool)));

        public bool RowEditable
        {
            get
            {
                return (bool) GetValue(RowEditableProperty);
            }
            set
            {
                SetValue(RowEditableProperty, value);
            }
        }
        public void ApplyColumns()
        {
            if (GridView == null) return;
            GridView.Columns.Clear();
            GridView.Columns.AddRange(Columns);
        }

        public static readonly DependencyProperty ControlConfigProperty =
                DependencyProperty.Register("ControlConfig", typeof(UserGridViewConfig), typeof(UserGridView), new PropertyMetadata(default(UserGridViewConfig)));

        public UserGridViewConfig ControlConfig
        {
            get
            {
                return (UserGridViewConfig)GetValue(ControlConfigProperty);
            }
            set
            {
                SetValue(ControlConfigProperty, value);
            }
        }
//        public static readonly DependencyProperty EditorConfigProperty =
//                DependencyProperty.Register("EditorConfig", typeof (ObjectEditorConfig), typeof (UserGridView), new PropertyMetadata(default(ObjectEditorConfig)));
//
//        public ObjectEditorConfig EditorConfig
//        {
//            get
//            {
//                return (ObjectEditorConfig) GetValue(EditorConfigProperty);
//            }
//            set
//            {
//                SetValue(EditorConfigProperty, value);
//            }
//        }
        public static readonly DependencyProperty SearchVisibilityProperty =
                DependencyProperty.Register("SearchVisibility", typeof (Visibility), typeof (UserGridView), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// 搜索区域是否显示
        /// </summary>
        public Visibility SearchVisibility
        {
            get
            {
                return (Visibility) GetValue(SearchVisibilityProperty);
            }
            set
            {
                SetValue(SearchVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty PagerVisibilityProperty =
                DependencyProperty.Register("PagerVisibility", typeof (Visibility), typeof (UserGridView), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// 分页区域是否显示
        /// </summary>
        public Visibility PagerVisibility
        {
            get
            {
                return (Visibility) GetValue(PagerVisibilityProperty);
            }
            set
            {
                SetValue(PagerVisibilityProperty, value);
//                Grid c;
//                c.Children
                //                GridViewColumnCollection
            }
        }

        
        public static readonly DependencyProperty ColumnsProperty =
                DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(UserGridView), new PropertyMetadata(new GridViewColumnCollection()));

        public static readonly DependencyProperty GridViewTypeProperty =
                DependencyProperty.Register("GridViewType", typeof(GridViewType), typeof(UserGridView), new PropertyMetadata(default(GridViewType), (s, e) =>
                {
//                    var viewType = (GridViewType) e.NewValue;
                    var gridView = ((UserGridView)s);
                    gridView.SetGridViewType();
                    
                    
                }));

        private void SetGridViewType()
        {
            if (GridView == null || ListBox==null) return;
            if (GridViewType == GridViewType.GridView)
            {
                GridView.Visibility = Visibility.Visible;
                ListBox.Visibility = Visibility.Collapsed;

            }
            else
            {
                GridView.Visibility = Visibility.Collapsed;
                ListBox.Visibility = Visibility.Visible;
            }
        }

        public GridViewType GridViewType
        {
            get
            {
                return (GridViewType)GetValue(GridViewTypeProperty);
            }
            set
            {
                SetValue(GridViewTypeProperty, value);
            }
        }

//        public GridViewColumnCollection Columns
        public GridViewColumnCollection Columns
        {
            get
            {
                return (GridViewColumnCollection)GetValue(ColumnsProperty);
            }
            set
            {
                SetValue(ColumnsProperty, value);
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
                DependencyProperty.Register("ItemsSource", typeof (object), typeof (UserGridView), new PropertyMetadata(default(object)));

        public object ItemsSource
        {
            get
            {
                return (object) GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public static readonly DependencyProperty ToolbarVisibilityProperty =
                DependencyProperty.Register("ToolbarVisibility", typeof(Visibility), typeof(UserGridView), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// 工具栏显示
        /// </summary>
        public Visibility ToolbarVisibility
        {
            get
            {
                return (Visibility)GetValue(ToolbarVisibilityProperty);
            }
            set
            {
                SetValue(ToolbarVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        /// <param name="configStr"></param>
        public override void LoadConfig(string configStr)
        {
            if (GridView == null)
            {
                UpdateLayout();
            }
            var cfg = configStr.ToObject<UserGridViewConfig>();
            if (cfg == null) return;
            if (cfg.Width.HasValue) Width = cfg.Width.Value;
            if (cfg.Height.HasValue) Height = cfg.Height.Value;
            if (cfg.ShowPager.HasValue) PagerVisibility = cfg.ShowPager.Value ? Visibility.Visible : Visibility.Collapsed;
            if (cfg.ShowToolBar.HasValue) ToolbarVisibility = cfg.ShowToolBar.Value ? Visibility.Visible : Visibility.Collapsed;
            ControlConfig = cfg;
            if (cfg.Columns != null)
            {
                var cl = GridView.Columns.OfType<GridViewColumn>().ToList();
                cfg.Columns.ForEach(w =>
                {
                    var haveColumn = cl.FirstOrDefault(z => z.Header.ToString() == w.HeaderText);
                    if (haveColumn==null)
                    {
                        GridViewDataColumn gvc = new GridViewDataColumn();
                        gvc.Header = w.HeaderText;
                        gvc.DataMemberBinding = new Binding(w.DataMemberBindingPath);
                        if (w.Width.HasValue) gvc.Width = w.Width.Value;
                        if (w.DisplayIndex.HasValue) gvc.DisplayIndex = w.DisplayIndex.Value;
                        GridView.Columns.Add(gvc);
                    }
                    else
                    {
                        if (w.Width.HasValue) haveColumn.Width = w.Width.Value;
                        if (w.DisplayIndex.HasValue) haveColumn.DisplayIndex = w.DisplayIndex.Value;
                    }
                });
            }
            if (cfg.SearchConfig != null)
            {
                ApplySearchPanel();
            }
        }

//        public static readonly DependencyProperty TIntProperty =
//                DependencyProperty.Register("TInt", typeof(List<Color>), typeof(UserGridView), new PropertyMetadata(new List<Color>()));

//        private List<Color> tInt = new List<Color>();
//        public List<Color> TInt
//        {
//            get
//            {
//                return tInt;
//                //                return (List<Color>)GetValue(TIntProperty);
//            }
//            set
//            {
//                tInt = value;
//                //                SetValue(TIntProperty, value);
//            }
//        }
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <returns></returns>
        public string SaveConfig()
        {
            UserGridViewConfig cfg = ControlConfig??new UserGridViewConfig();
            cfg.ShowPager = PagerVisibility == Visibility.Visible;
            cfg.ShowToolBar = ToolbarVisibility == Visibility.Visible;
            if (ControlConfig!=null) cfg.EditorConfig = ControlConfig.EditorConfig;
            cfg.Columns = new List<ColumnConfig>();
            if (!double.IsNaN(Width)) cfg.Width = (int)Width;
            if (!double.IsNaN(Height)) cfg.Height = (int)Height;
//            cfg.SearchConfig = 
            if (GridView != null)
            {
                var columns = GridView.Columns.OfType<GridViewColumn>().ToList();
                columns.ForEach(w =>
                {
                    ColumnConfig cc = new ColumnConfig();
                    cc.HeaderText = w.Header.ToString();
                    cc.Width = (int) w.ActualWidth;
                    cc.DisplayIndex = w.DisplayIndex;
                    if (w is GridViewDataColumn)
                    {
                        var db = w as GridViewDataColumn;
                        if (db.DataMemberBinding != null)
                        {
                            cc.DataMemberBindingPath = db.DataMemberBinding.Path.Path;
                        }
                    }
                    cfg.Columns.Add(cc);
                });
            }
            return cfg.ToJson();
        }

//        /// <summary>
//        /// 根据行中的控件,返回当前行的数据对象
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="uiElement"></param>
//        /// <returns></returns>
//        public T GetItemByControl<T>(UIElement uiElement) where T : class
//        {
//            var cell = (uiElement as Button).ParentOfType<GridViewCell>();
//            var context = cell.ParentRow.DataContext;
//            var userInfo = context as T;
//            return userInfo;
//        }
    }

    public enum GridViewType
    {
        GridView,
        ItemView,
    }
}
