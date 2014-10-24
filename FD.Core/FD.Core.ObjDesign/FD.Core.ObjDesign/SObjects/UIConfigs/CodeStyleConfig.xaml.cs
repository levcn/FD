using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using STComponse.CFG;


namespace ProjectCreater.SObjects.UIConfigs
{
    /// <summary>
    /// CodeStyleConfig.xaml 的交互逻辑
    /// </summary>
    public partial class CodeStyleConfig
    {
        public CodeStyleConfig()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TagPixTextProperty =
                DependencyProperty.Register("TagPixText", typeof (string), typeof (CodeStyleConfig), new PropertyMetadata(default(string),
                        (s, e) =>
                        {
                            ((CodeStyleConfig)s).OnTagPixTextChanged(e.NewValue as string);
                            
                        }));

        private void OnTagPixTextChanged(string str)
        {
            TagPix = new ObservableCollection<string>(str.Split('\r', '\n').Where(w => !string.IsNullOrEmpty(w)).ToList());
        }

        public string TagPixText
        {
            get
            {
                return (string) GetValue(TagPixTextProperty);
            }
            set
            {
                SetValue(TagPixTextProperty, value);
            }
        }

        public static readonly DependencyProperty SavePathProperty =
                DependencyProperty.Register("SavePath", typeof (string), typeof (CodeStyleConfig), new PropertyMetadata(default(string)));

        public string SavePath
        {
            get
            {
                return (string) GetValue(SavePathProperty);
            }
            set
            {
                SetValue(SavePathProperty, value);
            }
        }
        public static readonly DependencyProperty TagPixProperty =
                DependencyProperty.Register("TagPix", typeof (ObservableCollection<string>), typeof (CodeStyleConfig), new PropertyMetadata(default(ObservableCollection<string>), (s, e) =>
                {
                    ((CodeStyleConfig)s).OnTagPixChanged(e.NewValue as ObservableCollection<string>);
                }));

        private void OnTagPixChanged(ObservableCollection<string> list)
        {
            TagPixText = list.Serialize1("\r\n");
        }

        public static readonly DependencyProperty FilterTagPixProperty =
                DependencyProperty.Register("FilterTagPix", typeof (bool), typeof (CodeStyleConfig), new PropertyMetadata(default(bool)));

        public bool FilterTagPix
        {
            get
            {
                return (bool) GetValue(FilterTagPixProperty);
            }
            set
            {
                SetValue(FilterTagPixProperty, value);
            }
        }
        public ObservableCollection<string> TagPix
        {
            get
            {
                return (ObservableCollection<string>) GetValue(TagPixProperty);
            }
            set
            {
                SetValue(TagPixProperty, value);
            }
        }
    }
}
