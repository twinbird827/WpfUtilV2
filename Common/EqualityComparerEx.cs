using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Common
{
    public class EqualityComparerEx<T> : IEqualityComparer<T>
    {
        public EqualityComparerEx(Func<T, T, bool> do_equals, Func<T, int> do_get_hash_code)
        {
            this.do_equals = do_equals;
            this.do_get_hash_code = do_get_hash_code;
        }

        private Func<T, T, bool> do_equals { get; set; }

        private Func<T, int> do_get_hash_code { get; set; }

        public bool Equals(T x, T y)
        {
            return do_equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return do_get_hash_code(obj);
        }
    }
}
