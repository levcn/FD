using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Client;
using SLControls.Extends;
using STComponse.CFG;
using STComponse.GCode;


namespace SLControls.Editors
{
    public class BigFieldEditor : Grid
    {
        public BigFieldEditor()
        {
//            DefaultStyleKey = typeof(BigFieldEditor);
            Groups.CollectionChanged += Groups_CollectionChanged;
        }

        void Groups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        public static readonly DependencyProperty SavePathProperty =
                DependencyProperty.Register("SavePath", typeof (string), typeof (BigFieldEditor), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((BigFieldEditor) s).OnSavePathChanged(e.NewValue as string);
                }));

        private void OnSavePathChanged(string list)
        {}

        public string SavePath
        {
            get
            {
                return (string) GetValue(SavePathProperty);
            }
            set
            {
                SetValue(SavePathProperty, value);
            }
        }
        public static readonly DependencyProperty TemplateStringProperty =
                DependencyProperty.Register("TemplateString", typeof (string), typeof (BigFieldEditor), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((BigFieldEditor) s).OnTemplateStringChanged(e.NewValue as string);
                }));

        private void OnTemplateStringChanged(string list)
        {
            FrameworkElement fe = null;
            if (list != null)
            {
                fe = XamlReader.Load(list) as FrameworkElement;
            }
            Children.Clear();
            if (fe != null)
            {
                Children.Add(fe);
            }
            InitData();
        }

        private void InitData()
        {
            if (Items != null) Doubles.SetBinding(Items, this, Groups, SavePath);
        }
        public class Group
        {
            public Group(string name, string template)
            {
                Name = name;
                Template = template;
            }

            public string Name { get; set; }
            public string Template { get; set; }
        }

        public static readonly DependencyProperty GroupsProperty =
                DependencyProperty.Register("Groups", typeof(ObservableCollection<Group>), typeof(BigFieldEditor), new PropertyMetadata(new ObservableCollection<Group>(), (s, e) =>
                {
                    ((BigFieldEditor)s).OnGroupsChanged(e.NewValue as ObservableCollection<Group>);
                }));

        private void OnGroupsChanged(ObservableCollection<Group> list)
        {
            
        }

        public ObservableCollection<Group> Groups
        {
            get
            {
                return (ObservableCollection<Group>)GetValue(GroupsProperty);
            }
            private set
            {
                SetValue(GroupsProperty, value);
            }
        }
        public string TemplateString
        {
            get
            {
                return (string) GetValue(TemplateStringProperty);
            }
            set
            {
                SetValue(TemplateStringProperty, value);
            }
        }

        public static readonly DependencyProperty BigFieldValueProperty =
                DependencyProperty.Register("BigFieldValue", typeof (BigFieldValue), typeof (BigFieldEditor), new PropertyMetadata(default(BigFieldValue), (s, e) =>
                {
                    ((BigFieldEditor) s).OnBigFieldValueChanged(e.NewValue as BigFieldValue);
                }));

        private void OnBigFieldValueChanged(BigFieldValue list)
        {
            if (list != null)
            {
                Binding b = new Binding("ConfigItems");
                b.Mode = BindingMode.TwoWay;
                b.Source = list;
                SetBinding(ItemsProperty, b);
            }
        }

        public BigFieldValue BigFieldValue
        {
            get
            {
                return (BigFieldValue) GetValue(BigFieldValueProperty);
            }
            set
            {
                SetValue(BigFieldValueProperty, value);
            }
        }

        public static readonly DependencyProperty TextValueProperty =
                DependencyProperty.Register("TextValue", typeof (string), typeof (BigFieldEditor), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((BigFieldEditor) s).OnTextValueChanged(e.NewValue as string);
                }));

        private void OnTextValueChanged(string list)
        {
            if (textValueChangeEvent)
            {
                BigFieldValue = list.ToObject<BigFieldValue>();
                if (BigFieldValue != null)
                {
                    Items = BigFieldValue.ConfigItems ?? new ObservableCollection<BigFieldItem>();
                }
                else
                {
                    Items = new ObservableCollection<BigFieldItem>();
                }
            }
        }

        public string TextValue
        {
            get
            {
                return (string) GetValue(TextValueProperty);
            }
            set
            {
                SetValue(TextValueProperty, value);
            }
        }
        public static readonly DependencyProperty ItemsProperty =
                DependencyProperty.Register("Items", typeof(ObservableCollection<BigFieldItem>), typeof(BigFieldEditor),
                new PropertyMetadata(default(ObservableCollection<BigFieldItem>), (s, e) =>
                {
                    ((BigFieldEditor)s).OnItemsChanged(e.NewValue as ObservableCollection<BigFieldItem>);
                }));

        private void OnItemsChanged(ObservableCollection<BigFieldItem> list)
        {
            if (list != null)
            {
                list.ForEach(w =>
                {
                    if (string.IsNullOrEmpty(w.Value)) w.Value = w.DefaultValue;
                    w.PropertyChanged += ItemPropChanged;
                });
                list.CollectionChanged += list_CollectionChanged;
            }
            InitData();
        }

        private bool textValueChangeEvent = true;
        void UpdateString()
        {
            textValueChangeEvent = false;
            TextValue = BigFieldValue.ToJson();
            textValueChangeEvent = true;
        }
        void list_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
//            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems!=null) e.OldItems.OfType<BigFieldItem>().ForEach(w => w.PropertyChanged -= ItemPropChanged);
            }
//            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null) e.NewItems.OfType<BigFieldItem>().ForEach(w => w.PropertyChanged += ItemPropChanged);
            }
            UpdateString();
        }

        private void ItemPropChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateString();
        }

        public ObservableCollection<BigFieldItem> Items
        {
            get
            {
                return (ObservableCollection<BigFieldItem>)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }
    }
}
