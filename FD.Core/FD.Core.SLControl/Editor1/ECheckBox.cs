using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLControls.Editor1
{
    [TemplateVisualState(GroupName = "BrowsStates", Name = "Edit")]
    [TemplateVisualState(GroupName = "BrowsStates", Name = "Brows")]
    public class ECheckBox : CheckBox, IBrowseble
    {
        public ECheckBox()
        {
            DefaultStyleKey = typeof (ECheckBox);
            IsEnabledChanged += ECheckBox_IsEnabledChanged;
            Checked += ECheckBox_Checked;
        }

        protected override void OnClick()
        {
            if (IsBrowsMode) return;
            base.OnClick();
        }

        void ECheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        void ECheckBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public static readonly DependencyProperty IsBrowsModeProperty =
                DependencyProperty.Register("IsBrowsMode", typeof(bool), typeof(ECheckBox), new PropertyMetadata(default(bool), (s, e) =>
                {
                    
                    ((ECheckBox)s).OnIsBrowsModeChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnIsBrowsModeChanged(bool isBrows)
        {
            if (isBrows)
            {
                preEnabled = IsEnabled;
                IsEnabled = false;
                VisualStateManager.GoToState(this, "Brows", true);
            }
            else
            {
                IsEnabled = preEnabled;
                VisualStateManager.GoToState(this, "Edit", true);
            }
        }

        private bool preEnabled = true;
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
    }
}
