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
    [TemplateVisualState(GroupName = "BrowsStates", Name = "Edit")]
    [TemplateVisualState(GroupName = "BrowsStates", Name = "Brows")]
    public class EditItemText : Control, IBrowseble
    {
        public EditItemText()
        {
            this.DefaultStyleKey = typeof(EditItemText);
            this.BindingValidationError += CustomValidControl_BindingValidationError;
            GotFocus += CustomValidControl_GotFocus;
            LostFocus += CustomValidControl_LostFocus;
        }
        TextBox PART_TextBox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_TextBox = GetTemplateChild("CheckInDate") as TextBox;
            RefreshState();
        }

        public static readonly DependencyProperty IsBrowsModeProperty =
               DependencyProperty.Register("IsBrowsMode", typeof(bool), typeof(EditItemText), new PropertyMetadata(default(bool), (s, e) =>
               {
                   ((EditItemText)s).OnIsBrowsModeChanged(e.NewValue is bool ? (bool)e.NewValue : false);
               }));

        public static readonly DependencyProperty IsReadOnlyProperty =
                DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(EditItemText), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((EditItemText)s).OnIsReadOnlyChanged(e.NewValue is bool ? (bool) e.NewValue : false);
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
        private bool preEnabled = true;
        private void OnIsBrowsModeChanged(bool isBrows)
        {
            if (isBrows)
            {
                preEnabled = IsReadOnly;
                IsReadOnly = true;
//                VisualStateManager.GoToState(this, "Brows", false);
            }
            else
            {
                IsReadOnly = preEnabled;
//                VisualStateManager.GoToState(this, "Edit", false);
            }
            RefreshState();
        }

        private void RefreshState()
        {
            if (IsBrowsMode)
            {
                VisualStateManager.GoToState(this, "Brows", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Edit", false);
                
            }
        }

        public bool IsBrowsMode
        {
            get
            {
                return (bool)GetValue(IsBrowsModeProperty);
            }
            set
            {
                SetValue(IsBrowsModeProperty, value);
            }
        }
        void CustomValidControl_LostFocus(object sender, RoutedEventArgs e)
        {
//            this.RefreshState();
            VisualStateManager.GoToState(this, "Unfocused", true);
        }

        void CustomValidControl_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Focused", true);
//            if (Validation.GetHasError(this))
//            {
//                VisualStateManager.GoToState(this, "InvalidFocused", true);
//            }
            if (PART_TextBox != null) PART_TextBox.Focus();
//            var rewrw = this.IsFocusWithin();
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
                return uiElement.ParentOfType<EditItemText>() == this;
            else
                return false;
        }
        public static readonly DependencyProperty TooltipTextProperty =
                DependencyProperty.Register("TooltipText", typeof(string), typeof(EditItemText), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((EditItemText)s).OnTooltipTextChanged(e.NewValue as string);
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
        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof(string), typeof(EditItemText), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((EditItemText)s).OnDateTimeValueChanged(e.NewValue as string);
                }));

        private void OnDateTimeValueChanged(string list)
        { }

        public static readonly DependencyProperty TextBoxHeightProperty =
                DependencyProperty.Register("TextBoxHeight", typeof(double), typeof(EditItemText), new PropertyMetadata(30D, (s, e) =>
                {
                    ((EditItemText)s).OnTextBoxHeightChanged(e.NewValue is double ? (double)e.NewValue : 0);
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

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty MemoProperty =
                DependencyProperty.Register("Memo", typeof(string), typeof(EditItemText), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((EditItemText)s).OnMemoChanged(e.NewValue as string);
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
