using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SLControls.Extends;


namespace SLControls.Editors
{
    public class LevcnPager : Control
    {
        public LevcnPager()
        {
            this.DefaultStyleKey = typeof(LevcnPager);
            Loaded += LevcnPager_Loaded;
        }
        private Button FirstPageButton;
        private Button PrePageButton;
        private Button NextPageButton;
        private Button LastPageButton;
        private Button GoButton;
        private TextBlock PageTextControl;
        private ComboBox pageSizeCB;
        private NumberBox UserInputPageNumber;
        private Panel NumberButton;
        private Panel BaseGrid;
        private Panel PageSizePanel;
        private Panel GoControlPanel;



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            pageSizeCB = (ComboBox)GetTemplateChild("pageSizeCB");
            FirstPageButton = (Button)GetTemplateChild("FirstPageButton");
            PrePageButton = (Button)GetTemplateChild("PrePageButton");
            NextPageButton = (Button)GetTemplateChild("NextPageButton");
            LastPageButton = (Button)GetTemplateChild("LastPageButton");
            GoButton = (Button)GetTemplateChild("GoButton");
            PageTextControl = (TextBlock)GetTemplateChild("PageTextControl");
            UserInputPageNumber = (NumberBox)GetTemplateChild("UserInputPageNumber");
            NumberButton = (Panel)GetTemplateChild("NumberButton");
            BaseGrid = (Panel)GetTemplateChild("BaseGrid");
            GoControlPanel = (Panel)GetTemplateChild("GoControlPanel");
            PageSizePanel = (Panel)GetTemplateChild("PageSizePanel");

            //            if (PageSizePanel != null)PageSizePanel.Visibility = PageSizeVisibility;
            //            if (GoControlPanel != null) GoControlPanel.Visibility = GoControlVisibility;
            if (pageSizeCB != null)
            {
                pageSizeCB.SelectionChanged += PageSizeCB_OnSelectionChanged;
            }
            InitControlEvents();
            InitData();
            SetPageInfo();

        }

        public static readonly DependencyProperty CustomPageSizeIsEnabledProperty =
                DependencyProperty.Register("CustomPageSizeIsEnabled", typeof(bool), typeof(LevcnPager), new PropertyMetadata(true));

        public bool CustomPageSizeIsEnabled
        {
            get
            {
                return (bool)GetValue(CustomPageSizeIsEnabledProperty);
            }
            set
            {
                SetValue(CustomPageSizeIsEnabledProperty, value);
            }
        }
//        private int currentPage = 1;
        private bool pageEvent = true;
//        private int pageNumberCount = 5;
//        private int pageSize;

        private string pageText = "共有 {0} 条记录，当前第 {1}/{2} 页";
        private int recordCount;
        private bool showFirstLastPage = true;
        private bool showPreNextPage = true;
        private int totalPage;

        //        public LevcnPager()
        //        {
        ////            InitializeComponent();
        //            InitControlEvents();
        //            
        //        }

        /// <summary>
        ///     分页信息显示数据 "
        ///     <remarks>共有 {0} 条记录，当前第 {1}/{2} 页</remarks>
        ///     "
        /// </summary>
        public string PageText
        {
            get
            {
                return pageText;
            }
            set
            {
                pageText = value;
            }
        }

        public static readonly DependencyProperty PageSizeVisibilityProperty =
                DependencyProperty.Register("PageSizeVisibility", typeof(Visibility), typeof(LevcnPager), new PropertyMetadata(Visibility.Visible));

