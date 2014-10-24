using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SLControls.Extends;


namespace SLControls.Editors
{
    public class NumberBox : TextBox
    {
        public NumberBox()
        {
            Loaded += NumberBox_Loaded;
            KeyDown += NumberBox_KeyDown;
            TextChanged += NumberBox_TextChanged;
        }

        public static readonly DependencyProperty AcceptDotProperty =
                DependencyProperty.Register("AcceptDot", typeof(bool), typeof(NumberBox), new PropertyMetadata(true));

        /// <summary>
        /// 是否接受小数点
        /// </summary>
        public bool AcceptDot
        {
            get
            {
                return (bool)GetValue(AcceptDotProperty);
            }
            set
            {
                SetValue(AcceptDotProperty, value);
            }
        }
        public static readonly DependencyProperty MaxNumberProperty =
                DependencyProperty.Register("MaxNumber", typeof(double), typeof(NumberBox), new PropertyMetadata(double.MaxValue));

        public double MaxNumber
        {
            get
            {
                return (double)GetValue(MaxNumberProperty);
            }
            set
            {
                SetValue(MaxNumberProperty, value);
            }
        }
        public static readonly DependencyProperty NumberProperty =
                DependencyProperty.Register("Number", typeof(int?), typeof(NumberBox), new PropertyMetadata(null,
                                                                                                              (s, e) =>
                                                                                                              {
                                                                                                                  var source = ((NumberBox)s);
                                                                                                                  if (e.NewValue != null)
                                                                                                                  {
                                                                                                                      source.Text = e.NewValue.ToString();
                                                                                                                  }
                                                                                                              }));

        public int? Number
        {
            get
            {
                return (int?)GetValue(NumberProperty);
            }
            set
            {
                SetValue(NumberProperty, value);
            }
        }


        void NumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Match m = Regex.Match(Text, pattern);
                if (string.IsNullOrEmpty(Text))
                {
                    Text = null;
                    param = null;
                    Number = null;
                }
                else
                {
                    if (!m.Success)
                    {
                        Text = param;
                        SelectionStart = Text.Length;
                    }
                    else
                    {
                        int val = Text.ToInt();
                        if (val > MaxNumber) val = (int)MaxNumber;
                        param = val.ToString();
                        Text = param;
                        if (param != null)
                        {
                            Number = param.ToInt();
                        }
                        else
                        {
                            Number = null;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        void NumberBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Decimal && AcceptDot)
                 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9
                 || e.Key >= Key.D0 && e.Key <= Key.D9
                )
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        void NumberBox_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private string pattern = @"^[0-9]*$";
        private string param = "";

    }
}
