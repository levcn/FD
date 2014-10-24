using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectCreater.Settings;
using STComponse;
using STComponse.CFG;
using STComponse.DB;
using WPFControls;
using MessageBox = System.Windows.MessageBox;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// AddNewVersionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditDictWindow
    {
        public EditDictWindow()
        {
            ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Loaded += EditRelWindow_Loaded;
        }

        void EditRelWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitSource();
        }
        FwConfig currentConfig;
        List<EDataObject> currentObjects;
        private void InitSource()
        {
            currentConfig = VersionManage.Current.CurrentVersion;
            currentObjects = currentConfig.DictObject.ToList();
//            currentObjects.AddRange(currentConfig.DataObjects);
            //            if (!IsSimple)
//            {
////                CB_RelTableName.ItemsSource = currentObjects;
//            }
            CB_DicTableName.ItemsSource = currentObjects;
            var obj = currentObjects.FirstOrDefault(w => w.KeyTableName == Relation.DictTableName);
            if (obj != null)
            {
                CB_DicMainField.ItemsSource = obj.Property;
                CB_DicName.ItemsSource = obj.Property;
            }
//            if (!IsSimple)
//            {
//                CB_RelTableName.IsEditable = false;
//                CB_RelMainField.IsEditable = false;
//                CB_RelDicField.IsEditable = false;
//            }
        }

        private void OnResult2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

//        public static readonly DependencyProperty IsSimpleProperty =
//                DependencyProperty.Register("IsSimple", typeof(bool), typeof(EditDictWindow), new PropertyMetadata(default(bool)));
//
//        public bool IsSimple
//        {
//            get
//            {
//                return (bool) GetValue(IsSimpleProperty);
//            }
//            set
//            {
//                SetValue(IsSimpleProperty, value);
//            }
//        }

        public static readonly DependencyProperty RelationProperty =
                DependencyProperty.Register("Relation", typeof(RelConfig), typeof(EditDictWindow), new PropertyMetadata(default(RelConfig)));

        public RelConfig Relation
        {
            get
            {
                return (RelConfig)GetValue(RelationProperty);
            }
            set
            {
                SetValue(RelationProperty, value);
            }
        }
        
        private void OnResult1(object sender, RoutedEventArgs e)
        {
            try
            {
//                MessageBox.Show(CB_RelTableName.Text);
            }
            catch (Exception eee)
            {
                if (eee is PException)
                {
                    MainUtils.ShowMessageBoxError(this,(eee.Message));
                }
                else
                {
                    var text = eee.GetBaseException().ToString();
                    MainUtils.ShowMessageBoxError(this,text);
                }
            }
            DialogResult = true;
        }

//        private void CB_RelTableName_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            var selectedItem = CB_RelTableName.SelectedItem;
//            if (selectedItem == null)
//            {
//                CB_RelMainField.ItemsSource = null;
//                CB_RelDicField.ItemsSource = null;
//                return;
//            }
//            EDataObject o = selectedItem as EDataObject;
//            var obj = currentObjects.FirstOrDefault(w => w.KeyTableName == o.KeyTableName);
//            if (obj != null)
//            {
//                CB_RelMainField.ItemsSource = obj.Property;
//                CB_RelDicField.ItemsSource = obj.Property;
//            }
//        }

        private void CB_DicMainField_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void CB_DicTableName_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = CB_DicTableName.SelectedItem;
            if (selectedItem == null)
            {
                CB_DicMainField.ItemsSource = null;
                CB_DicName.ItemsSource = null;
                return;
            }
            EDataObject o = selectedItem as EDataObject;
            var obj = currentObjects.FirstOrDefault(w => w.KeyTableName == o.KeyTableName);
            if (obj != null)
            {
                CB_DicMainField.ItemsSource = obj.Property;
                CB_DicName.ItemsSource = obj.Property;
            }
        }
    }
}
