using System;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Calendar;


namespace SLControls.Editors
{

    [TemplateVisualState(GroupName = "DateTimeMode", Name = "OneDateTime")]
    [TemplateVisualState(GroupName = "DateTimeMode", Name = "TwoDateTime")]
    public class EditItemDateTimePicker : ContentEditItem
    {
        public EditItemDateTimePicker()
        {

            this.DefaultStyleKey = typeof(EditItemDateTimePicker);
        }

        private RadDateTimePicker dateTimePicker1;
        private RadDateTimePicker dateTimePicker2;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            dateTimePicker1 = GetTemplateChild("TB_TextBox1") as RadDateTimePicker;
            dateTimePicker2 = GetTemplateChild("TB_TextBox2") as RadDateTimePicker;
            if (dateTimePicker1 != null)
            {
                dateTimePicker1.GotFocus += dateTimePicker1_GotFocus;
            }
            if (dateTimePicker2 != null)
            {
                dateTimePicker2.GotFocus += dateTimePicker2_GotFocus;
               
            }
        }

        private void dateTimePicker2_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as RadDateTimePicker).IsDropDownOpen = true;
        }

        void dateTimePicker1_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as RadDateTimePicker).IsDropDownOpen = true;
        }

        public static readonly DependencyProperty InputModeProperty =
                DependencyProperty.Register("InputMode", typeof (InputMode), typeof (EditItemDateTimePicker), new PropertyMetadata(default(InputMode)));

        /// <summary>
        /// 日期的输入模式(日期+时间,只有日期,只有时间)
        /// </summary>
        public InputMode InputMode
        {
            get
            {
                return (InputMode) GetValue(InputModeProperty);
            }
            set
            {
                SetValue(InputModeProperty, value);
            }
        }

        public static readonly DependencyProperty DisplayFormatProperty =
                DependencyProperty.Register("DisplayFormat", typeof (DateTimePickerFormat), typeof (EditItemDateTimePicker), new PropertyMetadata(default(DateTimePickerFormat)));

        /// <summary>
        /// 日期显示格式,长类型,简短类型
        /// </summary>
        public DateTimePickerFormat DisplayFormat
        {
            get
            {
                return (DateTimePickerFormat) GetValue(DisplayFormatProperty);
            }
            set
            {
                SetValue(DisplayFormatProperty, value);
            }
        }

        public static readonly DependencyProperty SelectableDateStartProperty =
                DependencyProperty.Register("SelectableDateStart", typeof (DateTime?), typeof (EditItemDateTimePicker), new PropertyMetadata(default(DateTime?)));

        /// <summary>
        /// 可以选择的日期开始时间
        /// </summary>
        public DateTime? SelectableDateStart
        {
            get
            {
                return (DateTime?) GetValue(SelectableDateStartProperty);
            }
            set
            {
                SetValue(SelectableDateStartProperty, value);
            }
        }

        public static readonly DependencyProperty SelectableDateEndProperty =
                DependencyProperty.Register("SelectableDateEnd", typeof (DateTime?), typeof (EditItemDateTimePicker), new PropertyMetadata(default(DateTime?)));
        /// <summary>
        /// 可以选择的日期结束时间
        /// </summary>
        public DateTime? SelectableDateEnd
        {
            get
            {
                return (DateTime?) GetValue(SelectableDateEndProperty);
            }
            set
            {
                SetValue(SelectableDateEndProperty, value);
            }
        }

        public static readonly DependencyProperty SelectionOnFocusProperty =
                DependencyProperty.Register("SelectionOnFocus", typeof (SelectionOnFocus), typeof (EditItemDateTimePicker), new PropertyMetadata(default(SelectionOnFocus)));

        public SelectionOnFocus SelectionOnFocus
        {
            get
            {
                return (SelectionOnFocus) GetValue(SelectionOnFocusProperty);
            }
            set
            {
                SetValue(SelectionOnFocusProperty, value);
            }
        }

        public static readonly DependencyProperty DateSelectionModeProperty =
                DependencyProperty.Register("DateSelectionMode", typeof (DateSelectionMode), typeof (EditItemDateTimePicker), new PropertyMetadata(default(DateSelectionMode)));

        /// <summary>
        /// 日期选择模式(年,月,日)
        /// </summary>
        public DateSelectionMode DateSelectionMode
        {
            get
            {
                return (DateSelectionMode) GetValue(DateSelectionModeProperty);
            }
            set
            {
                SetValue(DateSelectionModeProperty, value);
            }
        }

        public static readonly DependencyProperty TimeIntervalProperty =
                DependencyProperty.Register("TimeInterval", typeof (TimeSpan), typeof (EditItemDateTimePicker), new PropertyMetadata(default(TimeSpan)));

        /// <summary>
        /// 小时部分的可选择间隔
        /// </summary>
        public TimeSpan TimeInterval
        {
            get
            {
                return (TimeSpan) GetValue(TimeIntervalProperty);
            }
            set
            {
                SetValue(TimeIntervalProperty, value);
            }
        }

        public static readonly DependencyProperty DateTimeModeProperty =
                DependencyProperty.Register("DateTimeMode", typeof (DateTimeMode), typeof (EditItemDateTimePicker), new PropertyMetadata(DateTimeMode.OneDateTime,
                    (s,e)=> {
                                ((EditItemDateTimePicker) s).OnDateTimeModeChanged((DateTimeMode)e.NewValue);
                    }));

        private void OnDateTimeModeChanged(DateTimeMode dateTimeMode)
        {
            var s = dateTimeMode.ToString();
            VisualStateManager.GoToState(this, s, false);
        }

        public DateTimeMode DateTimeMode
        {
            get
            {
                return (DateTimeMode) GetValue(DateTimeModeProperty);
            }
            set
            {
                SetValue(DateTimeModeProperty, value);
            }
        }
//        public static readonly DependencyProperty AnotherVisibilityProperty =
//                DependencyProperty.Register("AnotherVisibility", typeof(Visibility), typeof(EditItemDateTimePicker), new PropertyMetadata(Visibility.Collapsed));
//
//        /// <summary>
//        /// 另外一个日期是否显示
//        /// </summary>
//        public Visibility AnotherVisibility
//        {
//            get
//            {
//                return (Visibility) GetValue(AnotherVisibilityProperty);
//            }
//            set
//            {
//                SetValue(AnotherVisibilityProperty, value);
//            }
//        }

        public static readonly DependencyProperty DateTimeValueProperty =
                DependencyProperty.Register("DateTimeValue", typeof(DateTime?), typeof(EditItemDateTimePicker), new PropertyMetadata(default(DateTime?)));

        public DateTime? DateTimeValue
        {
            get
            {
                return (DateTime?)GetValue(DateTimeValueProperty);
            }
            set
            {
                SetValue(DateTimeValueProperty, value);
            }
        }

        public static readonly DependencyProperty AnotherDateTimeValueProperty =
                DependencyProperty.Register("AnotherDateTimeValue", typeof(DateTime?), typeof(EditItemDateTimePicker), new PropertyMetadata(default(string)));

        /// <summary>
        /// 另外一个日期的数据
        /// </summary>
        public DateTime? AnotherDateTimeValue
        {
            get
            {
                return (DateTime?)GetValue(AnotherDateTimeValueProperty);
            }
            set
            {
                SetValue(AnotherDateTimeValueProperty, value);
            }
        }
    }
    /// <summary>
    /// 日期选择方式
    /// </summary>
    public enum DateTimeMode
    {
        /// <summary>
        /// 一个日期
        /// </summary>
        OneDateTime,

        /// <summary>
        /// 日期范围
        /// </summary>
        TwoDateTime
    }
}
