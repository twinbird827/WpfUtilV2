using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfUtilV2.Mvvm.Converters
{
    public class Tick2DateDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long && parameter is string)
            {
                if ((long)value == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format((string)parameter, new DateTime((long)value));
                }
            }
            else
            {
                throw new ArgumentException("value is not long, or, parameter is not string.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
