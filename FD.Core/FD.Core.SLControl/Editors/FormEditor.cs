using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using StaffTrain.SVFw.Data;
using Telerik.Windows.Controls;


namespace SLControls.Editors
{
    /// <summary>
    /// 大字段的数据编辑
    /// </summary>
    public class FormEditor : Control
    {
        private Panel C_Grid;

        public FormEditor()
        {
            this.DefaultStyleKey = typeof(FormEditor);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            C_Grid = GetTemplateChild("C_Grid") as Panel;
            InitControlContent(ControlString);
        }

        public static readonly DependencyProperty FieldsConfigProperty =
                DependencyProperty.Register("FieldsConfig", typeof(List<Field>), typeof(FormEditor), new PropertyMetadata(default(List<Field>)));

        public List<Field> FieldsConfig
        {
            get
            {
                return (List<Field>)GetValue(FieldsConfigProperty);
            }
            set
            {
                SetValue(FieldsConfigProperty, value);
            }
        }
        public static readonly DependencyProperty ControlStringProperty =
                DependencyProperty.Register("ControlString", typeof (string), typeof (FormEditor), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((FormEditor) s).ControlStringChanged((string)e.NewValue);
                }));

        private void ControlStringChanged(string s)
        {
            
            InitControlContent(s);
        }

        private void InitControlContent(string s)
        {
            if (C_Grid == null) return;
            C_Grid.Children.Clear();
            if (string.IsNullOrEmpty(s)) return;
            var obj = XamlReader.Load(s) as Panel;
            if (FieldsConfig != null)
            {
                var childs = obj.ChildrenOfType<FrameworkElement>();
                FieldsConfig.ForEach(w =>
                {
                    var c = childs.FirstOrDefault(z => z.Name == w.ControlName);
                    if (c != null)
                    {
                        if(c is TextBox)SetValue(c as TextBox, w);
                    }
                });
            }
            C_Grid.Children.Add(obj);
        }

        public List<Field> Gather()
        {
            if (C_Grid == null) return null;
            if (FieldsConfig != null)
            {

                var childs = C_Grid.ChildrenOfType<FrameworkElement>();
                FieldsConfig.ForEach(w =>
                {
                    var c = childs.FirstOrDefault(z => z.Name == w.ControlName);
                    if (c != null)
                    {
                        if (c is TextBox)w.Value = (c as TextBox).Text;
                    }
                });
            }
            return FieldsConfig;
        }
        void SetValue(FrameworkElement tb, Field f)
        {
            
        }
        void SetValue(TextBox tb,Field f)
        {
            tb.Text = f.Value;
        }
        public string ControlString
        {
            get
            {
                return (string) GetValue(ControlStringProperty);
            }
            set
            {
                SetValue(ControlStringProperty, value);
            }
        }
    }
}
