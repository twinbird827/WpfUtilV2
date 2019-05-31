using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfUtilV2.Mvvm.Converters
{
    public class Double2StringFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 変換用ﾊﾟﾗﾒｰﾀの確認
            if (values == null)
            {
                throw new ArgumentException("values is null.");
            }
            else if (values.Length != 2)
            {
                throw new ArgumentException("the number of values is less than 2.");
            }
            else if (values[0] == null)
            {
                throw new ArgumentException("values[0] is null.");
            }
            else if (values[1] == null)
            {
                throw new ArgumentException("values[1] is null.");
            }
            else if (!(values[0] is double))
            {
                throw new ArgumentException("values[0] is not double.");
            }
            else if (!(values[1] is string))
            {
                throw new ArgumentException("values[1] is not string.");
            }

            // ﾌｫｰﾏｯﾄした文字を返却
            return string.Format((string)values[1], values[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}