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
        /// <summary>
        /// DbDataReaderを全件読み出し、ﾘｽﾄとして取得します。
        /// </summary>
        /// <typeparam name="T">1行の型</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <param name="func">1行読み出し用の処理内容</param>
        /// <returns></returns>
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
