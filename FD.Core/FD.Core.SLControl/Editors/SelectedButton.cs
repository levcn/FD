using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace SLControls.Editors
{
    [TemplateVisualState(GroupName = "SelectedStates", Name = "Selected")]
    [TemplateVisualState(GroupName = "SelectedStates", Name = "UnSelected")]
    public class SelectedButton : Button
    {
        public SelectedButton()
        {
            this.DefaultStyleKey = (object)typeof(SelectedButton);
            UpdateState();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateState();
        }
        public static readonly DependencyProperty SelectedProperty =
                DependencyProperty.Register("Selected", typeof(bool), typeof(SelectedButton), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((SelectedButton)s).OnSelectedChanged();
                }));
        public static readonly DependencyProperty SelectedBackgroundProperty =
               DependencyProperty.Register("SelectedBackground", typeof(Brush), typeof(SelectedButton), new PropertyMetadata(default(Brush), (s, e) =>
               {
                   ((SelectedButton)s).OnSelectedBackgroundChanged(e.NewValue as Brush);
               }));

        private void OnSelectedBackgroundChanged(Brush list)
        { }

        public Brush SelectedBackground
        {
            get
            {
                return (Brush)GetValue(SelectedBackgroundProperty);
            }
            set
            {

                SetValue(SelectedBackgroundProperty, value);
            }
        }
        public event EventHandler SelectedChanged;

        protected virtual void OnSelectedChanged()
        {
            UpdateState();
            var handler = SelectedChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public bool Selected
        {
            get
            {
                return (bool)GetValue(SelectedProperty);
            }
            set
            {
                SetValue(SelectedProperty, value);
            }
        }

        void UpdateState()
        {
            if (this.Selected && this.IsEnabled)
                VisualStateManager.GoToState(this, "Selected", false);
            else
                VisualStateManager.GoToState(this, "UnSelected", false);
        }
    }
}
