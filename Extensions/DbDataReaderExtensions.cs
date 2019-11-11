using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class DbDataReaderExtensions
    {
        public static async Task<IEnumerable<T>> GetRows<T>(this DbDataReader reader, Func<DbDataReader, T> func)
        {
            var ret = new List<T>();
            while (await reader.ReadAsync())
            {
                ret.Add(func(reader));
            }
            return ret;
        }
    }
}
