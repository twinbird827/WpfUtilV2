using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// 数値を16進数文字に変換します。
        /// </summary>
        /// <param name="i">対象数値</param>
        /// <param name="length">16進数文字の長さ(ﾃﾞﾌｫﾙﾄ2)</param>
        /// <returns></returns>
        public static string ToHex(this double d, int length = 2)
        {
            return ((long)d).ToHex(length);
        }
    }
}
