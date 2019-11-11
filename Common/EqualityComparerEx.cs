using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Common
{
    public class EqualityComparerEx<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        /// <param name="do_equals">Equalsﾒｿｯﾄﾞの実体</param>
        /// <param name="do_get_hash_code">GetHashCodeﾒｿｯﾄﾞの実体</param>
        public EqualityComparerEx(Func<T, T, bool> do_equals, Func<T, int> do_get_hash_code)
        {
            this.do_equals = do_equals;
            this.do_get_hash_code = do_get_hash_code;
        }

        /// <summary>
        /// Equalsﾒｿｯﾄﾞの実体
        /// </summary>
        private Func<T, T, bool> do_equals { get; set; }

        /// <summary>
        /// GetHashCodeﾒｿｯﾄﾞの実体
        /// </summary>
        private Func<T, int> do_get_hash_code { get; set; }

        /// <summary>
        /// ｵﾌﾞｼﾞｪｸﾄの等価比較をします。
        /// </summary>
        /// <param name="x">ｵﾌﾞｼﾞｪｸﾄ1</param>
        /// <param name="y">ｵﾌﾞｼﾞｪｸﾄ2</param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return do_equals(x, y);
        }

        /// <summary>
        /// ｵﾌﾞｼﾞｪｸﾄのﾊｯｼｭｺｰﾄﾞを取得します。
        /// </summary>
        /// <param name="obj">ｵﾌﾞｼﾞｪｸﾄ</param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return do_get_hash_code(obj);
        }
    }
}
