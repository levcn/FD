using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectCreater.Commands;
using ProjectCreater.Settings;
using STComponse.CFG;
using WPFControls;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// SPList.xaml 的交互逻辑
    /// </summary>
    public partial class SPList
    {
        public SPList()
        {
            InitializeComponent();
            Title = "版本属性";
//            ItemUpCommand = new DelegateCommand(w => InvalideUpItem(), w => ItemUp());
//            ItemDownCommand = new DelegateCommand(w => InvalideDownItem(), w => ItemDown());
            ItemAddCommand = new DelegateCommand(w => InvalideAddItem(), w => AddItem());
            ItemDeleteCommand = new DelegateCommand(w => InvalideDeleteItem(), w => DeleteItem());
            if(!MainUtils.InDesignMode) Loaded += SPList_Loaded;
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;
            dataGrid.CanUserAddRows = false;
        }

        void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemDeleteCommand.InvalideCanExecute();
        }
        public void DeleteItem()
        {
            if (MainUtils.MessageBoxYesNo(this, "删除确认", "您确定要删除选定的存储过程吗？") == MessageBoxResult.Yes)
            {
                var list = dataGrid.ItemsSource as IList;
                dataGrid.SelectedItems.OfType<StoredProcedure>().ToList().Where(w => w != null).ToList().ForEach(w =>
                {
                    list.Remove(w);
                });
            }
        }

        private void AddItem()
        {
//            if (EDataObjectList == null) EDataObjectList = new ObservableCollection<EDataObject>();
            StoredProcedure eo = new StoredProcedure
            {
                Parameters = new ObservableCollection<SPParameter>(),
            };
//            SetNewLocation(eo);
            eo.ObjectName = SObjectList.GetNewObjectName();
            eo.ObjectCode = SObjectList.GetNewObjectCode();
            VersionManage.Current.CurrentVersion.StoredProcedures.Add(eo);
        }
        public virtual bool InvalideDeleteItem()
        {
            var invalideDeleteItem = dataGrid != null && dataGrid.SelectedItem != null;
            return invalideDeleteItem;
        }
        public virtual bool InvalideAddItem()
        {
            return true;
        }
        void SPList_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = VersionManage.Current.CurrentVersion.StoredProcedures;
        }
        public static readonly DependencyProperty ItemDeleteCommandProperty =
                DependencyProperty.Register("ItemDeleteCommand", typeof(DelegateCommand), typeof(SPList), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand ItemDeleteCommand
        {
            get
            {
                return (DelegateCommand)GetValue(ItemDeleteCommandProperty);
            }
            set
            {
                SetValue(ItemDeleteCommandProperty, value);
            }
        }
        public static readonly DependencyProperty ItemAddCommandProperty =
                DependencyProperty.Register("ItemAddCommand", typeof(DelegateCommand), typeof(SPList), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand ItemAddCommand
        {
            get
            {
                return (DelegateCommand)GetValue(ItemAddCommandProperty);
            }
            set
            {
                SetValue(ItemAddCommandProperty, value);
            }
        }
        public static readonly DependencyProperty ItemDownCommandProperty =
                DependencyProperty.Register("ItemDownCommand", typeof(DelegateCommand), typeof(SPList), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand ItemDownCommand
        {
            get
            {
                return (DelegateCommand)GetValue(ItemDownCommandProperty);
            }
            set
            {
                SetValue(ItemDownCommandProperty, value);
            }
        }
        public static readonly DependencyProperty ItemUpCommandProperty =
                DependencyProperty.Register("ItemUpCommand", typeof(DelegateCommand), typeof(SPList), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand ItemUpCommand
        {
            get
            {
                return (DelegateCommand)GetValue(ItemUpCommandProperty);
            }
            set
            {
                SetValue(ItemUpCommandProperty, value);
            }
        }

        public static readonly DependencyProperty EditableProperty =
                DependencyProperty.Register("Editable", typeof (bool), typeof (SPList), new PropertyMetadata(default(bool)));

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
        public static readonly DependencyProperty VersionNumberProperty =
                DependencyProperty.Register("VersionNumber", typeof (int), typeof (SPList), new PropertyMetadata(default(int)));

        public int VersionNumber
        {
            get
            {
                return (int) GetValue(VersionNumberProperty);
            }
            set
            {
                SetValue(VersionNumberProperty, value);
            }
        }
        public static readonly DependencyProperty RemarkProperty =
                DependencyProperty.Register("Remark", typeof (string), typeof (SPList), new PropertyMetadata(default(string)));

        public string Remark
        {
            get
            {
                return (string) GetValue(RemarkProperty);
            }
            set
            {
                SetValue(RemarkProperty, value);
            }
        }
        public static readonly DependencyProperty VersionNameProperty =
                DependencyProperty.Register("VersionName", typeof (string), typeof (SPList), new PropertyMetadata(default(string)));

        public string VersionName
        {
            get
            {
                return (string) GetValue(VersionNameProperty);
            }
            set
            {
                SetValue(VersionNameProperty, value);
            }
        }

        private void OnResult2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnResult1(object sender, RoutedEventArgs e)
        {
            var item = VersionManage.Current.Versions.FirstOrDefault(w => w.VersionName == VersionName);
            if (item != null)
            {
                MainUtils.ShowMessageBoxInfo(this,"版本名称已经存在.");
            }
            else
            {
                DialogResult = true;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            var sp = button.DataContext as StoredProcedure;
            SPMainEdit sme = new SPMainEdit();
            sme.Editable = Editable;
            sme.StoredProcedure = sp;
            sme.Width = 1114;
            sme.Height = 528;
            sme.ShowDialog(this);
        }
    }
}
