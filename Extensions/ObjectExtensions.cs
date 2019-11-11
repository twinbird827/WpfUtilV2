using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// ｵﾌﾞｼﾞｪｸﾄを倍精度浮動小数点数に変換し、変換できたかどうかを取得します。
        /// </summary>
        /// <param name="value">対象ｵﾌﾞｼﾞｪｸﾄ</param>
        /// <param name="result">変換できた場合は変換値を格納する参照変数</param>
        public static bool TryDouble(this object value, out double result)
        {
            if (value is double)
            {
                result = (double)value;
                return true;
            }
            else if (value is float)
            {
                result = (float)value;
                return true;
            }
            else if (value is int)
            {
                result = (int)value;
                return true;
            }
            else if (value is long)
            {
                result = (long)value;
                return true;
            }
            else if (value is short)
            {
                result = (short)value;
                return true;
            }
            else if (value is string)
            {
                return double.TryParse((string)value, out result);
            }
            else
            {
                return double.TryParse(value.ToString(), out result);
            }
        }

        /// <summary>
        /// ｵﾌﾞｼﾞｪｸﾄを文字列型に変換します。nullの場合は空文字に変換します。
        /// </summary>
        /// <param name="value">文字列に変換するｵﾌﾞｼﾞｪｸﾄ</param>
        /// <returns></returns>
        public static string TryString(this object value)
        {
            if (value is string)
            {
                return (string)value;
            }
            else if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