        public Visibility PageSizeVisibility
        {
            get
            {
                return (Visibility)GetValue(PageSizeVisibilityProperty);
            }
            set
            {
                SetValue(PageSizeVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty GoControlVisibilityProperty =
                DependencyProperty.Register("GoControlVisibility", typeof(Visibility), typeof(LevcnPager), new PropertyMetadata(default(Visibility)));

        public Visibility GoControlVisibility
        {
            get
            {
                return (Visibility)GetValue(GoControlVisibilityProperty);
            }
            set
            {
                SetValue(GoControlVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty NumberButtonsVisibilityProperty =
                DependencyProperty.Register("NumberButtonsVisibility", typeof(Visibility), typeof(LevcnPager), new PropertyMetadata(default(Visibility)));

        public Visibility NumberButtonsVisibility
        {
            get
            {
                return (Visibility)GetValue(NumberButtonsVisibilityProperty);
            }
            set
            {
                SetValue(NumberButtonsVisibilityProperty, value);
            }
        }
//        /// <summary>
//        ///     当前页码
//        /// </summary>
//        public int CurrentPage
//        {
//            get
//            {
//                return currentPage;
//            }
//            set
//            {
//                currentPage = value;
//            }
//        }
        public static readonly DependencyProperty CurrentPageProperty =
                DependencyProperty.Register("CurrentPage", typeof (int), typeof (LevcnPager), new PropertyMetadata(1));

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
        /// <summary>
        ///     总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (totalPage < 1) totalPage = 1;
                return totalPage;
            }
            set
            {
                if (totalPage < 1) totalPage = 1;
                totalPage = value;
            }
        }

        /// <summary>
        ///     总记录数
        /// </summary>
        public int RecordCount
        {
            get
            {
                return recordCount;
            }
            set
            {
                recordCount = value;
            }
        }

        public static readonly DependencyProperty PageNumberCountProperty =
                DependencyProperty.Register("PageNumberCount", typeof(int), typeof(LevcnPager), new PropertyMetadata(5, (s, e) =>
                {
                    ((LevcnPager)s).OnPageNumberCountChanged(e.NewValue is int ? (int) e.NewValue : 0);
                }));

        private void OnPageNumberCountChanged(int list)
        {}

        public int PageNumberCount
        {
            get
            {
                return (int)GetValue(PageNumberCountProperty);
            }
            set
            {
                SetValue(PageNumberCountProperty, value);
            }
        }
//        /// <summary>
//        ///     显示页码按钮的数量
//        /// </summary>
//        public int PageNumberCount
//        {
//            get
//            {
//                return pageNumberCount;
//            }
//            set
//            {
//                pageNumberCount = value;
//            }
//        }

        public static readonly DependencyProperty FirstLastButtonVisibilityProperty =
                DependencyProperty.Register("FirstLastButtonVisibility", typeof(Visibility), typeof(LevcnPager), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// 显示首页尾页按钮
        /// </summary>
        public Visibility FirstLastButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(FirstLastButtonVisibilityProperty);
            }
            set
            {
                SetValue(FirstLastButtonVisibilityProperty, value);
            }
        }
        //  
        //        public bool ShowFirstLastPage
        //        {
        //            get
        //            {
        //                return showFirstLastPage;
        //            }
        //            set
        //            {
        //                showFirstLastPage = value;
        //            }
        //        }
        public static readonly DependencyProperty PreNextButtonVisibilityProperty =
                DependencyProperty.Register("PreNextButtonVisibility", typeof(Visibility), typeof(LevcnPager), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// 上一页下一页按钮
        /// </summary>
        public Visibility PreNextButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(PreNextButtonVisibilityProperty);
            }
            set
            {
                SetValue(PreNextButtonVisibilityProperty, value);
            }
        }
        //
        //        public bool ShowPreNextPage
        //        {
        //            get
        //            {
        //                return showPreNextPage;
        //            }
        //            set
        //            {
        //                showPreNextPage = value;
        //            }
        //        }
        public static readonly DependencyProperty PageSizeProperty =
                DependencyProperty.Register("PageSize", typeof (int), typeof (LevcnPager), new PropertyMetadata(20));

        public int PageSize
        {
            get
            {
                return (int) GetValue(PageSizeProperty);
            }
            set
            {
                SetValue(PageSizeProperty, value);
                InitPageSizeSelectedIndex();
            }
        }
//        public int PageSize
//        {
//            get
//            {
//                return pageSize;
//            }
//            set
//            {
//                pageSize = value;
//                InitPageSizeSelectedIndex();
//            }
//        }

        public class PageSizeItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        public static readonly DependencyProperty PageSizeListShowAllProperty =
                DependencyProperty.Register("PageSizeListShowAll", typeof(bool), typeof(LevcnPager), new PropertyMetadata(default(bool)));

        public bool PageSizeListShowAll
        {
            get
            {
                return (bool)GetValue(PageSizeListShowAllProperty);
            }
            set
            {
                SetValue(PageSizeListShowAllProperty, value);
            }
        }
        private void LevcnPager_Loaded(object sender, RoutedEventArgs e)
        {
            //            List<int> list = (new[] {20, 50, 100, 200}).ToList();
            //            InitData();

            //            list.ForEach(
            //                w =>
            //                    {
            //                        var l = new ComboBoxItem {Content = w.ToString()};
            //                        if (w == PageSize) l.IsSelected = true;
            //                        pageSizeCB.Items.Add(l);
            //                        pageSizeCB.sele
            //                    });
        }

        private void InitData()
        {
            var list = new List<PageSizeItem> {
                new PageSizeItem {Text = "20", Value = 20},
                new PageSizeItem {Text = "50", Value = 50},
                new PageSizeItem {Text = "100", Value = 100},
                new PageSizeItem {Text = "200", Value = 200},
            };
            if (PageSizeListShowAll)
            {
                list.Add(new PageSizeItem { Text = "所有", Value = 200000 });
            }
            pageSizeCB.ItemsSource = list;
            pageSizeCB.DisplayMemberPath = "Text";
            pageSizeCB.SelectedValuePath = "Value";
            //            pageSizeCB.SelectedItem = PageSize;
            pageEvent = false;
            pageSizeCB.SelectedValue = PageSize;
            pageEvent = true;
        }

        private void InitPageSizeSelectedIndex()
        {
            pageEvent = false;
            //            pageSizeCB.Items.Cast<ComboBoxItem>().ToList().ForEach(w=>w.IsSelected=false);
            //            int i = 0;
            //            int index = 0;
            //            pageSizeCB.Items.Cast<ComboBoxItem>().ToList()
            //                .ForEach(w =>
            //                             {
            //                                 if (Convert.ToInt32(w.Content) == PageSize)
            //                                 {
            //                                     index = i;
            //                                 }
            //                                 i++;
            //                             });
            if (pageSizeCB != null)
            {
                pageSizeCB.SelectedItem = PageSize;
                pageSizeCB.SelectedValue = PageSize;
            }
            //            pageSizeCB.SelectedIndex = index;
            pageEvent = true;
        }



        private void InitControlEvents()
        {
            FirstPageButton.Click += (s, e) => GotoNewPage(1);
            PrePageButton.Click += (s, e) => GotoNewPage(CurrentPage - 1);
            NextPageButton.Click += (s, e) => GotoNewPage(CurrentPage + 1);
            LastPageButton.Click += (s, e) => GotoNewPage(TotalPage);
            GoButton.Click += (s, e) => GotoInputPage();
            Loaded += (s, e) => { PageTextControl.Text = ""; };
        }

        /// <summary>
        ///     转到用户指定的页面
        /// </summary>
        private void GotoInputPage()
        {
            int p = Convert.ToInt32(UserInputPageNumber.Text);
            GotoNewPage(p);
        }

        /// <summary>
        ///     重新绑定分页数据
        /// </summary>
        public void DataBind()
        {
            //            Button b = new Button();
            if (PageTextControl == null)
            {
                //                Button
                return;
            }
            PageTextControl.Text = string.Format(PageText, RecordCount, CurrentPage, TotalPage);
            BindPageButton();
            UserInputPageNumber.Text = CurrentPage.ToString();
            FirstPageButton.IsEnabled = CurrentPage != 1;
            PrePageButton.IsEnabled = CurrentPage != 1;
            LastPageButton.IsEnabled = CurrentPage != TotalPage;
            NextPageButton.IsEnabled = CurrentPage != TotalPage;

            //            if (ShowFirstLastPage)
            //            {
            //                FirstPageButton.Visibility = Visibility;
            //                LastPageButton.Visibility = Visibility.Visible;
            //            }
            //            else
            //            {
            //                FirstPageButton.Visibility = Visibility.Collapsed;
            //                LastPageButton.Visibility = Visibility.Collapsed;
            //            }
            //            if (ShowPreNextPage)
            //            {
            //                PrePageButton.Visibility = Visibility.Visible;
            //                NextPageButton.Visibility = Visibility.Visible;
            //            }
            //            else
            //            {
            //                PrePageButton.Visibility = Visibility.Collapsed;
            //                NextPageButton.Visibility = Visibility.Collapsed;
            //            }
        }

        /// <summary>
        ///     绑定页码按钮
        /// </summary>
        private void BindPageButton()
        {
            NumberButton.Children.Clear();
            int startPage = 0;
            int cP = CurrentPage;
            int countPage = TotalPage;
            startPage = cP - (PageNumberCount - 1) / 2;

            if (startPage < 1) startPage = 1;
            int endPage = startPage + PageNumberCount - 1;
            if (endPage > countPage)
            {
                endPage = countPage;
                startPage = endPage - PageNumberCount + 1;
                if (startPage < 1) startPage = 1;
            }
            for (int i = startPage; i <= endPage; i++)
            {
                AddPageButton(NumberButton, i);
            }
        }

        /// <summary>
        ///     添加一个指定页的分页按钮到容器里
        /// </summary>
        /// <param name="p"></param>
        /// <param name="i"></param>
        private void AddPageButton(Panel p, int i)
        {

            var button = new SelectedButton();
            button.Click += (s, e) =>
            {
                p.Children.OfType<SelectedButton>().ForEach(z => z.Selected = false);
                button.Selected = true;
                GotoNewPage((int)button.CommandParameter);
            };
            button.Style = BaseGrid.Resources["ButtonStyle3"] as Style;
            button.CommandParameter = i;
            button.Content = i.ToString();
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Cursor = Cursors.Hand;
            button.Margin = new Thickness(8, 0, 0, 0);
            if (i == CurrentPage) button.IsEnabled = false;
            p.Children.Add(button);
        }

        /// <summary>
        ///     执行转到新页面事件
        /// </summary>
        /// <param name="newPageIndex"></param>
        private void GotoNewPage(int newPageIndex)
        {
            if (newPageIndex >= 1 && newPageIndex <= TotalPage)
            {
                OnPageChanged(new LevcnPageChangedEventArgs(newPageIndex));
            }
        }

        /// <summary>
        ///     设置分页信息
        /// </summary>
        /// <param name="page"></param>
        public void SetPageInfo(PageInfo page = null)
        {
            if (page == null)
            {
                page = new PageInfo { TotalPage = 1, PageSize = 20, PageIndex = 1 };
            }
            CurrentPage = page.PageIndex;
            totalPage = page.TotalPage;
            recordCount = page.TotalRecord;
            PageSize = page.PageSize;
            DataBind();
        }

        private void PageSizeCB_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!pageEvent) return;
            var cb = sender as ComboBox;
            if (cb.SelectedIndex >= 0)
            {
                PageSize = Convert.ToInt32((cb.SelectedValue));
                if (CurrentPage >= 1)
                {
                    OnPageChanged(CurrentPage);
                }
            }
        }

