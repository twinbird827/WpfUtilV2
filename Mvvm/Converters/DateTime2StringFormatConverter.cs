using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfUtilV2.Mvvm.Converters
{
    public class DateTime2StringFormatConverter : IMultiValueConverter
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
            if (values[0] is DateTime)
            {
                data.Value = (DateTime)values[0];
            }
            else
            {
                throw new ArgumentException("values[0] is not DateTime.");
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

            DateTime tmp;
            if (value is string && DateTime.TryParseExact((string)value, previous.Format, null, DateTimeStyles.None, out tmp))
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
            public FormattedData(DateTime value, string format)
            {
                this.Value = value;
                this.Format = format;
            }

            public DateTime Value;

            public string Format;
        }
    }
}
