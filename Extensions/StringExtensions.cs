using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 左辺から指定した長さの文字を取得します。
        /// </summary>
        /// <param name="s">対象文字</param>
        /// <param name="length">長さ</param>
        /// <param name="padding">長さが足りない場合に埋める文字(ﾃﾞﾌｫﾙﾄ空白)</param>
        /// <returns></returns>
        public static string Left(this string s, int length, char padding = ' ')
        {
            return s.Mid(0, length, padding);
        }

        /// <summary>
        /// 取得する位置と長さを指定して文字を取得します。
        /// </summary>
        /// <param name="s">対象文字</param>
        /// <param name="start">取得する開始位置</param>
        /// <param name="length">取得する文字の長さ</param>
        /// <param name="padding">長さが足りない場合に埋める文字(ﾃﾞﾌｫﾙﾄ空白)</param>
        /// <returns></returns>
        public static string Mid(this string s, int start, int length, char padding = ' ')
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            else if (s.Length < (start + length))
            {
                return s.PadRight(start + length, padding).Mid(start, length, padding);
            }
            else
            {
                return s.Substring(start, length);
            }
        }

        /// <summary>
        /// 右辺から指定した長さの文字を取得します。
        /// </summary>
        /// <param name="s">対象文字</param>
        /// <param name="length">長さ</param>
        /// <param name="padding">長さが足りない場合に埋める文字(ﾃﾞﾌｫﾙﾄ空白)</param>
        /// <returns></returns>
        public static string Right(this string s, int length, char padding = ' ')
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            else if (s.Length < (length))
            {
                return s.PadLeft(length, padding).Right(length, padding);
            }
            else
            {
                return s.Substring(s.Length - length, length);
            }
        }

        /// <summary>
        /// 文字をﾊﾞｲﾄ配列に変換します。
        /// </summary>
        /// <param name="s">対象文字</param>
        /// <param name="encoding">ﾊﾞｲﾄ配列に変換する際に使用するｴﾝｺｰﾃﾞｨﾝｸﾞ</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string s, Encoding encoding)
        {
            return encoding.GetBytes(s);
        }

        /// <summary>
        /// 文字を16進数文字に変換します。
        /// </summary>
        /// <param name="s">対象文字</param>
        /// <param name="encoding">16進数文字に変換する際に使用するｴﾝｺｰﾃﾞｨﾝｸﾞ</param>
        /// <param name="length">16進数文字の長さ(ﾃﾞﾌｫﾙﾄ4)</param>
        /// <returns></returns>
        public static string ToHex(this string s, Encoding encoding, int length = 4)
        {
            return BitConverter.ToString(s.ToBytes(encoding))
                .Replace("-", "")
                .PadRight(length, '0');
        }

        /// <summary>
        /// 16進数文字を数値に変換します。
        /// </summary>
        /// <param name="s">対象の16進数文字</param>
        /// <returns></returns>
        public static long Hex2Long(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;

            switch (s.Length)
            {
                case 2:
                    return Convert.ToInt16(s, 16);
                case 4:
                    return Convert.ToInt16(s, 16);
                case 8:
                    return Convert.ToInt32(s, 16);
                default:
                    return Convert.ToInt64(s, 16);
            }
        }

        public static string Hex2Bin(this string s, int length = 16)
        {
            return Convert.ToString(s.Hex2Long(), 2).PadLeft(length, '0');
        }

        public static string FromHex(this string s, Encoding encoding)
        {
            return encoding.GetString(
                Enumerable.Range(0, s.Length / 2)
                    .Select(i => Convert.ToByte(s.Mid(i * 2, 2), 16))
                    .ToArray()
            ).TrimEnd(new char[] { (char)0, ' ' });
            //For i As Integer = 0 To hex.Length / 2 - 1
            //    dec(i) = Convert.ToByte(hex.Substring(i * 2, 2), 16)
            //Next
            //Return CsvEncoding.GetString(dec)
        }
    }
}
