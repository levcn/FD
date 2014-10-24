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
    public partial class SPMainEdit
    {
        public SPMainEdit()
        {
            InitializeComponent();
            Title = "对象编辑";
            KeyDown += SObjectMainEdit_KeyDown;
        }

        void SObjectMainEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
        }

        public static readonly DependencyProperty EditableProperty =
                DependencyProperty.Register("Editable", typeof(bool), typeof(SPMainEdit), new PropertyMetadata(true));

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
                DependencyProperty.Register("StoredProcedure", typeof(StoredProcedure), typeof(SPMainEdit), new PropertyMetadata(default(StoredProcedure)));

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

        private void BaseGridViewPanel_OnDeleteBefore(object sender, DeleteBeforeEventArgs args)
        {
            var p = args.Item as Property;
//            var rel = StoredProcedure.Relation.Where(w => w.ObjectPorertity == p.Code).ToList();
//            if (rel.Any())
//            {
//
//                if (MessageBox.Show("该属性有关联关系,该操作会同时删除关联,是否继续删除?", "确认", MessageBoxButton.YesNo)
//                    == MessageBoxResult.No)
//                {
//                    args.AllowDelete = false;
//                }
//                else
//                {
//                    args.AllowDelete = true;
//                    rel.ForEach(w=>StoredProcedure.Relation.Remove(w));
////                    var ere = EDataObject.Relation;
////                    EDataObject.Relation = ere;
//                    relList.ItemSource = StoredProcedure.Relation;
//                }
//            }
        }
    }
}
