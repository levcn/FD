using System;
using System.Windows;


namespace SLControls.Editors
{
    public class DropDownBox : ContentEditItem
    {
        public DropDownBox()
        {
            this.DefaultStyleKey = typeof(DropDownBox);
            
            GotFocus += DropDownTextBox_GotFocus;
            LostFocus += DropDownTextBox_LostFocus;
        }

        void DropDownTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
//            IsDropDownOpen = false;
        }

        void DropDownTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DropDownOnFocus)
            {
                IsDropDownOpen = true;
            }
        }

        public static readonly DependencyProperty ContentTextProperty =
                DependencyProperty.Register("ContentText", typeof (String), typeof (DropDownBox), new PropertyMetadata(default(String)));

        public String ContentText
        {
            get
            {
                return (String) GetValue(ContentTextProperty);
            }
            set
            {
                SetValue(ContentTextProperty, value);
            }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
                DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DropDownBox), new PropertyMetadata(default(bool)));

        public bool IsDropDownOpen
        {
            get
            {
                return (bool)GetValue(IsDropDownOpenProperty);
            }
            set
            {
                SetValue(IsDropDownOpenProperty, value);
            }
        }

        public static readonly DependencyProperty DropDownContentProperty =
                DependencyProperty.Register("DropDownContent", typeof (object), typeof (DropDownBox), new PropertyMetadata(default(object)));

        public object DropDownContent
        {
            get
            {
                return (object) GetValue(DropDownContentProperty);
            }
            set
            {
                SetValue(DropDownContentProperty, value);
            }
        }

        public static readonly DependencyProperty DropDownTemplateProperty =
                DependencyProperty.Register("DropDownTemplate", typeof (DataTemplate), typeof (DropDownBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate DropDownTemplate
        {
            get
            {
                return (DataTemplate) GetValue(DropDownTemplateProperty);
            }
            set
            {
                SetValue(DropDownTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty DropDownOnFocusProperty =
                DependencyProperty.Register("DropDownOnFocus", typeof (bool), typeof (DropDownBox), new PropertyMetadata(default(bool)));

        public bool DropDownOnFocus
        {
            get
            {
                return (bool) GetValue(DropDownOnFocusProperty);
            }
            set
            {
                SetValue(DropDownOnFocusProperty, value);
            }
        }

        public static readonly DependencyProperty DropDownWidthProperty =
                DependencyProperty.Register("DropDownWidth", typeof(double), typeof(DropDownBox), new PropertyMetadata(double.NaN));

        public double DropDownWidth
        {
            get
            {
                return (double)GetValue(DropDownWidthProperty);
            }
            set
            {
                SetValue(DropDownWidthProperty, value);
            }
        }

        public static readonly DependencyProperty DropDownHeightProperty =
                DependencyProperty.Register("DropDownHeight", typeof (double), typeof (DropDownBox), new PropertyMetadata(double.NaN));

        public double DropDownHeight
        {
            get
            {
                return (double) GetValue(DropDownHeightProperty);
            }
            set
            {
                SetValue(DropDownHeightProperty, value);
            }
        }
    }
}
