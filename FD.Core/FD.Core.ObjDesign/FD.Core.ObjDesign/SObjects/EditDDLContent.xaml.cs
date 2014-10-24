using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// 
    /// </summary>
    public partial class EditDDLContent
    {
        public EditDDLContent()
        {
            ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Loaded += EditRelWindow_Loaded;
        }

        void EditRelWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof (string), typeof (EditDDLContent), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((EditDDLContent) s).OnTextChanged(e.NewValue as string);
                }));

        private void OnTextChanged(string list)
        {}

        public string Text
        {
            get
            {
                return (string) GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        FwConfig currentConfig;
        List<EDataObject> currentObjects;
        private void OnResult2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

//        public static readonly DependencyProperty IsSimpleProperty =
//                DependencyProperty.Register("IsSimple", typeof(bool), typeof(EditDDLContent), new PropertyMetadata(default(bool)));
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
                DependencyProperty.Register("Relation", typeof(RelConfig), typeof(EditDDLContent), new PropertyMetadata(default(RelConfig)));

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

    }
}
