using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Animation;


namespace SLControls.Editor1
{
    [TemplatePart(Name = "PART_OKBUTTON",Type = typeof(Button))]
    [TemplatePart(Name = "PART_CANCELBUTTON", Type = typeof(Button))]
    public class RadWindowLayer:RadWindow
    {
        public RadWindowLayer()
        {
            KeyDown += EItemsControl_KeyDown;
            AddHandler(KeyDownEvent, new KeyEventHandler(EItemsControl_KeyDown), false);
//           WindowStartupLocation = WindowStartupLocation.CenterOwner;
           HideMaximizeButton = true;
           HideMinimizeButton = true;
//           Foreground = new SolidColorBrush(Colors.White);
//           Background = new SolidColorBrush(Color.FromArgb(255, 180, 200, 255));
           ResizeMode = ResizeMode.CanMinimize;
           Width = 500;
           Height = 300;
           ModalBackground = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
           BorderBrush = new SolidColorBrush(Color.FromArgb(255, 102, 150, 255));
           this.DefaultStyleKey = typeof(RadWindowLayer);

        }
        void EItemsControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private Button PART_OKBUTTON;
        private Button PART_CANCELBUTTON;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_OKBUTTON = GetTemplateChild("PART_OKBUTTON") as Button;
            PART_CANCELBUTTON = GetTemplateChild("PART_CANCELBUTTON") as Button;
            if (PART_OKBUTTON != null) PART_OKBUTTON.Click += PART_OKBUTTON_Click;
            if (PART_CANCELBUTTON != null) PART_CANCELBUTTON.Click += PART_CANCELBUTTON_Click;
            AnimationManager.SetIsAnimationEnabled(this,false);
        }

        void PART_CANCELBUTTON_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        void PART_OKBUTTON_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        public static readonly DependencyProperty ControlVisibilityProperty =
                DependencyProperty.Register("ControlVisibility", typeof(Visibility), typeof(RadWindowLayer), new PropertyMetadata(Visibility.Visible, (s, e) =>
                {
                    ((RadWindowLayer) s).OnControlVisibilityChanged(e.NewValue is Visibility ? (Visibility) e.NewValue : Visibility.Visible);
                }));

        private void OnControlVisibilityChanged(Visibility list)
        {}

        public static readonly DependencyProperty FromSizeProperty =
                DependencyProperty.Register("FromSize", typeof (Size), typeof (RadWindowLayer), new PropertyMetadata(default(Size), (s, e) =>
                {
                    ((RadWindowLayer) s).OnFromSizeChanged(e.NewValue is Size ? (Size) e.NewValue : new Size());
                }));

        private void OnFromSizeChanged(Size list)
        {}

        public static readonly DependencyProperty ToSizeProperty =
                DependencyProperty.Register("ToSize", typeof (Size), typeof (RadWindowLayer), new PropertyMetadata(default(Size), (s, e) =>
                {
                    ((RadWindowLayer) s).OnToSizeChanged(e.NewValue is Size ? (Size) e.NewValue : new Size());
                }));

        private void OnToSizeChanged(Size list)
        {}

        public Size ToSize
        {
            get
            {
                return (Size) GetValue(ToSizeProperty);
            }
            set
            {
                SetValue(ToSizeProperty, value);
            }
        }
        public Size FromSize
        {
            get
            {
                return (Size) GetValue(FromSizeProperty);
            }
            set
            {
                SetValue(FromSizeProperty, value);
            }
        }
        public Visibility ControlVisibility
        {
            get
            {
                return (Visibility) GetValue(ControlVisibilityProperty);
            }
            set
            {
                SetValue(ControlVisibilityProperty, value);
            }
        }

        

        public static readonly DependencyProperty ToOpacityProperty =
                DependencyProperty.Register("ToOpacity", typeof (double), typeof (RadWindowLayer), new PropertyMetadata(default(double), (s, e) =>
                {
                    ((RadWindowLayer) s).OnToOpacityChanged(e.NewValue is double ? (double) e.NewValue : 0);
                }));

        private void OnToOpacityChanged(double list)
        {}

        public double ToOpacity
        {
            get
            {
                return (double) GetValue(ToOpacityProperty);
            }
            set
            {
                SetValue(ToOpacityProperty, value);
            }
        }
        private double _width, _height, _opacity;
        private bool isChangeSize = false;
        public void ChangeSize()
        {
            _width = Width;
            _height = Height;
            _opacity = Opacity;
            if (myStoryboard == null)
            {
                myStoryboard = new Storyboard();
                myStoryboard.Duration = new Duration(new TimeSpan(0,0,0,0,400));
                widthAnimation = new DoubleAnimation();
                heightAnimation = new DoubleAnimation();
                opacityAnimation = new DoubleAnimation();

                widthAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 400));
                heightAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 400));
                opacityAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 400));

                myStoryboard.Children.Add(widthAnimation);
                myStoryboard.Children.Add(heightAnimation);
                myStoryboard.Children.Add(opacityAnimation);

                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("Width"));
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("Height"));
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));


                Storyboard.SetTarget(widthAnimation, this);
                Storyboard.SetTarget(heightAnimation, this);
                Storyboard.SetTarget(opacityAnimation, this);
                myStoryboard.Completed += myStoryboard_Completed;
            }
            else
            {
//                if (myStoryboard.GetCurrentState() != ClockState.Stopped) myStoryboard.Stop();
            } 
            isChangeSize = true;
//            widthAnimation.EasingFunction = new ExponentialEase();
            widthAnimation.To = ToSize.Width;

//            heightAnimation.EasingFunction = new ExponentialEase();
            heightAnimation.To = ToSize.Height;

//            opacityAnimation.EasingFunction = new ExponentialEase();
            opacityAnimation.To = ToOpacity;
            
            myStoryboard.Begin();
            
        }

        void myStoryboard_Completed(object sender, EventArgs e)
        {
            if (isChangeSize)
            {
                OnChangeSizeCompleted();
            }
            else
            {
                OnRestorySizeCompleted();
            }
        }

        public event EventHandler<EventArgs> ChangeSizeCompleted;

        protected virtual void OnChangeSizeCompleted()
        {
            var handler = ChangeSizeCompleted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> RestorySizeCompleted;

        protected virtual void OnRestorySizeCompleted()
        {
            var handler = RestorySizeCompleted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void RestorySize()
        {
//            if (myStoryboard.GetCurrentState() != ClockState.Stopped)myStoryboard.Stop();
            if (myStoryboard==null)throw new Exception("未设置变化");
//            widthAnimation.EasingFunction = new CubicEase();
//            widthAnimation.From = Width;
            widthAnimation.To = _width;

//            heightAnimation.EasingFunction = new CubicEase();
            heightAnimation.To = _height;
//            heightAnimation.From = Height;

//            opacityAnimation.EasingFunction = new CubicEase();
            opacityAnimation.To = _opacity;
//            opacityAnimation.From = Opacity;

            myStoryboard.Begin();
            isChangeSize = false;
        }
        Storyboard myStoryboard;
        DoubleAnimation widthAnimation;
        DoubleAnimation heightAnimation;
        DoubleAnimation opacityAnimation;
    }
}
