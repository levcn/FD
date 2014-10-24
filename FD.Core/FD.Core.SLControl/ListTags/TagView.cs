using System;
using System.Windows;
using System.Windows.Controls;


namespace SLControls.ListTags
{
    [TemplateVisualState(GroupName = "ViewTypeStates", Name = "NonSelection")]
    [TemplateVisualState(GroupName = "ViewTypeStates", Name = "SingleSelection")]
    [TemplateVisualState(GroupName = "ViewTypeStates", Name = "MultiSelection")]
    public class TagView : Control
    {
        public static readonly DependencyProperty SelectionTypeProperty =
                DependencyProperty.Register("SelectionType", typeof(TagViewViewTypeStates), typeof(TagView), new PropertyMetadata(TagViewViewTypeStates.MultiSelection,
                        (s, e) => { ((TagView) s).OnSelectionTypeChanged((TagViewViewTypeStates) e.NewValue); }));

        public static readonly DependencyProperty IsCheckedProperty =
                DependencyProperty.Register("IsChecked", typeof (bool?), typeof (TagView), new PropertyMetadata(false,
                        (s, e) =>
                        {
                            ((TagView) s).OnIsCheckedChanged((bool?)e.NewValue);
                        }));

        private void OnIsCheckedChanged(bool? isChanged)
        {
            if (SelectionType == TagViewViewTypeStates.MultiSelection)
            {
                CONTROL_CheckBox.IsChecked = isChanged;
            }
            if (SelectionType == TagViewViewTypeStates.SingleSelection)
            {
                CONTROL_RadioButton.IsChecked = isChanged;
            }
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof (string), typeof (TagView), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty GroupNameProperty =
                DependencyProperty.Register("GroupName", typeof (string), typeof (TagView), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty DeleteVisibilityProperty =
                DependencyProperty.Register("DeleteVisibility", typeof(Visibility), typeof(TagView), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 删除按钮的显示状态
        /// </summary>
        public Visibility DeleteVisibility
        {
            get
            {
                return (Visibility) GetValue(DeleteVisibilityProperty);
            }
            set
            {
                SetValue(DeleteVisibilityProperty, value);
            }
        }
        public event EventHandler<EventArgs> Deleted;

        protected virtual void OnDeleted()
        {
            EventHandler<EventArgs> handler = Deleted;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        private CheckBox CONTROL_CheckBox;
        private RadioButton CONTROL_RadioButton;
        private Button CONTROL_DeleteButton;

        public TagView()
        {
            DefaultStyleKey = typeof (TagView);
            VisualStateManager.GoToState(this, "SingleSelection", false);
        }

        public bool? IsChecked
        {
            get
            {
                return (bool?) GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

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

        /// <summary>
        /// 单选时,需要设置分组
        /// </summary>
        public string GroupName
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

        public TagViewViewTypeStates SelectionType
        {
            get
            {
                return (TagViewViewTypeStates) GetValue(SelectionTypeProperty);
            }
            set
            {
                SetValue(SelectionTypeProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CONTROL_CheckBox = GetTemplateChild("PART_CheckBox") as CheckBox;
            CONTROL_RadioButton = GetTemplateChild("PART_RadioButton") as RadioButton;
            CONTROL_DeleteButton = GetTemplateChild("PART_DeleteButton") as Button;
            if (CONTROL_DeleteButton != null) CONTROL_DeleteButton.Click += ControlDeleteButtonClick;
            if (CONTROL_CheckBox != null)
            {
                CONTROL_CheckBox.Checked += CONTROL_CheckBox_Checked;
                CONTROL_CheckBox.Unchecked += CONTROL_CheckBox_Unchecked;
            }
            if (CONTROL_RadioButton != null)
            {
                CONTROL_RadioButton.Checked += CONTROL_RadioButton_Checked;
                CONTROL_RadioButton.Unchecked += CONTROL_RadioButton_Unchecked;
            }
            SwitchState(SelectionType);
        }

        void CONTROL_RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SelectionType == TagViewViewTypeStates.SingleSelection)
            {
                IsChecked = false;
                OnCheckedChanged();
            }
        }

        void CONTROL_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectionType == TagViewViewTypeStates.SingleSelection)
            {
                IsChecked = true;
                OnCheckedChanged();
            }
        }

        private void CONTROL_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SelectionType == TagViewViewTypeStates.MultiSelection)
            {
                IsChecked = false;
                OnCheckedChanged();
            }
        }

        void CONTROL_CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (SelectionType == TagViewViewTypeStates.MultiSelection)
            {
                IsChecked = true;
                OnCheckedChanged();
            }
        }
        public event EventHandler CheckedChanged;

        protected virtual void OnCheckedChanged()
        {
            EventHandler handler = CheckedChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        void ControlDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            OnDeleted();
        }

        private void OnSelectionTypeChanged(TagViewViewTypeStates newValue)
        {
            SwitchState(newValue);
        }

        private void SwitchState(TagViewViewTypeStates newValue)
        {
            VisualStateManager.GoToState(this, newValue.ToString(), false);
        }
    }

    public enum TagViewViewTypeStates
    {
        NonSelection,
        SingleSelection,
        MultiSelection
    }
}