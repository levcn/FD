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
using Telerik.Windows;
using Telerik.Windows.Controls;


namespace FD.Core.Test.Views.Sys
{
    public partial class SilverlightControl1 : UserControl
    {
        public SilverlightControl1()
        {
            InitializeComponent();
        }
    }
    public abstract class BehaviorBase
    {
        protected object AssociatedObject
        {
            get;
            private set;
        }

        public void Attach(object obj)
        {
            if (obj != AssociatedObject)
            {
                AssociatedObject = obj;
                OnAttached();
            }
        }

        protected virtual void OnAttached()
        {
        }
    }
    public abstract class Behavior<T> : BehaviorBase
    {
        protected new T AssociatedObject
        {
            get
            {
                return (T)base.AssociatedObject;
            }
        }
    }
    public class ReturnKeyBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeftButtonDown += OnAssociatedObjectKeyDown;
        }

        void OnAssociatedObjectKeyDown(object sender, MouseEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox != null)
            {
                MessageBox.Show(txtBox.Text);
            }
        }
    }

    public static class BehaviorService
    {
        public static readonly DependencyProperty BehaviorProperty = DependencyPropertyExtensions.RegisterAttached("Behavior",
            typeof(BehaviorBase),
            typeof(BehaviorService),
            new FrameworkPropertyMetadata(null));
        public static void SetBehavior(DependencyObject obj, BehaviorBase value)
        {
            value.Attach(obj);
            obj.SetValue(BehaviorProperty,value);
        }
        public static BehaviorBase GetBehavior(DependencyObject element)
        {
            return (BehaviorBase) element.GetValue(BehaviorProperty);
        }

        public static readonly DependencyProperty MouseDown111Property = DependencyPropertyExtensions.RegisterAttached("MouseDown111",
            typeof(String),
            typeof(EventService),
            new FrameworkPropertyMetadata(null));
        public static void SetMouseDown111(DependencyObject obj, string value)
        {
            FrameworkElement frameworkElement = obj as FrameworkElement;
            if (frameworkElement != null)
            {
                WeakEventListener<FrameworkElement, object, EventArgs> mouseEnterListener = new WeakEventListener<FrameworkElement, object, EventArgs>(frameworkElement);
                mouseEnterListener.OnEventAction = (instance, source, eventArgs) =>
                {
//                    value.Execute(null);
                };
                mouseEnterListener.OnDetachAction = (weakEventListener) => frameworkElement.Loaded -= mouseEnterListener.OnEvent;
                frameworkElement.Loaded += mouseEnterListener.OnEvent;
            }
            obj.SetValue(MouseDown111Property, value);
        }

        private static void d(object sender, MouseButtonEventArgs e)
        { }

        public static String GetMouseDown111(DependencyObject element)
        {
            return (String)element.GetValue(MouseDown111Property);
        }
    }
}
