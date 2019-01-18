using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfUtilV2.Mvvm.Converters
{
    public class Boolean2VisibilityConverter : IValueConverter
    {
        private BooleanToVisibilityConverter Inner { get; set; } = new BooleanToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility)Inner.Convert(value, targetType, parameter, culture);

            return visibility == Visibility.Visible
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Inner.ConvertBack(value, targetType, parameter, culture);
        }
    }
}
