using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;


namespace SLControls.Editors
{
    public class EditItemNumberBox : Control
    {
        public EditItemNumberBox()
        {
            this.DefaultStyleKey = typeof(EditItemNumberBox);
        }

        public static readonly DependencyProperty MinimumProperty =
                DependencyProperty.Register("Minimum", typeof(double), typeof(EditItemNumberBox), new PropertyMetadata(default(double)));

        public double Minimum
        {
            get
            {
                return (double)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        public static readonly DependencyProperty MaximumProperty =
                DependencyProperty.Register("Maximum", typeof(double), typeof(EditItemNumberBox), new PropertyMetadata(1000000));

        public double Maximum
        {
            get
            {
                return (double)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        public static readonly DependencyProperty SmallChangeProperty =
                DependencyProperty.Register("SmallChange", typeof(double), typeof(EditItemNumberBox), new PropertyMetadata(1));

        public double SmallChange
        {
            get
            {
                return (double)GetValue(SmallChangeProperty);
            }
            set
            {
                SetValue(SmallChangeProperty, value);
            }
        }

        public static readonly DependencyProperty LargeChangeProperty =
                DependencyProperty.Register("LargeChange", typeof(double), typeof(EditItemNumberBox), new PropertyMetadata(1));

        public double LargeChange
        {
            get
            {
                return (double)GetValue(LargeChangeProperty);
            }
            set
            {
                SetValue(LargeChangeProperty, value);
            }
        }

        public static readonly DependencyProperty ValueFormatProperty =
                DependencyProperty.Register("ValueFormat", typeof(ValueFormat), typeof(EditItemNumberBox), new PropertyMetadata(ValueFormat.Numeric));

        public ValueFormat ValueFormat
        {
            get
            {
                return (ValueFormat)GetValue(ValueFormatProperty);
            }
            set
            {
                SetValue(ValueFormatProperty, value);
            }
        }

        public static readonly DependencyProperty NumberDecimalDigitsProperty =
                DependencyProperty.Register("NumberDecimalDigits", typeof(int), typeof(EditItemNumberBox), new PropertyMetadata(0));

        public int NumberDecimalDigits
        {
            get
            {
                return (int)GetValue(NumberDecimalDigitsProperty);
            }
            set
            {
                SetValue(NumberDecimalDigitsProperty, value);
            }
        }
    }
}
