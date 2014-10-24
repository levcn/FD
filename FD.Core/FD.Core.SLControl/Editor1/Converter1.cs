using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLControls.Editor1
{
    public class Converter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = parameter as RelativeSource;
            //            r.ProvideValue()
            var c = parameter as Control;
            var validationErrors = Validation.GetErrors(c);
            //            return value;
            //            IList<ValidationError> validationErrors = value as IList<ValidationError>;
            if (validationErrors == null || validationErrors.Count == 0) return "";
            else
            {
                var ll = validationErrors.Select(w => w.ErrorContent.ToString()).ToArray();
                return ll.Aggregate("", (current, a) => current + a);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
