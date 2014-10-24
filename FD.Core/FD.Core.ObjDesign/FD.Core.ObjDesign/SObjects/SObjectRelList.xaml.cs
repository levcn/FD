using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using STComponse.CFG;
//using STComponse.ObjectConfig;
using WPFControls;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SObjectRelList 
    {
        public SObjectRelList()
        {
            InitializeComponent();
            dataGrid.CanUserAddRows = false;
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;
//            var dc = dataGrid.Columns[1] as DataGridComboBoxColumn;
//            dc.ItemsSource = RelTypes;
//            
        }

        private Relation current = null;
        void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            current = dataGrid.SelectedItem as Relation;
        }

        public static readonly DependencyProperty EditableProperty =
                DependencyProperty.Register("Editable", typeof (bool), typeof (SObjectRelList), new PropertyMetadata(true));

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

        public override void AddItem()
        {
//            (GridView.SelectedItem as Relation).RelationInfo = "sdfasdfasdfasdf";
            var list = dataGrid.ItemsSource as IList;
            var newItem = new Relation();
            list.Add(newItem);
//            ItemSource = list;
        }

        public override void DeleteItem()
        {
            if(MainUtils.MessageBoxYesNo(this,"删除确认","您确定要删除选定的关联吗？") == MessageBoxResult.Yes)
            {
                var list = dataGrid.ItemsSource as IList;
                dataGrid.SelectedItems.OfType<Relation>().ToList().Where(w => w != null).ToList().ForEach(w =>
                {
                    list.Remove(w);
                });
            }
        }
        public override DataGrid GridView
        {
            get
            {
                return dataGrid;
            }
        }
        public static readonly DependencyProperty ItemSourceProperty =
                DependencyProperty.Register("ItemSource", typeof(IEnumerable), typeof(SObjectRelList), new PropertyMetadata(default(IEnumerable), (s, e) =>
                {
                    var sp = ((SObjectRelList)s);

                    sp.ItemSourceChanged(e.NewValue as IEnumerable);
                }));

        

        public IEnumerable ItemSource
        {
            get
            {
                return (IEnumerable) GetValue(ItemSourceProperty);
            }
            set
            {
                SetValue(ItemSourceProperty, value);
                ItemSourceChanged(value);
            }
        }
        private void BT_Edit_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private bool notifyChange = true;
        private void ItemSourceChanged(IEnumerable enumerable)
        {
            if (notifyChange) BindSource(enumerable);
        }

        private void BindSource(IEnumerable enumerable)
        {
            dataGrid.ItemsSource = enumerable;
        }

        Relation GetCurrentItem()
        {
            return dataGrid.SelectedItem as Relation;
        }

        private void BT_Delete_OnClick(object sender, RoutedEventArgs e)
        {
DeleteItem();
        }
        //下移
        private void BT_Down_OnClick(object sender, RoutedEventArgs e)
        {
            var item = GetCurrentItem();
            if (item != null)
            {
                var list = dataGrid.ItemsSource as IList;
                var index = list.IndexOf(item);
                if (index >= 0 && index < list.Count - 1)
                {
                    list.Remove(item);
                    list.Insert(index+1,item);
                    ItemSource = list;
                }
            }
        }
        //上移
        private void BT_Up_OnClick(object sender, RoutedEventArgs e)
        {
            var item = GetCurrentItem();
            if (item != null)
            {
                var list = dataGrid.ItemsSource as IList;
                var index = list.IndexOf(item);
                if (index >= 1 && index < list.Count)
                {
                    list.Remove(item);
                    list.Insert(index - 1, item);
                    ItemSource = list;
                }
            }
        }

        private void BT_Add_OnClick(object sender, RoutedEventArgs e)
        {
AddItem();
        }

        public static readonly DependencyProperty EDataObjectProperty =
                DependencyProperty.Register("EDataObject", typeof(EDataObject), typeof(SObjectRelList), new PropertyMetadata(default(EDataObject)));

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
//        private List<Relation> GetDataGridItemList()
//        {
//            if (dataGrid.ItemsSource == null) return new List<Relation>();
//            return dataGrid.ItemsSource.Cast<Relation>().ToList();
//        }

        private void BT_Validate_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        public List<string> RelTypes
        {
            get
            {
                return new List<string>{
                    "字典",
                    "简单关联",
                    "复杂关联",
                }
                ;
            }
        }
        //SYS_RoleUserRel(UserID,RoleID),SYS_Role(ID,Name)
        private void EditDetail_OnClick(object sender, RoutedEventArgs e)
        {
                Relation relation = (GridView.SelectedItem as Relation);
            if (relation == null) return;
            if (relation.RelationType == "字典")
            {
                EditDictWindow rel = new EditDictWindow();
                rel.Relation = relation.RelConfig;
//                rel.IsSimple = relation.RelationType == "简单关联";
                var re = rel.ShowDialog(this);
                if (re.HasValue && re.Value)
                {
//                    string str = rel.Relation.ToDictStr();
                    relation.RelConfig = rel.Relation;
                }
            }
            else
            {

                EditRelWindow rel = new EditRelWindow();
                rel.Relation = relation.RelConfig;
                rel.IsSimple = relation.RelationType == "简单关联";
                var re = rel.ShowDialog(this);
                if (re.HasValue && re.Value)
                {
                    string str = rel.Relation;
                    relation.RelationInfo = str;
                }
            }
        }
    }
}
