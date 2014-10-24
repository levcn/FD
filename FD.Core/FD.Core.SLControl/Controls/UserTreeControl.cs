using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using FD.Core.SLControl.Data;
using SLControls;
using SLControls.Editors;
using SLControls.Extends;
using SLControls.Threads;
using StaffTrain.FwClass.Reflectors;
using Telerik.Windows.Controls;
using SelectionChangedEventArgs = Telerik.Windows.Controls.SelectionChangedEventArgs;


namespace FD.Core.SLControl.Controls
{
    /// <summary>
    ///     ���ؼ�
    /// </summary>
    public partial class UserTree : BaseMultiControl
    {
        public static void SetFocused(object target, bool enable)
        {

        }

        public static readonly DependencyProperty IsAutoEditableProperty =
                DependencyProperty.Register("IsAutoEditable", typeof(bool), typeof(UserTree), new PropertyMetadata(false));

        public static readonly DependencyProperty ExpandDeepProperty =
                DependencyProperty.Register("ExpandDeep", typeof(int), typeof(UserTree), new PropertyMetadata(1, (s, e) =>
                {
                    var userTree = (UserTree)s;
                    ThreadHelper.DelayRun(() => { return userTree.ExpandToDefaultDeep(); });
                }));

        public static readonly DependencyProperty ClickControlCheckBoxProperty =
                DependencyProperty.Register("ClickControlCheckBox", typeof(bool), typeof(UserTree), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty CheckBoxIsEnabledProperty =
                DependencyProperty.Register("CheckBoxIsEnabled", typeof(bool), typeof(UserTree), new PropertyMetadata(true));

        public static readonly DependencyProperty CheckBoxVisibilityProperty =
                DependencyProperty.Register("CheckBoxVisibility", typeof(Visibility), typeof(UserTree), new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty ContextMenuEnabledProperty = DependencyProperty.Register("ContextMenuEnabled", typeof(bool), typeof(UserTree), new PropertyMetadata(default(bool)));

        public EventHandler<ContextMenuRoutedEventArgs> BeforeShowMenu;
        private double _height;
        private double _width;
        private INode lastClickNode;

        public UserTree()
        {
//            InitializeComponent();
            InitTreeSource();
            Loaded += UserTree_Loaded;
            Unloaded += UserTree_Unloaded;
            //            TreeViewMain.SelectedItemChanged += TreeViewMain_SelectedItemChanged;
        }
        RadTreeView TreeViewMain;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TreeViewMain = GetTemplateChild("TreeViewMain") as RadTreeView;
            Grid LayoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            if (LayoutRoot != null)
            {
                LayoutRoot.MouseRightButtonDown += TreeViewMain_MouseRightButtonDown;
            }
            if (TreeViewMain != null)
            {
                TreeViewMain.SelectionChanged += TreeViewMain_SelectedItemChanged;
                TreeViewMain.MouseRightButtonDown += TreeViewMain_MouseRightButtonDown;
                TreeViewMain.MouseRightButtonUp += TreeViewMain_MouseRightButtonUp;
                TreeViewMain.MouseLeftButtonDown += TreeViewMain_MouseLeftButtonDown;
            }
        }

        private void UserTree_Loaded(object sender, RoutedEventArgs e)
        {
            //            ContextMenu c = new ContextMenu();
            //            ContextMenuService.SetContextMenu(this, c);
            if (TreeViewMain != null) TreeViewMain.UpdateLayout();
        }

        void UserTree_Unloaded(object sender, RoutedEventArgs e)
        {
//            TContextMenuService.Remove(this);
        }

        /// <summary>
        ///     �Ƿ�����ڽڵ����Զ��༭(Ĭ�Ϸ�,���԰�EditEnd�¼�����ȡ�༭���)
        /// </summary>
        public bool IsAutoEditable
        {
            get
            {
                return (bool)GetValue(IsAutoEditableProperty);
            }
            set
            {
                SetValue(IsAutoEditableProperty, value);
            }
        }

        /// <summary>
        ///     Ĭ��չ�������,Ĭ����1
        /// </summary>
        public int ExpandDeep
        {
            get
            {
                return (int)GetValue(ExpandDeepProperty);
            }
            set
            {
                SetValue(ExpandDeepProperty, value);
            }
        }

        /// <summary>
        ///     ������ָı�Checkbox��״̬
        /// </summary>
        public bool ClickControlCheckBox
        {
            get
            {
                return (bool)GetValue(ClickControlCheckBoxProperty);
            }
            set
            {
                SetValue(ClickControlCheckBoxProperty, value);
            }
        }

        public bool CheckBoxIsEnabled
        {
            get
            {
                return (bool)GetValue(CheckBoxIsEnabledProperty);
            }
            set
            {
                SetValue(CheckBoxIsEnabledProperty, value);
            }
        }

        public Visibility CheckBoxVisibility
        {
            get
            {
                return (Visibility)GetValue(CheckBoxVisibilityProperty);
            }
            set
            {
                SetValue(CheckBoxVisibilityProperty, value);
            }
        }

        public double TreeWidth
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                TreeViewMain.Width = value;
            }
        }

