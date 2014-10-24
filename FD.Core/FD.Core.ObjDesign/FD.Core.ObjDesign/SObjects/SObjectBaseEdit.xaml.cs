using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
//using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjectCreater.Settings;
using ServerFw.Collection;
using STComponse.CFG;
//using STComponse.ObjectConfig;
using WPFControls;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// SObjectBaseEdit.xaml 的交互逻辑
    /// </summary>
    public partial class SObjectBaseEdit
    {
        public SObjectBaseEdit()
        {
            InitializeComponent();
            Loaded += SObjectBaseEdit_Loaded;
            if(!DesignerProperties.GetIsInDesignMode(this))StoreList = VersionManage.Current.CurrentVersion.StoredProcedures.ToList();
        }

        void SObjectBaseEdit_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            Grid_ST.Visibility = IsVirtual ? Visibility.Visible : Visibility.Collapsed;
            Grid_Table.Visibility = IsVirtual ? Visibility.Collapsed : Visibility.Visible;
        }

        public static readonly DependencyProperty StoreListProperty =
                DependencyProperty.Register("StoreList", typeof (List<StoredProcedure>), typeof (SObjectBaseEdit), new PropertyMetadata(default(List<StoredProcedure>)));

        public List<StoredProcedure> StoreList
        {
            get
            {
                return (List<StoredProcedure>) GetValue(StoreListProperty);
            }
            set
            {
                SetValue(StoreListProperty, value);
            }
        }
        public static readonly DependencyProperty IsVirtualProperty =
                DependencyProperty.Register("IsVirtual", typeof (bool), typeof (SObjectBaseEdit), new PropertyMetadata(default(bool)));

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
                DependencyProperty.Register("Editable", typeof (bool), typeof (SObjectBaseEdit), new PropertyMetadata(true));

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
        public static readonly DependencyProperty EDataObjectProperty =
                DependencyProperty.Register("EDataObject", typeof(EDataObject), typeof(SObjectBaseEdit), new PropertyMetadata(default(EDataObject)));

        public EDataObject EDataObject
        {
            get
            {
                return (EDataObject)GetValue(EDataObjectProperty);
            }
            set
            {
                SetValue(EDataObjectProperty, value);
            }
        }

        private bool loaded = false;
        private void EDataCode_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if ((string.IsNullOrEmpty(keyTableName.Text) || !changed) && loaded)//修改类名时,一起修改表名,loaded是为了绑定时,修改了类名
            {
//                keyTableName.Text = (sender as TextBox).Text;
//                EDataObject.KeyTableName = (sender as TextBox).Text;
            }
        }

        private bool changed = false;
        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
//            changed = ((sender as TextBox).Text != keyTableName.Text);
        }

        private string tbText = null;
        private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tbText = tb.Text;
        }
        private void ObjectName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            TryChange(sender as TextBox, tbText, FwConfigEx.CanObjectRename, FwConfigEx.ObjectRename);

        }

        private void TryChange(TextBox newValue,string oldValue, Func<FwConfig, string, string, bool> CanChange, Action<FwConfig, string, string> Change)
        {
            var window = Window.GetWindow(this) as BaseWindow;
//            window.SetCanClose(true);

            var newText = newValue.Text;
            if (newText != oldValue)
            {
                var re = CanChange(VersionManage.Current.CurrentVersion, tbText, newText);
                if (re)
                {
                    Change(VersionManage.Current.CurrentVersion, tbText, newText);
                }
                else
                {
                    if (window != null && window.IsClosing) return;
                    var mre = MessageBox.Show(string.Format("您输入的名称有重名,是否还原?"), "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (mre == MessageBoxResult.Yes)
                    {
                        newValue.Text = oldValue;
                        return;
                    }
                    else
                    {
                        //                    newValue.Text = tbText;
                        new Action(() =>
                        {
                            Thread.Sleep(10);
                            Dispatcher.BeginInvoke(new tt(() =>
                            {
                                newValue.Focus();
                                tbText = oldValue;
                                newValue.Text = newText;
                                newValue.SelectionStart = newText.Length;

                            }));
                        }).BeginInvoke(null, null);
                    }
                    //                    window.SetCanClose(false);

                    //                    newValue.Text = newText;
                }
            }
        }

        public delegate void tt();
        private void ObjectCode_OnLostFocus(object sender, RoutedEventArgs e)
        {
            TryChange(sender as TextBox, tbText, FwConfigEx.CanObjectRenameCode, FwConfigEx.ObjectRenameCode);
//            TryChange(ObjectTableName, tbText, FwConfigEx.CanObjectRenameTableName, FwConfigEx.ObjectRenameTableName);
        }

        private void ObjectTableName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            TryChange(sender as TextBox, tbText, FwConfigEx.CanObjectRenameTableName, FwConfigEx.ObjectRenameTableName);
        }

        private void Selector_OnSelected(object sender, RoutedEventArgs e)
        {
            var cb = sender as ComboBox;
            var edo = cb.SelectedItem as StoredProcedure;
            var spName = edo.ObjectCode;
//            var sp = VersionManage.Current.CurrentVersion.StoredProcedures.FirstOrDefault(w => w.ObjectCode == spName);
            var sp = edo;
            if (sp != null)
            {
                EDataObject.SPParameters = new TList<SPParameter>(sp.Parameters.ToList().ToJson().ToObject<List<SPParameter>>());
            }
        }
    }
}




