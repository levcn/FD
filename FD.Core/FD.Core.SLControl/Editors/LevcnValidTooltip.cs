using System.Windows;
using System.Windows.Controls;


namespace SLControls.Editors
{
    [TemplateVisualState(GroupName = "ValidStates", Name = "Default")]
    [TemplateVisualState(GroupName = "ValidStates", Name = "Pass")]
    [TemplateVisualState(GroupName = "ValidStates", Name = "Error")]
    [TemplateVisualState(GroupName = "ValidStates", Name = "Focus")]
    public class LevcnValidTooltip : BaseLevcnValidTooltip
    {
        public LevcnValidTooltip()
        {
            this.DefaultStyleKey = typeof(LevcnValidTooltip);
        }

        private Panel LayoutRoot;
        private TextBlock TB_Msg;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayoutRoot = GetTemplateChild("LayoutRoot") as Panel;
            TB_Msg = GetTemplateChild("TB_Msg") as TextBlock;

        }

        public static readonly DependencyProperty FocusHiddenProperty =
                DependencyProperty.Register("FocusHidden", typeof(bool), typeof(LevcnValidTooltip), new PropertyMetadata(default(bool)));

        public bool FocusHidden
        {
            get
            {
                return (bool)GetValue(FocusHiddenProperty);
            }
            set
            {
                SetValue(FocusHiddenProperty, value);
            }
        }

        public static readonly DependencyProperty OnlyNotPassShowProperty =
                DependencyProperty.Register("OnlyNotPassShow", typeof(bool), typeof(LevcnValidTooltip), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 验证不通过显示
        /// </summary>
        public bool OnlyNotPassShow
        {
            get
            {
                return (bool)GetValue(OnlyNotPassShowProperty);
            }
            set
            {
                SetValue(OnlyNotPassShowProperty, value);
            }
        }
        public static readonly DependencyProperty BlurHiddenProperty =
                DependencyProperty.Register("BlurHidden", typeof(bool), typeof(LevcnValidTooltip), new PropertyMetadata(false));

        /// <summary>
        /// 失去焦点时隐藏
        /// </summary>
        public bool BlurHidden
        {
            get
            {
                return (bool)GetValue(BlurHiddenProperty);
            }
            set
            {
                SetValue(BlurHiddenProperty, value);
            }
        }

        public static readonly DependencyProperty OnlyPassShowProperty =
                DependencyProperty.Register("OnlyPassShow", typeof(bool), typeof(LevcnValidTooltip), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 验证通过才显示
        /// </summary>
        public bool OnlyPassShow
        {
            get
            {
                return (bool)GetValue(OnlyPassShowProperty);
            }
            set
            {
                SetValue(OnlyPassShowProperty, value);
            }
        }
        public static readonly DependencyProperty ShowIconProperty =
                DependencyProperty.Register("ShowIcon", typeof(bool), typeof(LevcnValidTooltip),
                                            new PropertyMetadata(false));

        /// <summary>
        /// 显示图标
        /// </summary>
        public bool ShowIcon
        {
            get
            {
                return (bool)GetValue(ShowIconProperty);
            }
            set
            {
                SetValue(ShowIconProperty, value);
            }
        }

        public static readonly DependencyProperty ShowTextProperty =
                DependencyProperty.Register("ShowText", typeof(bool), typeof(LevcnValidTooltip),
                                            new PropertyMetadata(true));

        public bool ShowText
        {
            get
            {
                return (bool)GetValue(ShowTextProperty);
            }
            set
            {
                SetValue(ShowTextProperty, value);
            }
        }

        private LevcnValidState state = LevcnValidState.Default;

//        public override LevcnValidState State
//        {
//            get
//            {
//                return state;
//            }
//            set
//            {
//                if (state != value)
//                {
//                    state = value;
//                    SetState(state);
//                }
//            }
//        }

        
        /// <summary>
        /// 设置控件的状态
        /// </summary>
        /// <param name="_state"></param>
        protected override void SetState(LevcnValidState _state)
        {
            switch (_state)
            {
                case LevcnValidState.Default:
                    VisualStateManager.GoToState(this, "Default", false);
                    Text = DefaultMsg;
                    break;
                case LevcnValidState.Pass:
                    VisualStateManager.GoToState(this, "Pass", false);
                    Text = PassMsg;
                    break;
                case LevcnValidState.Error:
                    VisualStateManager.GoToState(this, "Error", false);
                    Text = ErrorMsg;
                    break;
                case LevcnValidState.OverLength:
                    VisualStateManager.GoToState(this, "Error", false);
                    Text = OverMaxLengthErrorMsg;
                    break;
                case LevcnValidState.OverMax:
                    VisualStateManager.GoToState(this, "Error", false);
                    Text = OverMaxErrorMsg;
                    break;
                case LevcnValidState.LowMin:
                    VisualStateManager.GoToState(this, "Error", false);
                    Text = LowMinErrorMsg;
                    break;
                case LevcnValidState.Required:
                    VisualStateManager.GoToState(this, "Error", false);
                    Text = RequiredErrorMsg;
                    break;
                case LevcnValidState.Focus:
                    VisualStateManager.GoToState(this, "Focus", false);
                    Text = FocusMsg;
                    break;
            }
            if (!ShowIcon)
            {
//                LayoutRoot.Background = null;
                if (TB_Msg != null) TB_Msg.Margin = new Thickness(0, TB_Msg.Margin.Top, TB_Msg.Margin.Right, TB_Msg.Margin.Bottom);
            }
            if (!ShowText) Text = "  ";
            if (BlurHidden)
            {
                Visibility = _state == LevcnValidState.Focus ? Visibility.Visible : Visibility.Collapsed;
            }
            if (FocusHidden)
            {
                Visibility = _state == LevcnValidState.Focus ? Visibility.Collapsed : Visibility.Visible;
            }
            if (OnlyPassShow)
            {
                Visibility = _state == LevcnValidState.Pass ? Visibility.Visible : Visibility.Collapsed;
            }
            if (OnlyNotPassShow)
            {
                Visibility = _state == LevcnValidState.Pass ? Visibility.Collapsed : Visibility.Visible;
            }
            //TB_Msg.Visibility = ShowText ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
