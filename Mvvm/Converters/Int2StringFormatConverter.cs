using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfUtilV2.Mvvm.Converters
{
    public class Int2StringFormatConverter : IValueConverter
    {
        private Dictionary<string, int> Processeds = new Dictionary<string, int>();

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int output;

            // 表示する値を取得
            if (value is int)
            {
                output = (int)value;
            }
            else if (!int.TryParse(value.ToString(), out output))
            {
                throw new ArgumentException("value is not integer.");
            }

            // ﾏｯﾋﾟﾝｸﾞ
            Processeds[parameter.ToString()] = output;

            return output;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var previous = Processeds[parameter.ToString()];
            int output;

            if (value is string && int.TryParse((string)value, out output))
            {
                return output;
            }
            else
            {
                return previous;
            }
        }
    }
}
