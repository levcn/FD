//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using SLControls.Editors;
//using Telerik.Windows.Controls;
//
//
//namespace SLControls.Controls
//{
//    [TemplateVisualState(GroupName = "EditStates", Name = "Normal")]//正常
//    [TemplateVisualState(GroupName = "EditStates", Name = "Selectable")]//可以选择
//    [TemplateVisualState(GroupName = "EditStates", Name = "Selected")]//已经选择
//    public class BaseRichControl : BaseControl
//    {
//        public BaseRichControl()
//        {
//            this.DefaultStyleKey = typeof(BaseRichControl);
//
//            MouseEnter += BaseControl_MouseEnter;
//            MouseLeave += BaseControl_MouseLeave;
//            AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(EditClick), true);
//            Edit += BaseRichControl_Edit;
//        }
//
//        void BaseRichControl_Edit(object sender, MouseButtonEventArgs e)
//        {
//            BaseWindow rw = new BaseWindow();
//            rw.Height = 500;
//            rw.Width = 600;
//            rw.Content = new ControlEditPanel {Item = this};
//            rw.ShowDialog();
//            rw.Closed += rw_Closed;
//        }
//
//        void rw_Closed(object sender, WindowClosedEventArgs e)
//        {
//            this.EditState = EditState.Normal;
//        }
//        public virtual List<string> EditablePropters
//        {
//            get
//            {
//                return new List<string> { "Width", "Height" };
//            }
//        }
//        public override void OnApplyTemplate()
//        {
//            base.OnApplyTemplate();
//            EditButton = GetTemplateChild("EditButton") as Button;
//            if (EditButton != null) EditButton.Click += EditButton_Click;
//        }
//
//        void EditButton_Click(object sender, RoutedEventArgs e)
//        {
//            OnEdit(null);
//        }
//        public event MouseButtonEventHandler Edit;
//
//        protected virtual void OnEdit(MouseButtonEventArgs e)
//        {
//            MouseButtonEventHandler handler = Edit;
//            if (handler != null) handler(this, e);
//        }
//
//        public event MouseButtonEventHandler EditStateChanged;
//
//        protected virtual void OnEditStateChanged(MouseButtonEventArgs e)
//        {
//            MouseButtonEventHandler handler = EditStateChanged;
//            if (handler != null) handler(this, e);
//        }
//
//        private void EditClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
//        {
//            if (EditState == EditState.Hover)
//            {
//                EditState = EditState.Focus;
//            }
//        }
//
//        void BaseControl_MouseLeave(object sender, MouseEventArgs e)
//        {
//            if (EditState == EditState.Hover)
//            {
//                EditState = EditState.Normal;
//            }
//        }
//
//        void BaseControl_MouseEnter(object sender, MouseEventArgs e)
//        {
//            if (EditState == EditState.Normal)
//            {
//                EditState = EditState.Hover;
//            }
//        }
//
//        public static readonly DependencyProperty SelectBorderWidthProperty =
//                DependencyProperty.Register("SelectBorderWidth", typeof(Thickness), typeof(BaseRichControl), new PropertyMetadata(default(Thickness)));
//
//        /// <summary>
//        /// 选择边框的宽度
//        /// </summary>
//        public Thickness SelectBorderWidth
//        {
//            get
//            {
//                return (Thickness)GetValue(SelectBorderWidthProperty);
//            }
//            set
//            {
//                SetValue(SelectBorderWidthProperty, value);
//            }
//        }
//
//        public static readonly DependencyProperty SelectBorderColorProperty =
//                DependencyProperty.Register("SelectBorderColor", typeof(Brush), typeof(BaseRichControl), new PropertyMetadata(null));
//
//        /// <summary>
//        /// 选择边框的颜色
//        /// </summary>
//        public Brush SelectBorderColor
//        {
//            get
//            {
//                return (Brush)GetValue(SelectBorderColorProperty);
//            }
//            set
//            {
//                SetValue(SelectBorderColorProperty, value);
//            }
//        }
//
//        public static readonly DependencyProperty HoverBorderColorProperty =
//                DependencyProperty.Register("HoverBorderColor", typeof(Brush), typeof(BaseRichControl), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));
//
//        public Brush HoverBorderColor
//        {
//            get
//            {
//                return (Brush)GetValue(HoverBorderColorProperty);
//            }
//            set
//            {
//                SetValue(HoverBorderColorProperty, value);
//            }
//        }
//
//        public static readonly DependencyProperty HoverBorderWidthProperty =
//                DependencyProperty.Register("HoverBorderWidth", typeof(Thickness), typeof(BaseRichControl), new PropertyMetadata(new Thickness(1)));
//
//        public Thickness HoverBorderWidth
//        {
//            get
//            {
//                return (Thickness)GetValue(HoverBorderWidthProperty);
//            }
//            set
//            {
//                SetValue(HoverBorderWidthProperty, value);
//            }
//        }
//
//        public static readonly DependencyProperty EditBorderWidthProperty =
//                DependencyProperty.Register("EditBorderWidth", typeof(Thickness), typeof(BaseRichControl), new PropertyMetadata(new Thickness(2)));
//
//        [Editable(GroupName = "基本属性", DisplayName = "EditBorderWidth", Description = "EditBorderWidthxxxx。")]
//        public Thickness EditBorderWidth
//        {
//            get
//            {
//                return (Thickness)GetValue(EditBorderWidthProperty);
//            }
//            set
//            {
//                SetValue(EditBorderWidthProperty, value);
//            }
//        }
//
//        public static readonly DependencyProperty EditBorderColorProperty =
//                DependencyProperty.Register("EditBorderColor", typeof(Brush), typeof(BaseRichControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
//
//        public Brush EditBorderColor
//        {
//            get
//            {
//                return (Brush)GetValue(EditBorderColorProperty);
//            }
//            set
//            {
//                SetValue(EditBorderColorProperty, value);
//            }
//        }
//        public static readonly DependencyProperty EditStateProperty =
//                DependencyProperty.Register("EditState", typeof(EditState), typeof(BaseRichControl), new PropertyMetadata(EditState.Normal, (s, e) =>
//                {
//                    BaseRichControl control = ((BaseRichControl)s);
//                    EditState editState = (EditState)e.NewValue;
//                    switch (editState)
//                    {
//                        case EditState.Normal:
//                            control.SelectBorderWidth = default(Thickness);
//                            control.SelectBorderColor = null;
//                            VisualStateManager.GoToState(control, "Normal", true);
//                            break;
//                        case EditState.Hover:
//                            control.SelectBorderWidth = control.HoverBorderWidth;
//                            control.SelectBorderColor = control.HoverBorderColor;
//                            VisualStateManager.GoToState(control, "Selectable", true);
//
//                            break;
//                        case EditState.Focus:
//                            control.SelectBorderWidth = control.EditBorderWidth;
//                            control.SelectBorderColor = control.EditBorderColor;
//                            VisualStateManager.GoToState(control, "Selected", true);
//
//                            break;
//                        case EditState.Run:
//                            control.SelectBorderWidth = default(Thickness);
//                            control.SelectBorderColor = null;
//                            VisualStateManager.GoToState(control, "Normal", true);
//                            break;
//                    }
//                    control.OnEditStateChanged(null);
//                }));
//
//        private Button EditButton;
//
//        /// <summary>
//        /// 是否选择进行编辑
//        /// </summary>
//        public EditState EditState
//        {
//            get
//            {
//
//                return (EditState)GetValue(EditStateProperty);
//            }
//            set
//            {
//                SetValue(EditStateProperty, value);
//            }
//        }
//    }
//    public enum EditState
//    {
//        /// <summary>
//        /// 运行模式
//        /// </summary>
//        Run,
//        /// <summary>
//        /// 一般编辑模式
//        /// </summary>
//        Normal,
//        /// <summary>
//        /// 鼠标移上
//        /// </summary>
//        Hover,
//        /// <summary>
//        /// 鼠标点击获得焦点
//        /// </summary>
//        Focus,
//    }
//}

// WeakEventListener<FrameworkElement, object, MouseEventArgs> mouseEnterListener = new WeakEventListener<FrameworkElement, object, MouseEventArgs>(owner);
//            mouseEnterListener.OnEventAction = (instance, source, eventArgs) => OnMouseEnterElement(source, eventArgs);
//            mouseEnterListener.OnDetachAction = (weakEventListener) => owner.MouseEnter -= mouseEnterListener.OnEvent;
//            owner.MouseEnter += mouseEnterListener.OnEvent;
//
//            WeakEventListener<FrameworkElement, object, MouseEventArgs> mouseLeaveListener = new WeakEventListener<FrameworkElement, object, MouseEventArgs>(owner);
//            mouseLeaveListener.OnEventAction = (instance, source, eventArgs) => OnMouseLeaveElement(source, eventArgs);
//            mouseLeaveListener.OnDetachAction = (weakEventListener) => owner.MouseLeave -= mouseLeaveListener.OnEvent;
//            owner.MouseLeave += mouseLeaveListener.OnEvent;