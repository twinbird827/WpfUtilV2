using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class ObjectExtensions
    {
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
                return null;
            }
        }
    }
}
