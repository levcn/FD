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
using SLControls.Editors;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Animation;


namespace FD.Core.SLControl.Editors
{
    /// <summary>
    /// 搜索|高级搜索切换
    /// </summary>
    public class ListDownPanel : BaseMultiControl
    {
        public ListDownPanel()
        {
            this.DefaultStyleKey = typeof(ListDownPanel);
        }

        public static readonly DependencyProperty CurrentButtonContentProperty =
                DependencyProperty.Register("CurrentButtonContent", typeof(object), typeof(ListDownPanel), new PropertyMetadata("展开>>", (s, e) =>
                {
                    ((ListDownPanel) s).OnCurrentButtonContentChanged(e.NewValue as object);
                }));

        private void OnCurrentButtonContentChanged(object list)
        {}

        public object CurrentButtonContent
        {
            get
            {
                return (object) GetValue(CurrentButtonContentProperty);
            }
            set
            {
                SetValue(CurrentButtonContentProperty, value);
            }
        }
        public static readonly DependencyProperty OpenedButtonContentProperty =
                DependencyProperty.Register("OpenedButtonContent", typeof (object), typeof (ListDownPanel), new PropertyMetadata("收缩<<", (s, e) =>
                {
                    ((ListDownPanel) s).OnOpenedContentChanged(e.NewValue as object);
                }));

        private void OnOpenedContentChanged(object list)
        {
            InitCurrentButtonContent();
        }

        void InitCurrentButtonContent()
        {
            if (DetailVisibility == Visibility)
            {
                CurrentButtonContent = OpenedButtonContent;
            }
            else
            {
                CurrentButtonContent = ClosedButtonContent;

            }
        }
        /// <summary>
        /// 打开以后要显示的文本
        /// </summary>
        public object OpenedButtonContent
        {
            get
            {
                return (object) GetValue(OpenedButtonContentProperty);
            }
            set
            {
                SetValue(OpenedButtonContentProperty, value);
            }
        }
        public static readonly DependencyProperty ClosedButtonContentProperty =
                DependencyProperty.Register("ClosedButtonContent", typeof(object), typeof(ListDownPanel), new PropertyMetadata("展开>>", (s, e) =>
                {
                    ((ListDownPanel) s).OnClosedContentChanged(e.NewValue as object);
                }));

        private void OnClosedContentChanged(object list)
        {
            InitCurrentButtonContent();
        }

        public static readonly DependencyProperty DetailVisibilityProperty =
                DependencyProperty.Register("DetailVisibility", typeof(Visibility), typeof(ListDownPanel), new PropertyMetadata(Visibility.Collapsed, (s, e) =>
                {
                    ((ListDownPanel) s).OnDetailVisibilityChanged(e.NewValue is Visibility ? (Visibility) e.NewValue : Visibility.Visible);
                }));

        /// <summary>
        /// 关闭动画结束
        /// </summary>
        protected virtual void OnCloseEnd()
        {
            if (PART_HiddenContent != null) PART_HiddenContent.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 打开动画结束
        /// </summary>
        protected virtual void OnOpenEnd()
        {
            if (PART_HiddenContent != null) PART_HiddenContent.Visibility = Visibility.Visible;
        }
        private void OnDetailVisibilityChanged(Visibility list)
        {
            if (list == Visibility)
            {
                if (PART_HiddenContent != null) PART_HiddenContent.Visibility = Visibility.Visible;
                PlayShowAnimation();
            }
            else
            {
                PlayCloseAnimation();
            }
            InitCurrentButtonContent();
        }

        public Visibility DetailVisibility
        {
            get
            {
                return (Visibility) GetValue(DetailVisibilityProperty);
            }
            set
            {
                SetValue(DetailVisibilityProperty, value);
            }
        }
        /// <summary>
        /// 关闭以后要显示的文本
        /// </summary>
        public object ClosedButtonContent
        {
            get
            {
                return (object) GetValue(ClosedButtonContentProperty);
            }
            set
            {
                SetValue(ClosedButtonContentProperty, value);
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_HiddenContent = GetTemplateChild("PART_HiddenContent") as ContentControl;
            PART_Button = GetTemplateChild("PART_Button") as ContentControl;
            if (PART_Button != null)
            {
                PART_Button.MouseLeftButtonUp += (s, e) =>
                {
                    DetailVisibility = DetailVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                    InitCurrentButtonContent();
                };
            }
        }

        public override void LoadConfig(string configStr)
        {
            
        }
        private const string ShowAnimationName = "Show";
        private const string HideAnimationName = "Hide";
        private void StopAllAnimations()
        {
            AnimationManager.StopIfRunning(this, ShowAnimationName);
            AnimationManager.StopIfRunning(this, HideAnimationName);
        }
        private void PlayShowAnimation()
        {
            this.StopAllAnimations();
            
            if (!AnimationManager.Play(this, ShowAnimationName, OnOpenEnd))
            {
//                this.OnShowAnimationFinished();
                OnOpenEnd();
            }
        }

        private void PlayCloseAnimation()
        {
            this.StopAllAnimations();
            if (!AnimationManager.Play(this, HideAnimationName, OnCloseEnd))
            {
//                this.OnCloseAnimationFinished();
                OnCloseEnd();
            }
        }
        public static readonly DependencyProperty DefaultContentProperty =
                DependencyProperty.Register("DefaultContent", typeof(object), typeof(ListDownPanel), new PropertyMetadata(default(object), (s, e) =>
                {
                    ((ListDownPanel)s).OnDefaultContentChanged(e.NewValue as object);
                }));

        private void OnDefaultContentChanged(object list)
        {}

        public object DefaultContent
        {
            get
            {
                return (object)GetValue(DefaultContentProperty);
            }
            set
            {
                SetValue(DefaultContentProperty, value);
            }
        }

        public static readonly DependencyProperty HiddenContentProperty =
                DependencyProperty.Register("HiddenContent", typeof(object), typeof(ListDownPanel), new PropertyMetadata(default(object), (s, e) =>
                {
                    ((ListDownPanel)s).OnHiddenContentChanged(e.NewValue as object);
                }));

        private ContentControl PART_HiddenContent;
        private ContentControl PART_Button;

        private void OnHiddenContentChanged(object list)
        {
//            RadDropDownButton d;
//            d.DropDownContent=
//            Dropdown
        }

        public object HiddenContent
        {
            get
            {
                return (object)GetValue(HiddenContentProperty);
            }
            set
            {
                SetValue(HiddenContentProperty, value);
            }
        }
    }
}
