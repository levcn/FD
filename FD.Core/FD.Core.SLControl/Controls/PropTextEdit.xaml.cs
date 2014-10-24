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
using SLControls.Extends;


namespace SLControls.Controls
{
    public partial class PropTextEdit
    {
        public static readonly DependencyProperty ConfigProperty =
                DependencyProperty.Register("Config", typeof(PropItem), typeof(PropTextEdit), new PropertyMetadata(default(PropItem), (s, e) =>
                {
                    ((PropTextEdit)s).OnConfigChanged(e.NewValue as PropItem);
                }));

        private bool initing = false;

        private void OnConfigChanged(PropItem propItem)
        {
            initing = true;
            if (cb != null)
            {
                cb.IsEnabled = propItem.IsSaveCache;
                var v = new[] {CacheType.Navigator, CacheType.App, CacheType.Page};
                cb.SelectedIndex = v.FindIndex(w => w == propItem.CacheType);
            }
            initing = false;
        }

        public PropItem Config
        {
            get
            {
                return (PropItem)GetValue(ConfigProperty);
            }
            set
            {
                SetValue(ConfigProperty, value);
            }
        }
        
        public PropTextEdit()
        {
            InitializeComponent();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initing) return;
            var cb = sender as ComboBox;
            var v = new []{CacheType.Navigator,CacheType.App,CacheType.Page};
            Config.CacheType = v[cb.SelectedIndex];
        }
    }
}
