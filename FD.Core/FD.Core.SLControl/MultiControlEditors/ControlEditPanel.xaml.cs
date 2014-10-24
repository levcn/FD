using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using fastJSON;
using SLControls.Editors;


namespace SLControls.MultiControlEditors
{
    public partial class ControlEditPanel
    {
        public ControlEditPanel()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty EditControlProperty =
                DependencyProperty.Register("EditControl", typeof(BaseMultiControl), typeof(ControlEditPanel), new PropertyMetadata(default(BaseMultiControl), (s, e) =>
                {
                    var panel = ((ControlEditPanel) s);
                    panel.InitControl();
                }));

        private void InitControl()
        {
            LayoutRoot.Children.Clear();
            List<string> list = EditControl.EditablePropters;
            var ps = EditControl.GetType().GetProperties();
            list.ForEach(w =>
            {
                ControlEditItem sc = new ControlEditItem();
                sc.Text = w;
                sc.SetBinding(ControlEditItem.ValueProperty, new Binding {
                    Path = new PropertyPath(w),
                    Source = EditControl,
                    Mode = BindingMode.TwoWay,
                });
                sc.ValueType = 1;
                var p = ps.FirstOrDefault(z => z.Name == w);
                if (p != null)
                {
                    if (p.PropertyType == typeof (Brush))
                    {
                        sc.ValueType = 2;
                        sc.ItemSource = new List<string> {
                            "Black","Brown","Magenta","Green","Red"
                        };
                    }
                }
                
                sc.ValueChanged += sc_ValueChanged;
                LayoutRoot.Children.Add(sc);
            });
            GatherValues();
        }

        void sc_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            GatherValues();
            
        }

        public void SetTextValue(string jsonText)
        {
            List<NameValue> list = JSON.Instance.ToObject(jsonText, typeof(List<NameValue>)) as List<NameValue>;
            if (list != null)
            {
                LayoutRoot.Children.Cast<ControlEditItem>()
                .ToList().ForEach(w =>
                {
                    var l = list.FirstOrDefault(z => z.Name == w.Text);
                    if (l != null)
                    {
                        w.Value = l.Value;
                    }
                });
            }
        }
        private void GatherValues()
        {
            var list = LayoutRoot.Children.Cast<ControlEditItem>()
                .ToList()
                .Select(w=>new NameValue{Name = w.Text,Value = w.Value}).ToList();
            JsonText = JSON.Instance.ToJSON(list);
            OnValueChanged();
        }

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged()
        {
            EventHandler handler = ValueChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public string JsonText { get; set; }
        public BaseMultiControl EditControl
        {
            get
            {
                return (BaseMultiControl)GetValue(EditControlProperty);
            }
            set
            {
                SetValue(EditControlProperty, value);
            }
        }
    }

    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
