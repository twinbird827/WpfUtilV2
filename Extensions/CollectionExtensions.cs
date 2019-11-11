using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// ﾘｽﾄに配列を追加します。
        /// </summary>
        /// <typeparam name="T">ﾘｽﾄの型</typeparam>
        /// <param name="collection">配列を追加するﾘｽﾄ</param>
        /// <param name="array">ﾘｽﾄに追加する配列</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> array)
        {
            foreach (var tmp in array)
            {
                collection.Add(tmp);
            }
        }
    }
}