        public double TreeHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                TreeViewMain.Height = value;
            }
        }

        /// <summary>
        ///     �����µĽڵ�
        /// </summary>
        public Func<INode> GetNewNode { get; set; }

        public bool ContextMenuEnabled
        {
            get
            {
                return (bool)GetValue(ContextMenuEnabledProperty);
            }
            set
            {
                SetValue(ContextMenuEnabledProperty, value);
            }
        }

        public object SelectedItem { get; set; }

        #region �ڵ��¼�
        //private ContextMenu c;
        private TextBlock _tb;
        private MenuItemEventArgs _menuItem;

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            _tb = sender as TextBlock;
            if (_tb != null)
            {
                var selected = (INode)_tb.DataContext;
                selectedNode = selected;
                RadContextMenu menus = RadContextMenu.GetContextMenu(_tb) ?? new RadContextMenu();
//                BindEvent.RemoveClickEvent(menus.Items);
                menus.Items.Clear();

                _menuItem = new MenuItemEventArgs { Entity = selected, Menus = menus, DependencyObject = _tb };

                OnMenuOpening(_menuItem);
                //menus = ar.Menus;

                //t = tb;
                //c = menus;

//                TContextMenuService.SetContextMenu(this, _tb, menus.Items.Count == 0 ? null : menus);
                //if (menus.Items.Count == 0)
                //{
                //    //                    ThreadHelper.DelayRun(() =>
                //    //                    {
                //    //                        ContextMenuService.SetContextMenu(tb, null);
                //    //                    });
                //}
                //DisableEditForSelectedItem();
                //e.Handled = false;
            }
            else
            {
                var grid = sender as Grid;
                if (grid != null)
                { }
            }
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _menuItem = null;
            _tb = null;
        }

        private void TreeViewMain_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var tb = sender as TextBlock;
            //if (tb != null)
            //{
            //    var selected = (INode)tb.DataContext;
            //    selectedNode = selected;
            //    ContextMenu menus = ContextMenuService.GetContextMenu(tb) ?? new ContextMenu();
            //    BindEvent.RemoveClickEvent(menus.Items);
            //    menus.Items.Clear();

            //    var ar = new MenuItemEventArgs { Entity = selected, Menus = menus, DependencyObject = tb };

            //    OnMenuOpening(ar);
            //    menus = ar.Menus;

            //    t = tb;
            //    c = menus;

            //    TContextMenuService.SetContextMenu(this, tb, menus.Items.Count == 0 ? null : menus);
            //    if (menus.Items.Count == 0)
            //    {
            //        //                    ThreadHelper.DelayRun(() =>
            //        //                    {
            //        //                        ContextMenuService.SetContextMenu(tb, null);
            //        //                    });
            //    }
            //    DisableEditForSelectedItem();
            //    e.Handled = false;
            //}
            //else
            //{
            //    var grid = sender as Grid;
            //    if (grid != null)
            //    { }
            //}
            if (_menuItem != null)
            {
                //OnMenuOpening(menuItem);

                //TContextMenuService.SetContextMenu(this, _tb, _menuItem.Menus.Items.Count == 0 ? null : _menuItem.Menus);
                //if (menus.Items.Count == 0)
                //{
                //    //                    ThreadHelper.DelayRun(() =>
                //    //                    {
                //    //                        ContextMenuService.SetContextMenu(tb, null);
                //    //                    });
                //}
                DisableEditForSelectedItem();
                e.Handled = false;
            }
            else
            {
                var grid = sender as Grid;
                if (grid != null)
                { }
            }
        }

        private void TreeViewMain_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

            DisableEditForSelectedItem();

            if (sender is TextBlock)
            {
                TextBlock textBlock = sender as TextBlock;
                selectedNode = (INode)(textBlock.DataContext);
            }
            else
            {
                selectedNode = null;
            }

            //ShowContextMenu(e);
        }

        private void TreeViewMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisableEditForSelectedItem();
            //HideContextMenu();
            SetCheckBoxState(sender);
            if (IsAutoEditable)
            {
                var ttt = GetParentTreeViewItem(sender as DependencyObject);
                if (ttt == null) return;
                var node = ttt.DataContext as INode;
                if (lastClickNode == node)
                {
                    EnalbleEditForSelectedItem(node);
                    //                    lastClickNode = null;
                }
                //                else
                {
                    lastClickNode = node;
                }
            }
        }

        private void SetCheckBoxState(object sender)
        {
            if (ClickControlCheckBox)
            {
                var ttt = GetParentTreeViewItem(sender as DependencyObject);
                if (ttt == null) return;
                var er = ttt.DataContext as INode;
                if (er != null)
                {
                    er.IsSelected = !er.IsSelected;
                    OnCheckBoxChanged(new CheckBoxChangedEventArgs { Item = er, Checked = er.IsSelected });
                }
            }
        }

        #endregion

        #region  ����Ҽ��˵��¼�

        public void AddNode(INode newNode, bool addToRoot = false)
        {
            if (selectedNode != null && !addToRoot)
            {
                selectedNode.Add(newNode);
                var item = TreeViewMain.ContainerFromItemRecursive(selectedNode);
                item.IsExpanded = true;
            }
            else
            {
                if (DataSource != null)
                {
                    (DataSource as IList).Add(newNode);
                }
                else
                {
                    DataSource = new List<INode> { newNode };
                    //                    DataSource = new ObservableCollection<INode> {newNode};
                }
            }
        }

        /// <summary>
        ///     ���õ�һ��չ��
        /// </summary>
        public void SetFirstLayerExpanded()
        {
            if (DataSource != null)
                foreach (INode node in DataSource)
                {
                    SetIsExpanded(node, true);
                }
        }

        /// <summary>
        ///     �����Ƿ�չ��
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isExpanded"></param>
        public bool SetIsExpanded(INode node, bool isExpanded)
        {
//            TreeViewMain.ContainerFromItemRecursive(node)
            RadTreeViewItem selectedTreeViewItem = TreeViewMain.ContainerFromItemRecursive(node);
            if (selectedTreeViewItem != null)
            {
                INode item = node;
                item.IsExpanded = true;
                selectedTreeViewItem.IsExpanded = isExpanded;
                return true;
            }
            return false;
        }

        //private void HideContextMenu()
        //{
        //    ContextMenu.Visibility = Visibility.Collapsed;
        //    ContextMenu.IsOpen = false;
        //}

        #endregion

        #region �༭�ڵ�

        private INode _editItem;
        private string lastEditItemEditBeforeText;

        public void EnalbleEditForSelectedItem(INode node = null)
        {
            if (node == null) node = selectedNode;
            _editItem = node;
            if (node != null)
            {
                SetTemplateForSelectedItem("TreeViewMainEditTemplate", node);
            }
        }

        public void DisableEditForSelectedItem(INode node = null)
        {
            if (node == null) node = _editItem;
            if (node != null)
            {
                SetTemplateForSelectedItem("TreeViewMainReadTemplate", node);
                _editItem = null;
                if (lastEditItemEditBeforeText != node.Text) //����ı����ݸı�
                {
                    ThreadHelper.StartThread(
                            () => Dispatcher.BeginInvoke(
                                    () => OnEditend(new UserTreeEventArgs(node))), 100);
                }
            }
        }

        private void SetTemplateForSelectedItem(String templateName, INode node)
        {
            lastEditItemEditBeforeText = node.Text;
            var hdt = (HierarchicalDataTemplate)Resources[templateName];
            RadTreeViewItem selectedTreeViewItem = TreeViewMain.ContainerFromItemRecursive(node);
            if (selectedTreeViewItem == null) return;
            selectedTreeViewItem.HeaderTemplate = hdt;
            ThreadHelper.DelayRun(() =>
            {
                var box = selectedTreeViewItem.FindChildInTemplate<TextBox>("textBox");
                if (box != null) box.Focus();
            }, 10);

        }

        #endregion

        /// <summary>
        ///     ���ٱ༭����,�ڵ�Text�����仯ʱ����
        /// </summary>
        public event EventHandler<UserTreeEventArgs> EditEnd;

        public override void LoadConfig(string configStr)
        {
            
        }

        public void OnEditend(UserTreeEventArgs e)
        {
            EventHandler<UserTreeEventArgs> handler = EditEnd;
            if (handler != null) handler(this, e);
        }



        //����Checkbox����¼�
        //��̬��������ȡ����TreeViewItem

        private static RadTreeViewItem GetParentTreeViewItem(DependencyObject item)
        {
            if (item != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(item); //��ȡ�����ĸ�������
                var parentTreeViewItem = parent as RadTreeViewItem; //����ת��
                //�������TreeViewItem�����򷵻أ�����͵ݹ�Ѱ��
                return parentTreeViewItem ?? GetParentTreeViewItem(parent);
            }
            //�Ҳ��������󣬷��ظ����󲻴���
            return null;
        }

        /// <summary>
        ///     �����ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemCheckbox_Click(object sender, RoutedEventArgs e)
        {
            var item = GetParentTreeViewItem((DependencyObject)sender);

            if (item == null) return;

            bool? isChecked = (sender as CheckBox).IsChecked;
            var feature = item.DataContext as INode;
            if (feature == null) return;
            //���¹��ܲ�����,���¼�չ��ʱ,ѡ�е�ǰ�ڵ�,�¼�����ѡ��
            UpdateChildrenCheckedState(feature); //���������ѡ��״̬
            UpdateParentCheckedState(item); //���¸����ѡ��״̬

            OnCheckBoxChanged(new CheckBoxChangedEventArgs { Item = feature, Checked = isChecked });
        }

        /// <summary>
        ///     ��������ѡ�еĽڵ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetSelectedItems<T>() where T : INode
        {
            var re = new List<T>();
            TreeViewMain.Items.Cast<T>().ToList().ForEach(w => re.AddRange(GetSelectedItems(w)));
            return re;
        }

        private IEnumerable<T> GetSelectedItems<T>(T w) where T : INode
        {
            var re = new List<T>();
            re.Add(w);
            w.Children.Cast<T>().ToList().ForEach(q => re.AddRange(GetCheckedListItems(q)));
            return re;
        }

        /// <summary>
        ///     ��������CheckBoxѡ�е��б�
        /// </summary>
        /// <returns></returns>
        public List<T> GetCheckedListItems<T>() where T : INode
        {
            var re = new List<T>();
            TreeViewMain.Items.Cast<T>().ToList().ForEach(w => re.AddRange(GetCheckedListItems(w)));
            //            TreeViewExtensions.GetIsLeaf();
            return re;
            //var items = TreeViewMain.Items[0] as TreeItemBase;

            //            var itm = TreeViewExtensions.GetContainerFromItem(TreeViewMain, items);
            //return new List<TreeItemBase>();
        }

        public void RemoveItem(IEnumerable<INode> node)
        {
            node.ForEach(RemoveItem);
        }

        public void RemoveItem(INode node)
        {
            if ((DataSource as IList).Contains(node))
            {
                if (selectedNode == node)
                {
                    selectedNode = null;
                }
                (DataSource as IList).Remove(node);
            }
            else
            {
                foreach (INode node1 in DataSource)
                {
                    RemoveItem(node1, node);
                }
            }
        }

        /// <summary>
        ///     ɾ��һ���ڵ�
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="node"></param>
        private void RemoveItem(INode currentNode, INode node)
        {
            if (currentNode.Children.Contains(node))
            {
                currentNode.Children.Remove(node);
            }
            else
            {
                foreach (INode node1 in currentNode.Children)
                {
                    RemoveItem(node1, node);
                }
            }
        }

        private IEnumerable<T> GetCheckedListItems<T>(T w) where T : INode
        {
            var re = new List<T>();
            if (w.IsSelected) re.Add(w);
            w.Children.Cast<T>().ToList().ForEach(q => re.AddRange(GetCheckedListItems(q)));
            return re;
        }

        //��̬���������¸���TreeViewItemѡ��״̬

        private void UpdateParentCheckedState(RadTreeViewItem item)
        {
            var parent = GetParentTreeViewItem(item); //��ȡ����TreeViewItem
            if (parent == null) return;
            var feature = parent.DataContext as INode; //����ת��
            if (feature != null) //�������Ϊ��
            {
                //�����������ѡ��״̬
                bool? childrenCheckedState = feature.Children.ToList().Cast<INode>().First<INode>().IsSelected;
                //�õ���һ���������ѡ��״̬
                for (int i = 1; i < feature.Children.ToList().Cast<INode>().Count(); i++)
                {
                    if (childrenCheckedState == (feature.Children[i]).IsSelected) continue;
                    childrenCheckedState = null;
                    break;
                }
                //���������ѡ��״̬���������Ϊ��ͬ

                INode itemBase = (feature);
                bool selected = childrenCheckedState ?? false;
                if (itemBase.IsSelected != selected)
                {
                    itemBase.IsSelected = selected;
                    OnCheckBoxChanged(new CheckBoxChangedEventArgs { Item = itemBase, Checked = itemBase.IsSelected });
                }

                //�����ݹ�����.
                UpdateParentCheckedState(parent);
            }
        }

        //���������ӽڵ�
        public List<INode> GetAllChilds(INode feature, bool containsCurrent = false)
        {
            var re = new List<INode>();
            if (containsCurrent)
            {
                re.Add(feature);
            }
            re.AddRange(feature.Children.ToList().OfType<INode>());
            foreach (INode childFeature in feature.Children)
            {
                if (childFeature.Children.Any())
                {
                    re.AddRange(GetAllChilds(childFeature));
                }
            }
            return re;
        }

        //�õݹ�����������ѡ��״̬
        private void UpdateChildrenCheckedState(INode feature)
        {
            //if (feature.IsSelected)
            {
                foreach (INode childFeature in feature.Children)
                {
                    if (childFeature.IsSelected != feature.IsSelected)
                    {
                        childFeature.IsSelected = feature.IsSelected;

                        OnCheckBoxChanged(new CheckBoxChangedEventArgs { Item = childFeature, Checked = childFeature.IsSelected });
                    }
                    if (childFeature.Children.Any())
                    {
                        UpdateChildrenCheckedState(childFeature);
                    }
                }
            }
        }

        public event FD.Core.SLControl.Data.TEventHandler<object,SelectionChangedEventArgs> SelectedItemChanged;

        public void OnSelectedItemChanged(SelectionChangedEventArgs e)
        {
            var handler = SelectedItemChanged;
            if (handler != null) handler(this, e);
            //            var dwwee = TreeViewMain.GetContainerFromItem(e.NewValue);//.FindName("checkBox");
            //dwwee.
        }

        public event EventHandler<CheckBoxChangedEventArgs> CheckBoxChanged;

        public void OnCheckBoxChanged(CheckBoxChangedEventArgs e)
        {
            EventHandler<CheckBoxChangedEventArgs> handler = CheckBoxChanged;
            if (handler != null) handler(this, e);
        }

        private void TreeViewMain_SelectedItemChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            //            var ewww = GetTemplateChild("checkBox");
            SelectedItem = TreeViewMain.SelectedItem;
            OnSelectedItemChanged(selectionChangedEventArgs);

        }

        //��ʼ���˵�
        //private void InitSPMenuList()
        //{
        //    if (SPMenuList != null)
        //    {
        //        foreach (HyperlinkButton menu in SPMenuList)
        //        {
        //            SPMenu.Children.Add(menu);
        //        }
        //    }
        //}

        protected void OnBeforeShowMenu(ContextMenuRoutedEventArgs ea)
        {
            if (BeforeShowMenu != null) BeforeShowMenu(this, ea);
        }

        public event RoutedEventHandler BeforeAdd_Click;
        public event RoutedEventHandler AfterAdd_Click;
        public event RoutedEventHandler BeforeDel_Click;
        public event RoutedEventHandler AfterDel_Click;
        public event RoutedEventHandler BeforeEdit_Click;
        public event RoutedEventHandler AfterEdit_Click;
        public event EventHandler<MenuItemEventArgs> MenuOpening;

        public void OnMenuOpening(MenuItemEventArgs e)
        {
            EventHandler<MenuItemEventArgs> handler = MenuOpening;
            if (handler != null) handler(this, e);
        }

        //�˵����б�

        //��ʼ������Դ
        private void InitTreeSource()
        {
            //TreeViewMain.ItemsSource = DataSource;
        }

        ///// <summary>
        ///// ����ѡ�е���
        ///// </summary>
        ///// <returns></returns>
        //public List<TreeItemBase> GetCheckedItem()
        //{
        //    return TreeDataHelper.GetListByTree(DataSource.Cast<TreeItemBase>().ToList()).Where(w => w.IsSelected).ToList();
        //}

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            DisableEditForSelectedItem();
        }

        /// <summary>
        ///     ��ָ���Ľڵ���ӽڵ�����
        /// </summary>
        /// <param name="treeNode"></param>
        public void SortChilds(INode treeNode)
        {
            PropertyInfo property = ReflectionHelper.GetProperty("OrderID", treeNode);
            if (property != null)
            {
                //                Func<INode, object> d = w => Convert.ChangeType(property.GetValue(w, null), typeof (int?), null) as int?;

                List<INode> list = treeNode.Children
                                           .ToList().OrderBy(w => IntExtend.GetValue(property.GetValue(w, null) ?? -1)).ToList();
                treeNode.Children = new List<INode>(list);
                //                treeNode.Children = new ObservableCollection<INode>(list);
                ThreadHelper.DelayRun(() => BindingExpand(list), 10 , "sdfsd24");
            }
        }

        /// <summary>
        ///     ����ָ���ڵ�����
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        public int GetDeep(INode treeNode)
        {
            Func<int, RadTreeViewItem, int> d = null;
            d = (currentDeep, item) =>
            {
                var p = VisualTreeHelper.GetParent(item) as RadTreeViewItem;
                if (p != null) return d(currentDeep + 1, p);
                return 1;
            };

            var t = TreeViewMain.ContainerFromItemRecursive(treeNode);
            int re = d(1, t);
            return re;
        }

        /// <summary>
        ///     ÿ���ڵ�չ����ر�ʱ����
        /// </summary>
        public event TEventHandler<UserTree, RadTreeViewItem, INode> ItemExpandChanged;

        protected virtual void OnItemExpandChanged(RadTreeViewItem node, INode item)
        {
            var handler = ItemExpandChanged;
            if (handler != null) handler(this, node, item);
        }

        #region  ȫ�ֱ���

        private IEnumerable _objectTree;

        private INode selectedNode;
        public static readonly DependencyProperty TreeFontSizeProperty = DependencyProperty.Register("TreeFontSize", typeof(double), typeof(UserTree), new PropertyMetadata(double.Parse("12")));
        public static readonly DependencyProperty TreeFontWeightProperty = DependencyProperty.Register("TreeFontWeight", typeof(FontWeight), typeof(UserTree), new PropertyMetadata(default(FontWeight)));

        public static readonly DependencyProperty DataSourceProperty =
                DependencyProperty.Register("DataSource", typeof (IEnumerable), typeof (UserTree), new PropertyMetadata(default(IEnumerable), (s, e) =>
                {
                    ((UserTree) s).OnDataSourceChanged(e.NewValue as IEnumerable);
                }));

        private void OnDataSourceChanged(IEnumerable value)
        {
            if (IsDesign()) return;
            ThreadHelper.DelayRunNew(() =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    BindingExpand(value);
                    if (ExpandDeep >= 1)
                    {
                        foreach (object radTreeViewItem in TreeViewMain.Items)
                        {
                            var containerFromItem = (RadTreeViewItem) TreeViewMain.ItemContainerGenerator.ContainerFromItem(radTreeViewItem);
                            containerFromItem.IsExpanded = true;
                        }
                        TreeViewMain.UpdateLayout();
                    }
                });
            });
        }

        public IEnumerable DataSource
        {
            get
            {
                return (IEnumerable) GetValue(DataSourceProperty);
            }
            set
            {
                SetValue(DataSourceProperty, value);
            }
        }
