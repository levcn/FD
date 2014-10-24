using System;
using System.Collections.Generic;
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


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// SObjectPanel.xaml 的交互逻辑
    /// </summary>
    public partial class SObjectPanel
    {
        public SObjectPanel()
        {
            InitializeComponent();
            MouseUp += SObjectPanel_MouseUp;
        }

        void SObjectPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            OnEndDrag(e);
        }

        public static readonly DependencyProperty EDataObjectProperty =
                DependencyProperty.Register("EDataObject", typeof (EDataObject), typeof (SObjectPanel), new PropertyMetadata(default(EDataObject)));

        public EDataObject EDataObject
        {
            get
            {
                return (EDataObject) GetValue(EDataObjectProperty);
            }
            set
            {
                SetValue(EDataObjectProperty, value);
            }
        }

        public static readonly DependencyProperty MBackgroundProperty =
                DependencyProperty.Register("MBackground", typeof (Brush), typeof (SObjectPanel), new PropertyMetadata(new SolidColorBrush(Colors.Cornsilk)));

        public Brush MBackground
        {
            get
            {
                return (Brush) GetValue(MBackgroundProperty);
            }
            set
            {
                SetValue(MBackgroundProperty, value);
            }
        }

        public event TEventHandler<object, MouseButtonEventArgs> EndDrag;

        protected virtual void OnEndDrag(MouseButtonEventArgs e)
        {
            var handler = EndDrag;
            if (handler != null) handler(this, e);
        }

        public event TEventHandler<object, MouseButtonEventArgs> StartDrag;

        protected virtual void OnStartDrag(MouseButtonEventArgs e)
        {
            var handler = StartDrag;
            if (handler != null) handler(this, e);
        }

        public Point StartPoint;
        private void Header_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            StartPoint = e.GetPosition(this);
            this.CaptureMouse();
            OnStartDrag(e);
        }

        private void Header_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            StartPoint = default(Point);
            OnEndDrag(e);
        }

    }
}
