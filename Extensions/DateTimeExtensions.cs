using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToBeginDays(this DateTime date)
        {
            return date
                .AddHours(date.Hour * -1)
                .AddMinutes(date.Minute * -1)
                .AddSeconds(date.Second * -1)
                .AddMilliseconds(date.Millisecond * -1);
        }

        public static DateTime ToEndDays(this DateTime date)
        {
            return date
                .AddHours(23 - date.Hour)
                .AddMinutes(59 - date.Minute)
                .AddSeconds(59 - date.Second)
                .AddMilliseconds(999 - date.Millisecond);
        }
    }
}
