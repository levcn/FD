using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SLControls.Extends;
using Telerik.Windows.Controls;
using WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation;


namespace SLControls.Editors
{
    public class ObjectEditorPanel : RadWindow
    {
        public ObjectEditorPanel()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public static readonly DependencyProperty EditObjectProperty =
                DependencyProperty.Register("EditObject", typeof (object), typeof (ObjectEditorPanel), new PropertyMetadata(default(object)));

        public object EditObject
        {
            get
            {
                return (object) GetValue(EditObjectProperty);
            }
            set
            {
                SetValue(EditObjectProperty, value);
            }
        }

        public static readonly DependencyProperty EditorConfigProperty =
                DependencyProperty.Register("EditorConfig", typeof (ObjectEditorConfig), typeof (ObjectEditorPanel), new PropertyMetadata(default(ObjectEditorConfig)));

        public ObjectEditorConfig EditorConfig
        {
            get
            {
                return (ObjectEditorConfig) GetValue(EditorConfigProperty);
            }
            set
            {
                SetValue(EditorConfigProperty, value);
                
            }
        }

        public new void ShowDialog()
        {
            InitContent();
            base.ShowDialog();
        }

        private void InitContent()
        {
            Grid g = new Grid();
            g.RowDefinitions.Add(new RowDefinition{Height = new GridLength(1,GridUnitType.Star)});
            g.RowDefinitions.Add(new RowDefinition{Height = GridLength.Auto});
            
            ItemPanel = new StackPanel();

            Grid.SetRow(ItemPanel,0);
            var buttons = new StackPanel{HorizontalAlignment = HorizontalAlignment.Right,Orientation = Orientation.Horizontal};
            Grid.SetRow(buttons, 1);
            InitButtons(buttons);
            g.Children.Add(ItemPanel);
            g.Children.Add(buttons);
            this.Content = g;
            if (EditorConfig == null && EditObject!=null)
            {
                EditorConfig = new ObjectEditorConfig();
                EditorConfig.PropertyEditConfigs = new List<ObjectPropertyEditConfig>();
                EditObject.GetType().GetProperties()
                    .Where(w=>w.PropertyType == typeof(string)||
                        w.PropertyType == typeof(int)||
                        w.PropertyType == typeof(int?))
                    .ForEach(w =>
                    {
                        EditorConfig.PropertyEditConfigs.Add(new ObjectPropertyEditConfig {
                            DisplayName = w.Name,
                            Editable = true,
                            IsDisplay = true,
                            PropertyName = w.Name,
                        });
                    });
                
            }
            if (EditorConfig != null)
            {
                InitProperyItems(EditorConfig, ItemPanel);
            }
        }
        StackPanel ItemPanel;
        private void InitProperyItems(ObjectEditorConfig editorConfig, StackPanel panel)
        {
            if (editorConfig.PropertyEditConfigs != null)
            {
                editorConfig.PropertyEditConfigs
                    .Where(w=>w.IsDisplay.HasValue&&w.IsDisplay.Value)
                    .OrderBy(w=>w.OrderIndex)
                    .ForEach(w =>
                {
                    EditItemTextBox item = new EditItemTextBox();
                    item.LabelText = w.DisplayName + "：";
                    Binding b = new Binding(w.PropertyName);
                    b.Source = EditObject;
                    b.Mode = BindingMode.TwoWay;
                    b.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
                    item.SetBinding(BaseEditItem.TextProperty, b);
                    panel.Children.Add(item);
                });
            }
        }

        private void InitButtons(StackPanel buttons)
        {
            this.ModalBackground = new SolidColorBrush(Color.FromArgb(20,50,50,50));
//            this.ModalBackground
            var ok = new Button {Content = "确定"};
            ok.Width = 100;
            ok.Height = 30;
            ok.Margin = new Thickness(3);

            ok.Click += (s, e) =>
            {
                ItemPanel.Children.OfType<BaseEditItem>().ToList().ForEach(w=>w.UpdateSource());
                Close();
            };
            buttons.Children.Add(ok);

            var cancle = new Button { Content = "取消" };
            cancle.Margin = new Thickness(3,3,40,3);
            cancle.Width = 100;
            cancle.Height = 30;
            cancle.Click += (s, e) =>
            {
                Close();
            };
            buttons.Children.Add(cancle);


        }
    }
}