//        public IEnumerable DataSource
//        {
//            get
//            {
//                return _objectTree;
//            }
//            set
//            {
//                _objectTree = value;
//                TreeViewMain.ItemsSource = value;
//                //                            value.CollectionChanged += value_CollectionChanged;
//                //                            InitExpand(value);
//
//                ThreadHelper.DelayRunNew(() =>
//                {
//                    Dispatcher.BeginInvoke(() =>
//                    {
//                        BindingExpand(value);
//                        if (ExpandDeep >= 1)
//                        {
//                            foreach (object radTreeViewItem in TreeViewMain.Items)
//                            {
//                                var containerFromItem = (RadTreeViewItem) TreeViewMain.ItemContainerGenerator.ContainerFromItem(radTreeViewItem);
//                                containerFromItem.IsExpanded = true;
//                            }
//                            TreeViewMain.UpdateLayout();
//                        }
//                    });
//                },100);
//            }
//        }

//        public List<INode> Items
//        {
//            get
//            {
//                return DataSource.ToList();
//            }
//            set
//            {
//                DataSource = new List<INode>(value);
//                TreeViewMain.ItemsSource = DataSource;
//            }
//        }

        public double TreeFontSize
        {
            get { return (double)GetValue(TreeFontSizeProperty); }
            set { SetValue(TreeFontSizeProperty, value); }
        }

        public FontWeight TreeFontWeight
        {
            get { return (FontWeight)GetValue(TreeFontWeightProperty); }
            set { SetValue(TreeFontWeightProperty, value); }
        }

        private bool ExpandToDefaultDeep()
        {
            if (ExpandDeep <= 0) return true;
            bool re = true;
            List<INode> list = GetTopDeepNodes(ExpandDeep);

            foreach (INode node in list)
            {
                re = SetIsExpanded(node, true);
                if (!re) return false;
            }
            return true;
        }

        /// <summary>
        ///     ����ǰ�����List
        /// </summary>
        /// <param name="expandDeep"></param>
        /// <returns></returns>
        private List<INode> GetTopDeepNodes(int expandDeep)
        {
            if (DataSource == null) return new List<INode>();
            List<INode> list = (DataSource as IList).OfType<INode>().ToList();
            List<INode> lastList = list;
            for (int i = 1; i < expandDeep; i++)
            {
                lastList = lastList.SelectMany(w => w.Children).ToList();
                list.AddRange(lastList);
            }
            return list;
        }

        //        void value_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //        {
        //            InitExpand(e.NewItems);
        //        }

        //        private void InitExpand(IList e)
        //        {
        //            if (e == null) return;
        //            ThreadHelper.DelayRun(() =>
        //            {
        //                return InitExpand1(e);
        //            });
        //
        //        }

        private void BindingExpand(IEnumerable list)
        {
            //            TreeViewMain.
            if (list == null) return;
            foreach (INode item in list)
            {
                var t = TreeViewMain.ContainerFromItemRecursive(item);

                if (t == null) continue;
                INode node = item;
                BindingExpression expression = t.GetBindingExpression(RadTreeViewItem.IsExpandedProperty);
                if (expression == null) //û�а󶨹�
                {
                    if (node.IsExpanded || t.IsExpanded) //����չ��
                    {
                        t.IsExpanded = true;
                        node.IsExpanded = true;
                    }

                    t.Expanded += (s, e) =>
                    {
                        if (node.Children != null)
                        {
                            BindingExpand(node.Children);
                        }
                        //                                DelayRun(() =>
                        //                                {
                        node.IsExpanded = t.IsExpanded;
                        OnItemExpandChanged(t, node);
                        //                                },10);
                    };
                    t.Collapsed += (s, e) =>
                    {
                        node.IsExpanded = t.IsExpanded;
                        //                                DelayRun(() =>
                        //                                {
                        OnItemExpandChanged(t, node);
                        //                                },10);  
                    };
                    t.SetBinding(RadTreeViewItem.IsExpandedProperty, new Binding("IsExpanded") { Source = node, Mode = BindingMode.TwoWay });
                    //                            t.IsExpanded = true;
                }
                //                        if (node.Children != null)
                //                        {
                //                            BindingExpand(node.Children);
                //                        }
            }
        }

        #endregion

        #region Nested type: CheckBoxChangedEventArgs

        public class CheckBoxChangedEventArgs : EventArgs
        {
            public INode Item { get; set; }
            public bool? Checked { get; set; }
        }

        #endregion

        #region Nested type: UserTreeEventArgs

        public class UserTreeEventArgs : EventArgs
        {
            public UserTreeEventArgs(INode node)
            {
                OptionItem = node;
            }

            public INode OptionItem { get; set; }
        }

        #endregion

        private void TextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) DisableEditForSelectedItem();
        }
    }
    public class ContextMenuRoutedEventArgs : RoutedEventArgs
    {
        public ContextMenuRoutedEventArgs(List<HyperlinkButton> menuitems)
        {
            MenuItems = menuitems;
        }
        public List<HyperlinkButton> MenuItems { get; set; }
    }
    public class MenuItemEventArgs : EventArgs
    {
        public object Entity { get; set; }
        public RadContextMenu Menus { get; set; }

        public DependencyObject DependencyObject { get; set; }
    }
}