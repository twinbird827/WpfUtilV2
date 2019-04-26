using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class IEnumerableExtensions
    {
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

        public static TResult MaxOrDefault<T, TResult>(this IEnumerable<T> source, Func<T, TResult> func)
        {
            return source.MaxOrDefault(func, default(TResult));
        }
        public static TResult MaxOrDefault<T, TResult>(this IEnumerable<T> source, Func<T, TResult> func, TResult def)
        {
            return source.Any() ? source.Max(func) : def;
        }
    }
}