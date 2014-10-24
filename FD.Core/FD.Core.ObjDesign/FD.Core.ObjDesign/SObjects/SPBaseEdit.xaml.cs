using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ProjectCreater.Settings;
using STComponse.CFG;
//using STComponse.ObjectConfig;
using WPFControls;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// SObjectBaseEdit.xaml 的交互逻辑
    /// </summary>
    public partial class SPBaseEdit
    {
        public SPBaseEdit()
        {
            InitializeComponent();
            Loaded += SObjectBaseEdit_Loaded;
        }

        void SObjectBaseEdit_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
        }

        public static readonly DependencyProperty EditableProperty =
                DependencyProperty.Register("Editable", typeof(bool), typeof(SPBaseEdit), new PropertyMetadata(true));

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
        public static readonly DependencyProperty StoredProcedureProperty =
                DependencyProperty.Register("StoredProcedure", typeof(StoredProcedure), typeof(SPBaseEdit), new PropertyMetadata(default(StoredProcedure)));

        public StoredProcedure StoredProcedure
        {
            get
            {
                return (StoredProcedure)GetValue(StoredProcedureProperty);
            }
            set
            {
                SetValue(StoredProcedureProperty, value);
            }
        }

        private bool loaded = false;
        private void EDataCode_OnTextChanged(object sender, TextChangedEventArgs e)
        {
//            if ((string.IsNullOrEmpty(keyTableName.Text) || !changed) && loaded)//修改类名时,一起修改表名,loaded是为了绑定时,修改了类名
//            {
////                keyTableName.Text = (sender as TextBox).Text;
////                EDataObject.KeyTableName = (sender as TextBox).Text;
//            }
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
    }
}




