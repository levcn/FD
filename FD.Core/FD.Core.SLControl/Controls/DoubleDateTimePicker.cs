using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;


namespace FD.Core.SLControl.Controls
{
    public class DoubleDateTimePicker : Control
    {
        public DoubleDateTimePicker()
        {
            this.DefaultStyleKey = typeof(DoubleDateTimePicker);
            RadDateTimePicker d = new RadDateTimePicker();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_StartTime = GetTemplateChild("PART_StartTime") as RadCalendar;
            PART_EndTime = GetTemplateChild("PART_EndTime") as RadCalendar;
        }

        public static readonly DependencyProperty DateTimeFormatProperty =
                DependencyProperty.Register("DateTimeFormat", typeof (string), typeof (DoubleDateTimePicker), new PropertyMetadata("yyyy-MM-dd hh:mm", (s, e) =>
                {
                    ((DoubleDateTimePicker) s).OnDateTimeFormatChanged(e.NewValue as string);
                }));

        private void OnDateTimeFormatChanged(string list)
        {}

        public string DateTimeFormat
        {
            get
            {
                return (string) GetValue(DateTimeFormatProperty);
            }
            set
            {
                SetValue(DateTimeFormatProperty, value);
            }
        }
        public static readonly DependencyProperty DateTimeTextProperty =
                DependencyProperty.Register("DateTimeText", typeof (string), typeof (DoubleDateTimePicker), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((DoubleDateTimePicker) s).OnDateTimeTextChanged(e.NewValue as string);
                }));

        private void OnDateTimeTextChanged(string list)
        {}

        public string DateTimeText
        {
            get
            {
                var re = "";
                if (StartTime != null)
                {
                    re += StartTime.Value.ToString(DateTimeFormat);
                }
                else
                {
                    re += " ";
                }
                re += " 至 ";
                if (EndTime != null)
                {
                    re += EndTime.Value.ToString(DateTimeFormat);
                }
                else
                {
                    re += " ";
                }

                return re;
                //return (string) GetValue(DateTimeTextProperty);
            }
            private set
            {
                SetValue(DateTimeTextProperty, value);
            }
        }
        public static readonly DependencyProperty IsDropDownOpenProperty =
                DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DoubleDateTimePicker), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((DoubleDateTimePicker)s).OnIsDropDownOpenChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnIsDropDownOpenChanged(bool list)
        {}
        
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
        public static readonly DependencyProperty TextAlignmentProperty =
                DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(DoubleDateTimePicker), new PropertyMetadata(default(TextAlignment), (s, e) =>
                {
                    ((DoubleDateTimePicker)s).OnTextAlignmentChanged(e.NewValue is TextAlignment ? (TextAlignment) e.NewValue : TextAlignment.Left);
                }));

        private void OnTextAlignmentChanged(TextAlignment list)
        {}

        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TextAlignmentProperty);
            }
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }
        public static readonly DependencyProperty SelectionOnFocusProperty =
                DependencyProperty.Register("SelectionOnFocus", typeof(SelectionOnFocus), typeof(DoubleDateTimePicker), new PropertyMetadata(default(SelectionOnFocus), (s, e) =>
                {
                    ((DoubleDateTimePicker)s).OnSelectionOnFocusChanged(e.NewValue is SelectionOnFocus ? (SelectionOnFocus) e.NewValue : SelectionOnFocus.Unchanged);
                }));

        private void OnSelectionOnFocusChanged(SelectionOnFocus list)
        {}

        public SelectionOnFocus SelectionOnFocus
        {
            get
            {
                return (SelectionOnFocus)GetValue(SelectionOnFocusProperty);
            }
            set
            {
                SetValue(SelectionOnFocusProperty, value);
            }
        }
        public static readonly DependencyProperty DateTimeWatermarkContentProperty =
                DependencyProperty.Register("DateTimeWatermarkContent", typeof(object), typeof(DoubleDateTimePicker), new PropertyMetadata(default(object), (s, e) =>
                {
                    ((DoubleDateTimePicker)s).OnDateTimeWatermarkContentChanged(e.NewValue as object);
                }));

        private void OnDateTimeWatermarkContentChanged(object list)
        {}

        public object DateTimeWatermarkContent
        {
            get
            {
                return (object)GetValue(DateTimeWatermarkContentProperty);
            }
            set
            {
                SetValue(DateTimeWatermarkContentProperty, value);
            }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
                DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DoubleDateTimePicker), new PropertyMetadata(false, (s, e) =>
                {
                    ((DoubleDateTimePicker)s).OnIsReadOnlyChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnIsReadOnlyChanged(bool list)
        {}

        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }
        public static readonly DependencyProperty StartTimeProperty =
                DependencyProperty.Register("StartTime", typeof (DateTime?), typeof (DoubleDateTimePicker), new PropertyMetadata(default(DateTime?), (s, e) =>
                {
                    ((DoubleDateTimePicker) s).OnStartTimeChanged(e.NewValue as DateTime?);
                }));

        private void OnStartTimeChanged(DateTime? list)
        {
            DateTimeText = DateTimeText;
            ReInitSelectionRange();
        }

        private void ReInitSelectionRange()
        {
            if (PART_StartTime != null && EndTime!=null)
            {
                PART_StartTime.SelectableDateEnd = EndTime;
//                PART_StartTime.DisplayDateEnd = EndTime;
            }
            if (PART_EndTime != null && StartTime != null)
            {
                PART_EndTime.SelectableDateStart = StartTime;
//                PART_EndTime.DisplayDateStart = StartTime;
            }
        }

        public DateTime? StartTime
        {
            get
            {
                return (DateTime?) GetValue(StartTimeProperty);
            }
            set
            {
                SetValue(StartTimeProperty, value);
                DateTimeText = DateTimeText;
            }
        }

        public static readonly DependencyProperty EndTimeProperty =
                DependencyProperty.Register("EndTime", typeof (DateTime?), typeof (DoubleDateTimePicker), new PropertyMetadata(default(DateTime?), (s, e) =>
                {
                    ((DoubleDateTimePicker) s).OnEndTimeChanged(e.NewValue as DateTime?);
                }));

        private RadCalendar PART_StartTime;
        private RadCalendar PART_EndTime;

        private void OnEndTimeChanged(DateTime? list)
        {
            DateTimeText = DateTimeText;
            ReInitSelectionRange();
        }

        public DateTime? EndTime
        {
            get
            {
                return (DateTime?) GetValue(EndTimeProperty);
            }
            set
            {
                SetValue(EndTimeProperty, value);
                DateTimeText = DateTimeText;
            }
        }
    }
}
