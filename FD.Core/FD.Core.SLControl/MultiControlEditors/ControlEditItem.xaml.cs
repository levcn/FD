using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace SLControls.MultiControlEditors
{
    public partial class ControlEditItem : UserControl
    {
        public ControlEditItem()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof (string), typeof (ControlEditItem), new PropertyMetadata(default(string)));

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

        public static readonly DependencyProperty ComboBoxVisibilityProperty =
                DependencyProperty.Register("ComboBoxVisibility", typeof (Visibility), typeof (ControlEditItem), new PropertyMetadata(default(Visibility)));

        public Visibility ComboBoxVisibility
        {
            get
            {
                return (Visibility) GetValue(ComboBoxVisibilityProperty);
            }
            set
            {
                SetValue(ComboBoxVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty TextVisibilityProperty =
                DependencyProperty.Register("TextVisibility", typeof (Visibility), typeof (ControlEditItem), new PropertyMetadata(default(Visibility)));

        public Visibility TextVisibility
        {
            get
            {
                return (Visibility) GetValue(TextVisibilityProperty);
            }
            set
            {
                SetValue(TextVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty ValueTypeProperty =
                DependencyProperty.Register("ValueType", typeof (int), typeof (ControlEditItem), new PropertyMetadata(default(int), (s, e) =>
                {
                    var control1 = ((ControlEditItem)s);
                    var newValue = (int)e.NewValue;
                    if (newValue == 1)
                    {
                        control1.TextVisibility = Visibility.Visible;
                        control1.ComboBoxVisibility = Visibility.Collapsed;
                    }
                    else if (newValue == 2)
                    {
                        control1.TextVisibility = Visibility.Collapsed;
                        control1.ComboBoxVisibility = Visibility.Visible;
                    }
                }));

        public int ValueType
        {
            get
            {
                return (int) GetValue(ValueTypeProperty);
            }
            set
            {
                SetValue(ValueTypeProperty, value);
            }
        }
        public static readonly DependencyProperty ItemSourceProperty =
                DependencyProperty.Register("ItemSource", typeof (List<string>), typeof (ControlEditItem), new PropertyMetadata(default(List<string>)));

        public List<string> ItemSource
        {
            get
            {
                return (List<string>) GetValue(ItemSourceProperty);
            }
            set
            {
                SetValue(ItemSourceProperty, value);
            }
        }
        public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof (string), typeof (ControlEditItem), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((ControlEditItem)s).OnValueChanged(new ValueChangedEventArgs((string)e.NewValue));
                }));

        public string Value
        {
            get
            {
                return (string) GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
                
            }
        }

        public event ValueChangedEventHandler ValueChanged;

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChangedEventHandler handler = ValueChanged;
            if (handler != null) handler(this, e);
        }
    }
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public sealed class ValueChangedEventArgs : RoutedEventArgs
    {
        public string NewValue { get; set; }

        public ValueChangedEventArgs(string newValue)
        {
            NewValue = newValue;
        }
    }
}
