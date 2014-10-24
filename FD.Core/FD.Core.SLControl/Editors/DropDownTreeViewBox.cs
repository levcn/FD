using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;


namespace SLControls.Editors
{
    public class DropDownTreeViewBox : DropDownBox
    {
        public DropDownTreeViewBox()
        {
            this.DefaultStyleKey = typeof(DropDownTreeViewBox);
        }
        RadTreeView radTreeView;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            radTreeView = GetTemplateChild("PART_TreeView") as RadTreeView;
            radTreeView.Selected += radTreeView_Selected;
        }

        public event TEventHandler<object, object> ItemSelected;

        protected virtual void OnItemSelected(object args)
        {
            TEventHandler<object, object> handler = ItemSelected;
            if (handler != null) handler(this, args);
        }

        void radTreeView_Selected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            var p = radTreeView.SelectedItem.GetType().GetProperties().FirstOrDefault(w => w.Name.Equals("Text"));
            var item = radTreeView.ContainerFromItemRecursive(radTreeView.SelectedItem);
            if (OnlyLastSelectable)
            {
                if (item.HasItems)
                {
                    return;
                }
            }
            
            ContentText = p.GetValue(radTreeView.SelectedItem,null).ToString();
            OnItemSelected(radTreeView.SelectedItem);
            if (!IsMultiSelection)
            {
                base.IsDropDownOpen = false;
            }
        }

        public static readonly DependencyProperty OnlyLastSelectableProperty =
                DependencyProperty.Register("OnlyLastSelectable", typeof (bool), typeof (DropDownTreeViewBox), new PropertyMetadata(default(bool)));

        public bool OnlyLastSelectable
        {
            get
            {
                return (bool) GetValue(OnlyLastSelectableProperty);
            }
            set
            {
                SetValue(OnlyLastSelectableProperty, value);
            }
        }
        public static readonly DependencyProperty IsMultiSelectionProperty =
                DependencyProperty.Register("IsMultiSelection", typeof (bool), typeof (DropDownTreeViewBox), new PropertyMetadata(default(bool)));

        public bool IsMultiSelection
        {
            get
            {
                return (bool) GetValue(IsMultiSelectionProperty);
            }
            set
            {
                SetValue(IsMultiSelectionProperty, value);
            }
        }

        public static readonly DependencyProperty ItemTemplateSelectorProperty =
                DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(DropDownTreeViewBox), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector ItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(ItemTemplateSelectorProperty, value);
            }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
                DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DropDownTreeViewBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
                DependencyProperty.Register("ItemsSource", typeof(object), typeof(DropDownTreeViewBox), new PropertyMetadata(default(object)));

        public object ItemsSource
        {
            get
            {
                return (object)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
                DependencyProperty.Register("SelectedItem", typeof(object), typeof(DropDownTreeViewBox), new PropertyMetadata(default(object)));

        public object SelectedItem
        {
            get
            {
                return (object)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
                DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(DropDownTreeViewBox), new PropertyMetadata(default(ObservableCollection<object>)));

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return (ObservableCollection<object>)GetValue(SelectedItemsProperty);
            }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }
    }
}
