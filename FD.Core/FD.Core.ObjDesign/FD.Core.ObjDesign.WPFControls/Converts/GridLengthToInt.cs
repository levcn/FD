using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace WPFControls.Converts
{
    /// <summary>
    /// DataGridLeng 转 int
    /// </summary>
    public class GridLengthToInt:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new DataGridLength((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)(((DataGridLength)value).Value);
        }
    }
}
