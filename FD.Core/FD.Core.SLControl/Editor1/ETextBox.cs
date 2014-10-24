using System;
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
    public class ETextBox : TextBox, IBrowseble
    {
        public ETextBox()
        {
            DefaultStyleKey = typeof (ETextBox);
        }

        public static readonly DependencyProperty IsBrowsModeProperty =
                DependencyProperty.Register("IsBrowsMode", typeof(bool), typeof(ETextBox), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((ETextBox)s).OnIsBrowsModeChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private bool preEnabled = true;
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
