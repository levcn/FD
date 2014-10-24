using System;
using System.Activities.Statements;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using fastJSON;
using ProjectCreater.Settings;
using STComponse.CFG;
//using STComponse.ObjectConfig;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// TObjectList.xaml 的交互逻辑
    /// </summary>
    public partial class SObjectList
    {
        /// <summary>
        /// 画布的方格大小
        /// </summary>
        const int DragGridSize = 30;
        public SObjectList()
        {
            InitializeComponent();
            Loaded += TObjectList_Loaded;
        }

        public static readonly DependencyProperty IsVirtualProperty =
                DependencyProperty.Register("IsVirtual", typeof (bool), typeof (SObjectList), new PropertyMetadata(default(bool)));

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
        public static readonly DependencyProperty EditableProperty =
                DependencyProperty.Register("Editable", typeof (bool), typeof (SObjectList), new PropertyMetadata(true));

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
        void TObjectList_Loaded(object sender, RoutedEventArgs e)
        {
//            aaa.ItemsSource = EDataObjectList;
//            Binding b = new Binding("EDataObjectList");
//            b.Source = this;
//            b.Mode = BindingMode.OneWay;
//            aaa.SetBinding(ItemsControl.ItemsSourceProperty, b);
            new Thread(w =>
            {
                Thread.Sleep(500);
                Action s = () =>
                {
                    InitSize();
                };
                Dispatcher.BeginInvoke(s);
            }).Start();
        }

        public static readonly DependencyProperty EDataObjectListProperty =
                DependencyProperty.Register("EDataObjectList", typeof(IList), typeof(SObjectList), new PropertyMetadata(null,
                        (s, e) =>
                        {
//                            ObservableCollection<EDataObject> list = new ObservableCollection<EDataObject>();
//                            if (e.NewValue != null)
//                            {
//                                list = new ObservableCollection<EDataObject>(e.NewValue as IList<EDataObject>);
//                            }
                            ((SObjectList)(s)).EDataObjectListChanged(e.NewValue as IList);
                            //                            ((SObjectList) (s)).aaa.ItemsSource = list;
                        }));

        private void EDataObjectListChanged(IList list)
        {
            
            if (list == null)
            {
                bbb.Children.Clear();
            }
            else
            {
                var list1 = list as INotifyCollectionChanged;
                if (list1 != null)
                {
                    list1.CollectionChanged += list_CollectionChanged;
                }
                bbb.Children.Clear();
                InitList();
            }
        }

        void list_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            bbb.Children.Clear();
            InitList();
        }

        private void InitList()
        {
            foreach (var obj in EDataObjectList.OfType<IPositioning>())
            {
                SObjectPanel p =new SObjectPanel();
                p.StartDrag += p_StartDrag;
                p.EndDrag += p_EndDrag;
                p.DataContext = obj;
                p.Width = 200;
                p.Height = 200;
                p.ContextMenu = Resources["ss"] as ContextMenu;
                Canvas.SetLeft(p,obj.Left);
                Canvas.SetTop(p,obj.Top);
                p.ContextMenuOpening += FrameworkElement_OnContextMenuOpening;
                bbb.Children.Add(p);
            }
            InitSize();
        }

        private SObjectPanel dragItem = null;
        void p_EndDrag(object sender, MouseButtonEventArgs e)
        {
            var panel = sender as SObjectPanel;
            dragItem = null;
            InitSize();
        }
        private void InitSize()
        {
            var list = bbb.Children.OfType<SObjectPanel>().ToList();
            if (list.Count == 0) return;
            var width = list.Max(w => Canvas.GetLeft(w) + w.ActualWidth);
            var height = list.Max(w => Canvas.GetTop(w) + w.ActualHeight);
            if (bbb.ActualHeight>0 &&bbb.ActualHeight <= height)
            {
                bbb.Height = height;
            }
            else
            {
                bbb.Height = Double.NaN;
            }
            if (bbb.ActualWidth>0&&bbb.ActualWidth <= width)
            {
                bbb.Width = width;
            }
            else
            {
                bbb.Width = Double.NaN;
            }
        }
        private Point StartPoint;
        void p_StartDrag(object sender, MouseButtonEventArgs e)
        {
            var panel = sender as SObjectPanel;
            StartPoint = e.GetPosition(this);
            dragItem = panel;
            startCanvasLeft = Canvas.GetLeft(dragItem);
            startCanvasTop = Canvas.GetTop(dragItem);
        }

        public IList EDataObjectList
        {
            get
            {
                return (IList)GetValue(EDataObjectListProperty);
            }
            set
            {
                SetValue(EDataObjectListProperty, value);
            }
        }

        private SObjectPanel lastItem = null;
        private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            lastItem = sender as SObjectPanel;
            //            e.
            //            var v = (int)b.DataContext;
        }

        private void MenuItem_Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var sObjectPanel = (((sender as MenuItem).Parent as ContextMenu).PlacementTarget as SObjectPanel);
            var v = sObjectPanel.DataContext as EDataObject;
            if (v != null)
            {
                SObjectMainEdit sme = new SObjectMainEdit();
                sme.Editable = Editable;
                sme.EDataObject = v;
                sme.IsVirtual = IsVirtual;
                sme.Width = 1114;
                sme.Height = 528;
                sme.ShowDialog(this);
            }
            else
            {
                var v1 = sObjectPanel.DataContext as StoredProcedure;
                if (v1 != null)
                {
                    SPMainEdit sme = new SPMainEdit();
                    sme.Editable = Editable;
                    sme.StoredProcedure = v1;
                    sme.Width = 1114;
                    sme.Height = 528;
                    sme.ShowDialog(this);
                }
            }
        }

        private void MenuItem_Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (lastItem == null) return;
            var v = lastItem.DataContext as EDataObject;
            if (v != null)
            {
                List<EDataObject> list;
                var checkResult = VersionManage.Current.CurrentVersion.CanDeleteObject(v.KeyTableName, out list);
                if (!checkResult)
                {
                    var objs = list.Select(w => w.ObjectName).Serialize1(",");
                    TextMessageBox tb = new TextMessageBox();
                    tb.Text = string.Format("您选择的对象正在被以下对象使用,无法删除.\r\n({0})", objs);
                    return;
                }
                if (Editable && MessageBox.Show("您确定要删除该对象吗?", "确认", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    if (lastItem != null)
                    {
                        v = lastItem.DataContext as EDataObject;
                        EDataObjectList.Remove(v);
                    }
                }
            }
            else
            {
                var v1 = lastItem.DataContext as StoredProcedure;
                if (v1 != null)
                {
                    if (Editable && MessageBox.Show("您确定要删除该对象吗?", "确认", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        if (lastItem != null)
                        {
                            EDataObjectList.Remove(v1);
                        }
                    }
                }
            }
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (EDataObjectList == null) EDataObjectList = new ObservableCollection<EDataObject>();
            var genericArgument = EDataObjectList.GetType().GetGenericArguments()[0];
            if (genericArgument == typeof (EDataObject))
            {
                if (EDataObjectList == null) EDataObjectList = new ObservableCollection<EDataObject>();
                EDataObject eo = new EDataObject {
                    IsVirtual = IsVirtual,
                    Property = new ObservableCollection<Property> {
                        new Property {
                            Code = "ID",
                            ColumnType = "GUID",
                            InitValue = "newid()",
                            IsPrimaryKey = true,
                            IsOutKey = true,
                            ID = Guid.NewGuid(),
                            Name = "ID",
                        }
                    },
                };
                SetNewLocation(eo);
                eo.ObjectName = GetNewObjectName();
                eo.ObjectCode = GetNewObjectCode();
                eo.KeyTableName = GetNewObjectCode();
                EDataObjectList.Add(eo);
            }
            else if (genericArgument == typeof(StoredProcedure))
            {
                if (EDataObjectList == null) EDataObjectList = new ObservableCollection<EDataObject>();
                StoredProcedure eo = new StoredProcedure
                {
                    Parameters = new ObservableCollection<SPParameter>(),
                };
                SetNewLocation(eo);
                eo.ObjectName = GetNewObjectName();
                eo.ObjectCode = GetNewObjectCode();
                EDataObjectList.Add(eo);
            }
            
        }

        public static string GetNewObjectCode()
        {
            var l = VersionManage.Current.CurrentVersion.DataObjects.Select(w => w.ObjectCode).ToList();
            l = l.Concat(VersionManage.Current.CurrentVersion.DictObject.Select(w=>w.ObjectCode)).ToList();
            l = l.Concat(VersionManage.Current.CurrentVersion.VirtualEDataObject.Select(w=>w.ObjectCode)).ToList();
            l = l.Concat(VersionManage.Current.CurrentVersion.StoredProcedures.Select(w => w.ObjectCode)).ToList();
            var list = l.Where(w => w.StartsWith("NewObject_", StringComparison.OrdinalIgnoreCase)).Select(w => w.Replace("NewObject_", "").ToInt()).ToList();
            var maxName = list.Any()?list.Max():0;
            maxName++;
            return "NewObject_" + maxName;
        }

        public static string GetNewObjectName()
        {
            var l = VersionManage.Current.CurrentVersion.DataObjects.Select(w=>w.ObjectName).ToList();
            l = l.Concat(VersionManage.Current.CurrentVersion.VirtualEDataObject.Select(w => w.ObjectName)).ToList();
            l =  l.Concat(VersionManage.Current.CurrentVersion.DictObject.Select(w=>w.ObjectName)).ToList();
            l =  l.Concat(VersionManage.Current.CurrentVersion.StoredProcedures.Select(w=>w.ObjectName)).ToList();
            var list = l.Where(w => w.StartsWith("新对象")).Select(w => w.Replace("新对象","").ToInt());

            var maxName = list.Any() ? list.Max() : 0;
            maxName++;
            return "新对象" + maxName;
        }

        private void SetNewLocation(IPositioning eo)
        {
            int x = 50;
            var l = EDataObjectList.OfType<IPositioning>().ToList();
            while(true)
            {
                var f = l.FirstOrDefault(w => w.Left == x && w.Top == x);
                if (f == null)
                {
                    eo.Left = x;
                    eo.Top = x;
                    break;
                }
                x += 40;
            }
        }

        double startCanvasTop;
        double startCanvasLeft;
        
        private void Bbb_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (dragItem != null)
            {
                var obj = dragItem.DataContext as IPositioning;
                var currentPoint = e.GetPosition(this);
                var vector = currentPoint - StartPoint;

                var left = startCanvasLeft + vector.X;
                if (left <= 0) left = 0;
                left = (int)left / DragGridSize * DragGridSize; //格式化到画布的方格中
                Canvas.SetLeft(dragItem, left);
                var top = startCanvasTop + vector.Y;
                if (top <= 0) top = 0;
                top = (int)top / DragGridSize * DragGridSize;//格式化到画布的方格中
                Canvas.SetTop(dragItem, top);
                obj.Top = (int)top;
                obj.Left = (int)left;
//                sd.Text = string.Format("{0} {1}", top, left);
            }
        }
    }
}
