using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ProjectCreater.Commands;


namespace ProjectCreater.SObjects
{
    public class BaseGridViewPanel : System.Windows.Controls.UserControl
    {
        public BaseGridViewPanel()
        {
            ItemUpCommand = new DelegateCommand(w => InvalideUpItem(), w => ItemUp());
            ItemDownCommand = new DelegateCommand(w => InvalideDownItem(), w => ItemDown());
            ItemAddCommand = new DelegateCommand(w => InvalideAddItem(), w => AddItem());
            ItemDeleteCommand = new DelegateCommand(w => InvalideDeleteItem(), w => DeleteItem());
            Loaded += BaseGridViewPanel_Loaded;
        }

        public event TEventHandler<object, DeleteBeforeEventArgs> DeleteBefore;

        protected virtual void OnDeleteBefore(DeleteBeforeEventArgs args)
        {
            var handler = DeleteBefore;
            if (handler != null) handler(this, args);
        }

        void BaseGridViewPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (GridView!=null) GridView.SelectionChanged += GridView_SelectionChanged;
        }

        void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemUpCommand.InvalideCanExecute();
            ItemDownCommand.InvalideCanExecute();
            ItemAddCommand.InvalideCanExecute();
            ItemDeleteCommand.InvalideCanExecute();
        }

        /// <summary>
        /// 列表
        /// </summary>
        public virtual DataGrid GridView
        {
            get
            {
                return null;
            }
        }

        #region 需要重写的方法 

        
        public virtual void AddItem()
        {
            
        }

        public virtual void DeleteItem()
        {
            
        }


        #endregion
        public virtual bool InvalideUpItem()
        {
            if (GridView == null) return false;
            var item = GridView.SelectedItem;
            if (item != null)
            {
                var list = GridView.Items;
                var index = list.IndexOf(item);
                if (index >= 1 && index < list.Count)
                {
                    return true;
                }
            }
            return false;
        }
        public virtual bool InvalideDownItem()
        {
            if (GridView == null) return false;
            var item = GridView.SelectedItem;
            if (item != null)
            {
                var list = GridView.Items;
                var index = list.IndexOf(item);
                if (index >= 0 && index < list.Count - 1)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool InvalideAddItem()
        {
            return true;
        }

        public virtual bool InvalideDeleteItem()
        {
            var invalideDeleteItem = GridView != null && GridView.SelectedItem != null;
            return invalideDeleteItem;
        }

        public static readonly DependencyProperty ItemDeleteCommandProperty =
                DependencyProperty.Register("ItemDeleteCommand", typeof (DelegateCommand), typeof (BaseGridViewPanel), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand ItemDeleteCommand
        {
            get
            {
                return (DelegateCommand) GetValue(ItemDeleteCommandProperty);
            }
            set
            {
                SetValue(ItemDeleteCommandProperty, value);
            }
        }
        public static readonly DependencyProperty ItemAddCommandProperty =
                DependencyProperty.Register("ItemAddCommand", typeof (DelegateCommand), typeof (BaseGridViewPanel), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand ItemAddCommand
        {
            get
            {
                return (DelegateCommand) GetValue(ItemAddCommandProperty);
            }
            set
            {
                SetValue(ItemAddCommandProperty, value);
            }
        }
        public static readonly DependencyProperty ItemDownCommandProperty =
                DependencyProperty.Register("ItemDownCommand", typeof (DelegateCommand), typeof (BaseGridViewPanel), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand ItemDownCommand
        {
            get
            {
                return (DelegateCommand) GetValue(ItemDownCommandProperty);
            }
            set
            {
                SetValue(ItemDownCommandProperty, value);
            }
        }
        public static readonly DependencyProperty ItemUpCommandProperty =
                DependencyProperty.Register("ItemUpCommand", typeof(DelegateCommand), typeof(BaseGridViewPanel), new PropertyMetadata(default(DelegateCommand)));

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

        void ItemUp()
        {
            var item = GridView.SelectedItem;
            if (item != null)
            {
                var list = GridView.ItemsSource as IList;
                var index = list.IndexOf(item);
                if (index >= 1 && index < list.Count)
                {
                    list.Remove(item);
                    list.Insert(index - 1, item);
                    GridView.ItemsSource = null;
                    GridView.ItemsSource = list;
                    GridView.SelectedIndex = index - 1;
                }
            }
        }

        void ItemDown()
        {
            var item = GridView.SelectedItem;
            if (item != null)
            {
                var list = GridView.ItemsSource as IList;
                var index = list.IndexOf(item);
                if (index >= 0 && index < list.Count - 1)
                {
                    list.Remove(item);
                    list.Insert(index + 1, item);
                    GridView.ItemsSource = null;
                    GridView.ItemsSource = list;
                    GridView.SelectedIndex = index + 1;
                }
            }
        }
    }
}
