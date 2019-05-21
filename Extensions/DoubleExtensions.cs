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

        /// <summary>
        /// 指定した範囲内の値を取得します。
        /// </summary>
        /// <param name="target">対象値</param>
        /// <param name="minimum">最小値</param>
        /// <param name="maximum">最大値</param>
        /// <returns>対象値が最小値、または最大値の範囲外なら最小値、または最大値を返却する。それ以外は対象値を返却する。</returns>
        public static double InRange(this double target, double minimum, double maximum)
        {
            return target < minimum
                ? minimum
                : maximum < target
                ? maximum
                : target;
        }
    }
}
