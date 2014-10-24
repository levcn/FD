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
using STComponse.CFG;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;


//using STComponse.ObjectConfig;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SObjectMainEdit
    {
        public SObjectMainEdit()
        {
            InitializeComponent();
            Title = "对象编辑";
            KeyDown += SObjectMainEdit_KeyDown;
            Loaded += SObjectMainEdit_Loaded;
        }

        void SObjectMainEdit_Loaded(object sender, RoutedEventArgs e)
        {
            Tab_ST.Visibility = IsVirtual ? Visibility.Visible : Visibility.Collapsed;
            Tab_Comm.Visibility = IsVirtual ? Visibility.Collapsed : Visibility.Visible;
        }

        public static readonly DependencyProperty IsVirtualProperty =
                DependencyProperty.Register("IsVirtual", typeof (bool), typeof (SObjectMainEdit), new PropertyMetadata(default(bool)));

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
        void SObjectMainEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
        }

        public static readonly DependencyProperty EditableProperty =
                DependencyProperty.Register("Editable", typeof (bool), typeof (SObjectMainEdit), new PropertyMetadata(true));

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
                DependencyProperty.Register("EDataObject", typeof(EDataObject), typeof(SObjectMainEdit), new PropertyMetadata(default(EDataObject)));

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

        private void BaseGridViewPanel_OnDeleteBefore(object sender, DeleteBeforeEventArgs args)
        {
            var p = args.Item as Property;
            var rel = EDataObject.Relation.Where(w => w.ObjectPorertity == p.Code).ToList();
            if (rel.Any())
            {

                if (MessageBox.Show("该属性有关联关系,该操作会同时删除关联,是否继续删除?", "确认", MessageBoxButton.YesNo)
                    == MessageBoxResult.No)
                {
                    args.AllowDelete = false;
                }
                else
                {
                    args.AllowDelete = true;
                    rel.ForEach(w=>EDataObject.Relation.Remove(w));
//                    var ere = EDataObject.Relation;
//                    EDataObject.Relation = ere;
                    relList.ItemSource = EDataObject.Relation;
                }
            }
        }
    }
}
