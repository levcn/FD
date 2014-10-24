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
using SLComponse.Validate;


namespace SLControls.Controls
{
    public partial class PropValidEditor : UserControl
    {
        public PropValidEditor()
        {
            InitializeComponent();
            ValidTypes = ColumnValidataConfig.ValidTypes;

        }
        public static readonly DependencyProperty ValidTypesProperty =
                DependencyProperty.Register("ValidTypes", typeof(List<ValidType>), typeof(PropValidEditor), new PropertyMetadata(default(List<ValidType>), (s, e) =>
                {
                    ((PropValidEditor) s).OnValidTypesChanged(e.NewValue as List<ValidType>);
                }));

        private void OnValidTypesChanged(List<ValidType> list)
        {}

        public List<ValidType> ValidTypes
        {
            get
            {
                return (List<ValidType>) GetValue(ValidTypesProperty);
            }
            set
            {
                SetValue(ValidTypesProperty, value);
            }
        }
    }
}