        #region Event

        /// <summary>
        ///     触发分页改变事件(该事件可促使重新加载数据表)
        /// </summary>
        public void RunPageChanged()
        {
            OnPageChanged(new LevcnPageChangedEventArgs(CurrentPage));
        }

        /// <summary>
        ///     用户点击了不同的页面
        /// </summary>
        public event EventHandler<LevcnPageChangedEventArgs> PageChanged;

        public void OnPageChanged(int pageIndex = 1)
        {
            OnPageChanged(new LevcnPageChangedEventArgs(pageIndex));
        }

        public void OnPageChanged(LevcnPageChangedEventArgs e)
        {
            CurrentPage = e.NewPageIndex;
            EventHandler<LevcnPageChangedEventArgs> handler = PageChanged;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
    /// <summary>
    /// 切换页面事件参数
    /// </summary>
    public class LevcnPageChangedEventArgs : EventArgs
    {
        public LevcnPageChangedEventArgs(int newPageIndex)
        {
            NewPageIndex = newPageIndex;
        }
        public int NewPageIndex { get; set; }
    }
    public class PageInfo
    {
        public int IsPaging { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecord { get; set; }
        /// <summary>
        /// 总页
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        public int StartRecord { get; set; }
        public int EndRecord { get; set; }
        /// <summary>
        /// 页面记录数
        /// </summary>
        public int PageSize { get; set; }
    }
}
