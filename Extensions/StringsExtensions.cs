using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class StringsExtensions
    {
        /// <summary>
        /// 文字配列を連結文字で結合して一つの文字列として返却します。
        /// </summary>
        /// <param name="s">対象文字配列</param>
        /// <param name="separator">連結する文字</param>
        /// <returns></returns>
        public static string GetString(this IEnumerable<string> s, string separator = "")
        {
            return string.Join(separator, s);
        }

    }
}
