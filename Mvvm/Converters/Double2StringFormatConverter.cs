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
        private Dictionary<string, FormattedData> Processeds = new Dictionary<string, FormattedData>();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            FormattedData data;

            // 変換用ﾊﾟﾗﾒｰﾀの確認
            if (values == null)
            {
                throw new ArgumentException("values is null.");
            }
            else if (values.Length < 2)
            {
                throw new ArgumentException("the number of values is less than 2.");
            }
            else if (parameter == null)
            {
                throw new ArgumentException("parameter is null.");
            }
            else if (values[0] == null)
            {
                throw new ArgumentException("values[0] is null.");
            }

            // 書式指定前の値を取得
            if (values[0] is double)
            {
                data.Value = (double)values[0];
            }
            else if (!double.TryParse(values[0].ToString(), out data.Value))
            {
                throw new ArgumentException("values[0] is not double.");
            }

            // 書式を取得
            data.Format = (string)values[1];

            // 変換ﾃﾞｰﾀとしてﾏｯﾋﾟﾝｸﾞする。
            Processeds[parameter.ToString()] = data;

            // ﾌｫｰﾏｯﾄした文字を返却
            return string.Format(data.Format, data.Value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var previous = Processeds[parameter.ToString()];

            double tmp;
            if (value is string && double.TryParse((string)value, out tmp))
            {
                return new object[] { tmp, previous.Format };
            }
            else
            {
                return new object[] { previous.Value, previous.Format };
            }
        }

        private struct FormattedData
        {
            public FormattedData(double value, string format)
            {
                this.Value = value;
                this.Format = format;
            }

            public double Value;

            public string Format;
        }
    }
}