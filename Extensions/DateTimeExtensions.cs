using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 日における起点となる日付を取得します。
        /// </summary>
        /// <param name="date">元ﾃﾞｰﾀ</param>
        public static DateTime ToBeginDays(this DateTime date)
        {
            return date.ToBeginHours()
                .AddHours(date.Hour * -1);
        }

        /// <summary>
        /// 時における起点となる日付を取得します。
        /// </summary>
        /// <param name="date">元ﾃﾞｰﾀ</param>
        public static DateTime ToBeginHours(this DateTime date)
        {
            return date.ToBeginMinutes()
                .AddMinutes(date.Minute * -1);
        }

        /// <summary>
        /// 分における起点となる日付を取得します。
        /// </summary>
        /// <param name="date">元ﾃﾞｰﾀ</param>
        public static DateTime ToBeginMinutes(this DateTime date)
        {
            return date.ToBeginSeconds()
                .AddSeconds(date.Second * -1);
        }

        /// <summary>
        /// 秒における起点となる日付を取得します。
        /// </summary>
        /// <param name="date">元ﾃﾞｰﾀ</param>
        public static DateTime ToBeginSeconds(this DateTime date)
        {
            return date
                .AddMilliseconds(date.Millisecond * -1);
        }

        /// <summary>
        /// 日における終点となる日付を取得します。
        /// </summary>
        /// <param name="date">元ﾃﾞｰﾀ</param>
        public static DateTime ToEndDays(this DateTime date)
        {
            return date.ToEndHours()
                .AddHours(23 - date.Hour);
        }

        /// <summary>
        /// 時における終点となる日付を取得します。
        /// </summary>
        /// <param name="date">元ﾃﾞｰﾀ</param>
        public static DateTime ToEndHours(this DateTime date)
        {
            return date.ToEndMinutes()
                .AddMinutes(59 - date.Minute);
        }

        /// <summary>
        /// 分における終点となる日付を取得します。
        /// </summary>
        /// <param name="date">元ﾃﾞｰﾀ</param>
        public static DateTime ToEndMinutes(this DateTime date)
        {
            return date.ToEndSeconds()
                .AddSeconds(59 - date.Second);
        }

        /// <summary>
        /// 秒における終点となる日付を取得します。
        /// </summary>
        /// <param name="date">元ﾃﾞｰﾀ</param>
        public static DateTime ToEndSeconds(this DateTime date)
        {
            return date
                .AddMilliseconds(999 - date.Millisecond);
        }
    }
}
