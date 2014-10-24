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


namespace SLControls.Editor1
{
    [TemplateVisualState(GroupName = "FocusStates", Name = "Unfocused")]
    [TemplateVisualState(GroupName = "FocusStates", Name = "Focused")]
    [TemplateVisualState(GroupName = "ValidationStates", Name = "Valid")]
    [TemplateVisualState(GroupName = "ValidationStates", Name = "InvalidUnfocused")]
    [TemplateVisualState(GroupName = "ValidationStates", Name = "InvalidFocused")]
    public class EditItemDateTime : Control
    {
        public EditItemDateTime()
        {
            this.DefaultStyleKey = typeof(EditItemDateTime);
            this.BindingValidationError += CustomValidControl_BindingValidationError;
            GotFocus += CustomValidControl_GotFocus;
            LostFocus += CustomValidControl_LostFocus;
        }

        void CustomValidControl_LostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Unfocused", true);
        }

        void CustomValidControl_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Focused", true);
        }

        void CustomValidControl_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            RefreshTooltip();
        }

        private void RefreshTooltip()
        {
            if (Validation.GetHasError(this))
            {
                Memo = Validation.GetErrors(this)[0].ErrorContent.ToString();
            }
            else
            {
                Memo = TooltipText;
            }
        }

        private bool IsFocusWithin()
        {
            UIElement uiElement = FocusManagerHelper.GetFocusedElement((DependencyObject)this) as UIElement;
            if (uiElement != null)
                return uiElement.ParentOfType<EditItemDateTime>() == this;
            else
                return false;
        }
        public static readonly DependencyProperty TooltipTextProperty =
                DependencyProperty.Register("TooltipText", typeof(string), typeof(EditItemDateTime), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((EditItemDateTime)s).OnTooltipTextChanged(e.NewValue as string);
                }));

        private void OnTooltipTextChanged(string list)
        {
            RefreshTooltip();
        }

        public string TooltipText
        {
            get
            {
                return (string)GetValue(TooltipTextProperty);
            }
            set
            {
                SetValue(TooltipTextProperty, value);
            }
        }
        public static readonly DependencyProperty DateTimeValueProperty =
                DependencyProperty.Register("DateTimeValue", typeof(DateTime?), typeof(EditItemDateTime), new PropertyMetadata(default(DateTime?), (s, e) =>
                {
                    ((EditItemDateTime)s).OnDateTimeValueChanged(e.NewValue as DateTime?);
                }));

        private void OnDateTimeValueChanged(DateTime? list)
        { }

        public static readonly DependencyProperty TextBoxHeightProperty =
                DependencyProperty.Register("TextBoxHeight", typeof(double), typeof(EditItemDateTime), new PropertyMetadata(30D, (s, e) =>
                {
                    ((EditItemDateTime)s).OnTextBoxHeightChanged(e.NewValue is double ? (double)e.NewValue : 0);
                }));

        private void OnTextBoxHeightChanged(double list)
        { }

        public double TextBoxHeight
        {
            get
            {
                return (double)GetValue(TextBoxHeightProperty);
            }
            set
            {
                SetValue(TextBoxHeightProperty, value);
            }
        }

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

        public static readonly DependencyProperty MemoProperty =
                DependencyProperty.Register("Memo", typeof(string), typeof(EditItemDateTime), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((EditItemDateTime)s).OnMemoChanged(e.NewValue as string);
                }));

        private void OnMemoChanged(string list)
        {
            RefreshTooltip();
        }

        public string Memo
        {
            get
            {
                return (string)GetValue(MemoProperty);
            }
            set
            {
                SetValue(MemoProperty, value);
            }
        }
    }

}
