using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 指定した配列を<code>chunkSize</code>区切りのIEnumerable&lt;IEnumerable&lt;T&gt;&gt;として取得します。
        /// </summary>
        /// <typeparam name="T">配列内のｲﾝｽﾀﾝｽ型</typeparam>
        /// <param name="source">元配列</param>
        /// <param name="chunkSize">元配列を分割する区切り数</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            if (chunkSize <= 0)
                throw new ArgumentException("Chunk size must be greater than 0.", nameof(chunkSize));

            while (source.Any())
            {
                yield return source.Take(chunkSize);
                source = source.Skip(chunkSize);
            }
        }

        /// <summary>
        /// 最大値、またはﾃﾞﾌｫﾙﾄ値を取得します。
        /// </summary>
        /// <typeparam name="T">配列の型</typeparam>
        /// <typeparam name="TResult">返却する値の型</typeparam>
        /// <param name="source">配列</param>
        /// <param name="func">返却する値を取得するﾛｼﾞｯｸ</param>
        /// <returns></returns>
        public static TResult MaxOrDefault<T, TResult>(this IEnumerable<T> source, Func<T, TResult> func)
        {
            return source.MaxOrDefault(func, default(TResult));
        }

        /// <summary>
        /// 最大値、またはﾃﾞﾌｫﾙﾄ値を取得します。
        /// </summary>
        /// <typeparam name="T">配列の型</typeparam>
        /// <typeparam name="TResult">返却する値の型</typeparam>
        /// <param name="source">配列</param>
        /// <param name="func">返却する値を取得するﾛｼﾞｯｸ</param>
        /// <param name="def">ﾃﾞﾌｫﾙﾄ値</param>
        /// <returns></returns>
        public static TResult MaxOrDefault<T, TResult>(this IEnumerable<T> source, Func<T, TResult> func, TResult def)
        {
            return source.Any() ? source.Max(func) : def;
        }

        /// <summary>
        /// 指定した値の配列ｲﾝﾃﾞｯｸｽを取得します。
        /// </summary>
        /// <typeparam name="T">配列の型</typeparam>
        /// <param name="source">配列</param>
        /// <param name="item">検索値</param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T item)
        {
            var i = 0;
            foreach (var value in source)
            {
                if (object.Equals(value, item))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
    }
}