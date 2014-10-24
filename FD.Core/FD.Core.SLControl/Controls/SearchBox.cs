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


namespace FD.Core.SLControl.Controls
{
    public class SearchBox : BaseMultiControl
    {
        public SearchBox()
        {
            this.DefaultStyleKey = typeof(SearchBox);
        }

        public static readonly DependencyProperty ButtonVisibilityProperty =
                DependencyProperty.Register("ButtonVisibility", typeof (Visibility), typeof (SearchBox), new PropertyMetadata(default(Visibility), (s, e) =>
                {
                    ((SearchBox) s).OnButtonVisibilityChanged(e.NewValue is Visibility ? (Visibility) e.NewValue : Visibility.Visible);
                }));

        public static readonly DependencyProperty SearchTextProperty =
                DependencyProperty.Register("SearchText", typeof (string), typeof (SearchBox), new PropertyMetadata("搜索", (s, e) =>
                {
                    ((SearchBox) s).OnSearchTextChanged(e.NewValue as string);
                }));

        private void OnSearchTextChanged(string list)
        {}

        public string SearchText
        {
            get
            {
                return (string) GetValue(SearchTextProperty);
            }
            set
            {
                SetValue(SearchTextProperty, value);
            }
        }
        private void OnButtonVisibilityChanged(Visibility list)
        {}

        public Visibility ButtonVisibility
        {
            get
            {
                return (Visibility) GetValue(ButtonVisibilityProperty);
            }
            set
            {
                SetValue(ButtonVisibilityProperty, value);
            }
        }
        public override void LoadConfig(string configStr)
        {
            
        }
    }
}
