using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Extends;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using ItemsControl = System.Windows.Controls.ItemsControl;


namespace SLControls.ListTags
{
    public class TagViewList : ItemsControl
    {

        public TagViewList()
        {
            DefaultStyleKey = typeof(TagViewList);
            GroupName = (new Random()).NextDouble().ToString();
        }
        Button PART_MoreButton;

        private RadWrapPanel PART_RadWrapPanel;
       public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_MoreButton = GetTemplateChild("PART_MoreButton") as Button;
            PART_RadWrapPanel = GetTemplateChild("PART_RadWrapPanel") as RadWrapPanel;
           if (PART_MoreButton != null) PART_MoreButton.Click += PART_MoreButton_Click;
           InitItems();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            InitItems();
        }
        List<TagView> AllTagView = new List<TagView>();
        List<TagView> ShowedTagView = new List<TagView>();
        private void InitItems()
        {
            if (PART_RadWrapPanel == null) return;
            PART_RadWrapPanel.Children.OfType<TagView>().ForEach(w => PART_RadWrapPanel.Children.Remove(w));
            if (Items.Count >= 0)
            {
                Items.ForEach(w =>
                {
                    var c = AddItemToShow(w);
                    AllTagView.Add(c);
                });
                
                
            }
            PART_RadWrapPanel.InvalidateMeasure();
        }

        private void RemoveItemFromShow(object w)
        {
            var item = PART_RadWrapPanel.Children.OfType<TagView>().FirstOrDefault(z=>z==w) ;
            if (item != null)
            {
                PART_RadWrapPanel.Children.Remove(item);
            }
        }
        private TagView AddItemToShow(object w)
        {
            var c = ItemTemplate.LoadContent() as TagView;

            GetValue(c, "DeleteVisibility", TagView.DeleteVisibilityProperty);
            GetValue(c, "SelectionType", TagView.SelectionTypeProperty);
            //                c.SelectionType = TagViewViewTypeStates.SingleSelection;
            c.GroupName = GroupName;
            c.DataContext = w;
            c.Deleted += c_Deleted;
            c.CheckedChanged += c_CheckedChanged;

            PART_RadWrapPanel.Children.Insert(PART_RadWrapPanel.Children.Count - 1, c);
            ShowedTagView.Add(c);
            return c;
        }

        void c_CheckedChanged(object sender, EventArgs e)
        {
            var tv = sender as TagView;
            if (tv != null && MaxCheckCount.HasValue && MaxCheckCount >= 0)
            {
                var tagViews = PART_RadWrapPanel.Children.OfType<TagView>().ToList();
                if ((tv.IsChecked.HasValue && tv.IsChecked.Value))
                {

                    {

                        var count = tagViews.Count(w => w.IsChecked.HasValue && w.IsChecked.Value);
                        if (count >= MaxCheckCount.Value)
                        {
                            //                        tv.IsChecked = false;
                            tagViews.Where(w => !(w.IsChecked.HasValue && w.IsChecked.Value))
                                    .ForEach(w =>
                                    {
                                        w.IsEnabled = false;
                                    });
                        }
                    }
                }
                else
                {
                    tagViews.ForEach(w =>
                    {
                        w.IsEnabled = true;
                    });
                }
            }
        }

        private void GetValue(TagView c,string path,DependencyProperty dp)
        {
            Binding b = new Binding(path);
            b.Mode = BindingMode.TwoWay;
            b.Source = this;
            c.SetBinding(dp, b);
        }

        void c_Deleted(object sender, EventArgs e)
        {
            var uiElement = sender as FrameworkElement;
            PART_RadWrapPanel.Children.Remove(uiElement);
            var item =  uiElement.DataContext;
            (ItemsSource as IList).Remove(item);
        }

        public static readonly DependencyProperty MaxCheckCountProperty =
                DependencyProperty.Register("MaxCheckCount", typeof (int?), typeof (TagViewList), new PropertyMetadata(default(int?)));

        public int? MaxCheckCount
        {
            get
            {
                return (int?) GetValue(MaxCheckCountProperty);
            }
            set
            {
                SetValue(MaxCheckCountProperty, value);
            }
        }
        protected override Size MeasureOverride(Size availableSize)
        {
//            InitItems();
            var size = base.MeasureOverride(availableSize);
            return size;
//            var size = base.MeasureOverride(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size lastSize = size;
            for (int i = 1; i < Items.Count; i++)
            {
//                AddItemToShow(Items[i]);
                var s = base.MeasureOverride(availableSize);
                if (s.Height > size.Height)
                {
                    RemoveItemFromShow(Items[i]);
                    return lastSize;
                }
                lastSize = s;
                InvalidateMeasure();
                return lastSize;
            }
            return lastSize;
        }

        void PART_MoreButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public static readonly DependencyProperty ShowAllProperty =
                DependencyProperty.Register("ShowAll", typeof (bool), typeof (TagViewList), new PropertyMetadata(default(bool)));

        public bool ShowAll
        {
            get
            {
                return (bool) GetValue(ShowAllProperty);
            }
            set
            {
                SetValue(ShowAllProperty, value);
            }
        }

        public static readonly DependencyProperty MoreVisibilityProperty =
                DependencyProperty.Register("MoreVisibility", typeof(Visibility), typeof(TagViewList), new PropertyMetadata(Visibility.Collapsed));

        public Visibility MoreVisibility
        {
            get
            {
                return (Visibility) GetValue(MoreVisibilityProperty);
            }
            set
            {
                SetValue(MoreVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty GroupNameProperty =
                DependencyProperty.Register("GroupName", typeof (string), typeof (TagViewList), new PropertyMetadata(default(string)));

        protected string GroupName
        {
            get
            {
                return (string) GetValue(GroupNameProperty);
            }
            set
            {
                SetValue(GroupNameProperty, value);
            }
        }
        public static readonly DependencyProperty SelectionTypeProperty =
                DependencyProperty.Register("SelectionType", typeof(TagViewViewTypeStates), typeof(TagViewList), new PropertyMetadata(TagViewViewTypeStates.MultiSelection));

        public TagViewViewTypeStates SelectionType
        {
            get
            {
                return (TagViewViewTypeStates)GetValue(SelectionTypeProperty);
            }
            set
            {
                SetValue(SelectionTypeProperty, value);
            }
        }

        public static readonly DependencyProperty DeleteVisibilityProperty =
                DependencyProperty.Register("DeleteVisibility", typeof(Visibility), typeof(TagViewList), new PropertyMetadata(Visibility.Collapsed));

        public Visibility DeleteVisibility
        {
            get
            {
                return (Visibility)GetValue(DeleteVisibilityProperty);
            }
            set
            {
                SetValue(DeleteVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty OrientationProperty =
                DependencyProperty.Register("Orientation", typeof(Orientation), typeof(TagViewList), new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get
            {
                return (Orientation)GetValue(OrientationProperty);
            }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }
    }
}
