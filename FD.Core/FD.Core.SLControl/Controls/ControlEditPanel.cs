using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.PropertyGrid;


namespace SLControls.Controls
{
    public class ControlEditPanel : Control
    {
        public ControlEditPanel()
        {
            this.DefaultStyleKey = typeof(ControlEditPanel);
        }
        RadPropertyGrid CTL_PropertyGrid;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CTL_PropertyGrid = GetTemplateChild("CTL_PropertyGrid") as RadPropertyGrid;
            OnItemChanged(Item);
        }

        public static readonly DependencyProperty ItemProperty =
                DependencyProperty.Register("Item", typeof(object), typeof(ControlEditPanel), new PropertyMetadata(default(object),
                        (s, e) =>
                        {
                            ((ControlEditPanel) s).OnItemChanged(e.NewValue);
                        }));

        private void OnItemChanged(object testButton)
        {
            if (CTL_PropertyGrid == null || testButton==null) return;
            var list = testButton.GetType().GetProperties().Select(w => new { P = w, A = w.GetCustomAttributes(typeof(EditableAttribute), true) }).Where(w => w.A.Any()).ToList();
            CTL_PropertyGrid.PropertyDefinitions.Clear();
            list.ForEach(w =>
            {
                var ea = w.A[0] as EditableAttribute;
                ea.DisplayName = ea.DisplayName ?? w.P.Name;
                ea.Description = ea.Description ?? ea.DisplayName;
                var item = new PropertyDefinition
                {
                    Binding = new Binding { Path = new PropertyPath(w.P.Name), Mode = BindingMode.TwoWay },
                    GroupName = ea.GroupName,
                    DisplayName = ea.DisplayName,
                    Description = ea.Description,
                };
                CTL_PropertyGrid.PropertyDefinitions.Add(item);
//                item.PropertyChanged += item_PropertyChanged;
            });
        }

        public object Item
        {
            get
            {
                return (object)GetValue(ItemProperty);
            }
            set
            {
                SetValue(ItemProperty, value);
            }
        }
    }
}
