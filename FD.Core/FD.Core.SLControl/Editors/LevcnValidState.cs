using System.Windows;
using System.Windows.Controls;


namespace SLControls.Editors
{
    public class BaseLevcnValidTooltip : Control
    {
        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

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
        public static readonly DependencyProperty ErrorMsgProperty =
                DependencyProperty.Register("ErrorMsg", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

        public string ErrorMsg
        {
            get
            {
                return (string)GetValue(ErrorMsgProperty);
            }
            set
            {
                SetValue(ErrorMsgProperty, value);
                if (State == LevcnValidState.Error) Text = value;
            }
        }

        public static readonly DependencyProperty PassMsgProperty =
                DependencyProperty.Register("PassMsg", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

        public string PassMsg
        {
            get
            {
                return (string) GetValue(PassMsgProperty);
            }
            set
            {
                SetValue(PassMsgProperty, value);
                if (State == LevcnValidState.Pass) Text = value;
            }
        }

        public static readonly DependencyProperty FocusMsgProperty =
                DependencyProperty.Register("FocusMsg", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

        public string FocusMsg
        {
            get
            {
                return (string) GetValue(FocusMsgProperty);
            }
            set
            {
                SetValue(FocusMsgProperty, value);
                if (State == LevcnValidState.Focus) Text = value;
            }
        }

        public static readonly DependencyProperty DefaultMsgProperty =
                DependencyProperty.Register("DefaultMsg", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

        public string DefaultMsg
        {
            get
            {
                return (string) GetValue(DefaultMsgProperty);
            }
            set
            {
                SetValue(DefaultMsgProperty, value);
                if (State == (int)LevcnValidState.Default) Text = value;
            }
        }

        public static readonly DependencyProperty RequiredErrorMsgProperty =
                DependencyProperty.Register("RequiredErrorMsg", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

        public string RequiredErrorMsg
        {
            get
            {
                return (string) GetValue(RequiredErrorMsgProperty);
            }
            set
            {
                SetValue(RequiredErrorMsgProperty, value);
            }
        }

        public static readonly DependencyProperty OverMaxLengthErrorMsgProperty =
                DependencyProperty.Register("OverMaxLengthErrorMsg", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

        public string OverMaxLengthErrorMsg
        {
            get
            {
                return (string) GetValue(OverMaxLengthErrorMsgProperty);
            }
            set
            {
                SetValue(OverMaxLengthErrorMsgProperty, value);
            }
        }

        public static readonly DependencyProperty OverMaxErrorMsgProperty =
                DependencyProperty.Register("OverMaxErrorMsg", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

        public string OverMaxErrorMsg
        {
            get
            {
                return (string) GetValue(OverMaxErrorMsgProperty);
            }
            set
            {
                SetValue(OverMaxErrorMsgProperty, value);
            }
        }

        public static readonly DependencyProperty LowMinErrorMsgProperty =
                DependencyProperty.Register("LowMinErrorMsg", typeof(string), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(string)));

        public string LowMinErrorMsg
        {
            get
            {
                return (string) GetValue(LowMinErrorMsgProperty);
            }
            set
            {
                SetValue(LowMinErrorMsgProperty, value);
            }
        }
//        public string  { get; set; }
//        public string  { get; set; }
//        public string  { get; set; }
//        public string  { get; set; }
//        public string  { get; set; }

        public void CheckMsg()
        {

        }

//        public virtual LevcnValidState State { get; set; }
        public static readonly DependencyProperty StateProperty =
                DependencyProperty.Register("State", typeof(LevcnValidState), typeof(BaseLevcnValidTooltip), new PropertyMetadata(default(LevcnValidState),
                        (s, e) =>
                        {
                            ((BaseLevcnValidTooltip)s).SetState((LevcnValidState)e.NewValue);
                        }));

        public virtual LevcnValidState State
        {
            get
            {
                return (LevcnValidState)GetValue(StateProperty);
            }
            set
            {
                SetValue(StateProperty, value);
//                SetState(value);
            }
        }

        protected virtual void SetState(LevcnValidState _state)
        {
            
        }
    }
    /// <summary>
    /// 验证状态
    /// </summary>
    public enum LevcnValidState : byte
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        Default = 0,
        /// <summary>
        /// 焦点状态
        /// </summary>
        Focus = 1,
        /// <summary>
        /// 错误状态
        /// </summary>
        Error = 2,
        /// <summary>
        /// 未写内容
        /// </summary>
        Required = 3,
        /// <summary>
        /// 超出最大长度
        /// </summary>
        OverLength = 4,
        /// <summary>
        /// 超出最大值
        /// </summary>
        OverMax = 5,
        /// <summary>
        /// 小于最小值
        /// </summary>
        LowMin = 6,
        /// <summary>
        /// 验证通过状态
        /// </summary>
        Pass = 7,
    }
}
