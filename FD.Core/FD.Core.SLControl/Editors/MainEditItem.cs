using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace SLControls.Editors
{
    /// <summary>
    ///     编辑条目的基类
    /// </summary>
    [TemplateVisualState(GroupName = "BrowserStates", Name = "Browser")]
    [TemplateVisualState(GroupName = "BrowserStates", Name = "Editor")]
    public abstract class BaseEditItem : Control
    {
        #region 验证

        public static readonly DependencyProperty ValidStateProperty =
                DependencyProperty.Register("ValidState", typeof(LevcnValidState), typeof(BaseEditItem), new PropertyMetadata(default(LevcnValidState)));

        public LevcnValidState ValidState
        {
            get
            {
                return (LevcnValidState)GetValue(ValidStateProperty);
            }
            set
            {
                SetValue(ValidStateProperty, value);
            }
        }
        public static readonly DependencyProperty ErrorMsgProperty =
                DependencyProperty.Register("ErrorMsg", typeof(string), typeof(BaseEditItem), new PropertyMetadata(default(string)));

        public string ErrorMsg
        {
            get
            {
                return (string)GetValue(ErrorMsgProperty);
            }
            set
            {
                SetValue(ErrorMsgProperty, value);
            }
        }

        public static readonly DependencyProperty PassMsgProperty =
                DependencyProperty.Register("PassMsg", typeof(string), typeof(BaseEditItem), new PropertyMetadata(default(string)));

        public string PassMsg
        {
            get
            {
                return (string)GetValue(PassMsgProperty);
            }
            set
            {
                SetValue(PassMsgProperty, value);
            }
        }

        public static readonly DependencyProperty FocusMsgProperty =
                DependencyProperty.Register("FocusMsg", typeof(string), typeof(BaseEditItem), new PropertyMetadata(default(string)));

        public string FocusMsg
        {
            get
            {
                return (string)GetValue(FocusMsgProperty);
            }
            set
            {
                SetValue(FocusMsgProperty, value);
            }
        }

        public static readonly DependencyProperty DefaultMsgProperty =
                DependencyProperty.Register("DefaultMsg", typeof(string), typeof(BaseEditItem), new PropertyMetadata(default(string)));

        public string DefaultMsg
        {
            get
            {
                return (string)GetValue(DefaultMsgProperty);
            }
            set
            {
                SetValue(DefaultMsgProperty, value);
            }
        }

        public static readonly DependencyProperty RequiredErrorMsgProperty =
                DependencyProperty.Register("RequiredErrorMsg", typeof(string), typeof(BaseEditItem), new PropertyMetadata(default(string)));

        public string RequiredErrorMsg
        {
            get
            {
                return (string)GetValue(RequiredErrorMsgProperty);
            }
            set
            {
                SetValue(RequiredErrorMsgProperty, value);
            }
        }

        public static readonly DependencyProperty OverMaxLengthErrorMsgProperty =
                DependencyProperty.Register("OverMaxLengthErrorMsg", typeof(string), typeof(BaseEditItem), new PropertyMetadata(default(string)));

        public string OverMaxLengthErrorMsg
        {
            get
            {
                return (string)GetValue(OverMaxLengthErrorMsgProperty);
            }
            set
            {
                SetValue(OverMaxLengthErrorMsgProperty, value);
            }
        }

        public static readonly DependencyProperty OverMaxErrorMsgProperty =
                DependencyProperty.Register("OverMaxErrorMsg", typeof(string), typeof(BaseEditItem), new PropertyMetadata(default(string)));

        public string OverMaxErrorMsg
        {
            get
            {
                return (string)GetValue(OverMaxErrorMsgProperty);
            }
            set
            {
                SetValue(OverMaxErrorMsgProperty, value);
            }
        }

        public static readonly DependencyProperty LowMinErrorMsgProperty =
                DependencyProperty.Register("LowMinErrorMsg", typeof(string), typeof(BaseEditItem), new PropertyMetadata(default(string)));

        public string LowMinErrorMsg
        {
            get
            {
                return (string)GetValue(LowMinErrorMsgProperty);
            }
            set
            {
                SetValue(LowMinErrorMsgProperty, value);
            }
        } 

        #endregion
        public static readonly DependencyProperty ReadOnlyProperty =
                DependencyProperty.Register("ReadOnly", typeof(bool), typeof(BaseEditItem), new PropertyMetadata(false,
                                                                                                                   (s, e) => { ((BaseEditItem)s).OnReadOnlyChanged1(e.OldValue, e.NewValue); }));

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof(object), typeof(BaseEditItem),
                                            new PropertyMetadata(default(string),
                                                                 (s, e) => ((BaseEditItem)s).OnTextChanged1(e.OldValue, e.NewValue)));

        public static readonly DependencyProperty LabelTextProperty =
                DependencyProperty.Register("LabelText", typeof(string), typeof(BaseEditItem),
                                            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty RequiredVisibilityProperty =
                DependencyProperty.Register("RequiredVisibility", typeof(Visibility), typeof(BaseEditItem),
                                            new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty ValidateVisibilitiProperty =
                DependencyProperty.Register("ValidateVisibiliti", typeof (Visibility), typeof (BaseEditItem), new PropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty ViewModeProperty =
                DependencyProperty.Register("ViewMode", typeof (ViewMode), typeof (BaseEditItem), new PropertyMetadata(ViewMode.Editor, (s, e) =>
                {
                    ((BaseEditItem)s).OnViewModeChanged((ViewMode)e.NewValue);
                }));

        
        protected virtual void OnViewModeChanged(ViewMode newValue)
        {
                string stateName = newValue.ToString();
                VisualStateManager.GoToState(this, stateName, false);
        }

        public ViewMode ViewMode
        {
            get
            {
                return (ViewMode) GetValue(ViewModeProperty);
            }
            set
            {
                SetValue(ViewModeProperty, value);
            }
        }
        public Visibility ValidateVisibiliti
        {
            get
            {
                return (Visibility) GetValue(ValidateVisibilitiProperty);
            }
            set
            {
                SetValue(ValidateVisibilitiProperty, value);
            }
        }

        public static readonly DependencyProperty LableWidthProperty =
                DependencyProperty.Register("LableWidth", typeof (double), typeof (BaseEditItem), new PropertyMetadata(double.NaN));

        public double LableWidth
        {
            get
            {
                return (double) GetValue(LableWidthProperty);
            }
            set
            {
                SetValue(LableWidthProperty, value);
            }
        }
        public BaseEditItem()
        {
            Loaded += MainEditItem_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnViewModeChanged(ViewMode);
        }

        public bool ReadOnly
        {
            get
            {
                return (bool)GetValue(ReadOnlyProperty);
            }
            set
            {
                SetValue(ReadOnlyProperty, value);
            }
        }

        /// <summary>
        ///     返回验证控件
        /// </summary>
        public virtual List<BaseLevcnValidTooltip> ValidTooltip
        {
            get
            {
                return new List<BaseLevcnValidTooltip>();
            }
        }

        /// <summary>
        ///     返回*号控件
        /// </summary>
        public virtual TextBlock ItemLableRequired
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        ///     返回*号控件
        /// </summary>
        protected virtual FrameworkElement ContentControl
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        ///     是否显示必填*号
        /// </summary>
        public Visibility RequiredVisibility
        {
            get
            {
                return (Visibility)GetValue(RequiredVisibilityProperty);
            }
            set
            {
                SetValue(RequiredVisibilityProperty, value);
            }
        }

        /// <summary>
        ///     标签文本
        /// </summary>
        public string LabelText
        {
            get
            {
                return (string)GetValue(LabelTextProperty);
            }
            set
            {
                SetValue(LabelTextProperty, value);
            }
        }

        /// <summary>
        ///     值
        /// </summary>
        public virtual object Text
        {
            get
            {
                return GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public event TEventHandler<RoutedEventArgs> ReadOnlyChanged;

        /// <summary>
        ///     重新验证事件,值发重要变化,需要重新验证,由控件手工触发验证.
        /// </summary>
        public event TEventHandler<BaseEditItem, RoutedEventArgs> ReValidate;

        protected virtual void OnReValidate()
        {
            OnReValidate(new RoutedEventArgs());
        }

        protected virtual void OnReValidate(RoutedEventArgs e)
        {
            TEventHandler<BaseEditItem, RoutedEventArgs> handler = ReValidate;
            if (handler != null) handler(this, e);
        }

        private void OnReadOnlyChanged1(object oldValue, object newValue)
        {
            if (ReadOnlyChanged != null)
            {
                ReadOnlyChanged(new RoutedEventArgs());
            }
            OnReadOnlyChanged(oldValue, newValue);
        }

        protected virtual void OnReadOnlyChanged(object oldValue, object newValue)
        { }

        private void OnTextChanged1(object oldValue, object newValue)
        {
            OnChanged(new RoutedEventArgs());
            OnTextChanged(oldValue, newValue);
        }

        protected virtual void OnTextChanged(object oldValue, object newValue)
        { }

        /// <summary>
        ///     保存数据
        /// </summary>
        public virtual void Save()
        { }

        private void MainEditItem_Loaded(object sender, RoutedEventArgs e)
        {
            InitValid();
        }

        private void InitValid()
        {
            //var attr = GetValidInfo();
            //if(attr!=)
        }
//
//        /// <summary>
//        ///     返回当前属性的验证信息
//        /// </summary>
//        /// <returns></returns>
//        protected virtual LevcnValidateAttribute GetValidInfo()
//        {
//            BindingExpression expression = GetBindingExpression(TextProperty);
//            if (expression != null)
//            {
//                object obj = expression.DataItem;
//                string path = expression.ParentBinding.Path.Path;
//                return ReflectionHelper.GetPropertyAttribute<LevcnValidateAttribute>(path, obj);
//            }
//            return null;
//        }

        public event EventHandler<RoutedEventArgs> Changed;

        public void OnChanged(RoutedEventArgs e)
        {
            EventHandler<RoutedEventArgs> handler = Changed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     使用控件的值更新数据源
        /// </summary>
        public virtual void UpdateSource()
        {
            UpdateSourceItem(TextProperty);
        }
        /// <summary>
        /// 使用控件的值更新数据源
        /// </summary>
        /// <param name="dp"></param>
        protected void UpdateSourceItem(DependencyProperty dp)
        {
            BindingExpression expr = GetBindingExpression(dp);
            if (expr != null) expr.UpdateSource();
        }
    }
}
