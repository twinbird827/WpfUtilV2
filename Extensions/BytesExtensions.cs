using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class BytesExtensions
    {
        /// <summary>
        /// ﾊﾞｲﾄ配列を文字に変換します。
        /// </summary>
        /// <param name="bytes">対象ﾊﾞｲﾄ配列</param>
        /// <param name="encoding">文字に変換する際に使用するｴﾝｺｰﾃﾞｨﾝｸﾞ</param>
        /// <returns></returns>
        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }
    }
}
