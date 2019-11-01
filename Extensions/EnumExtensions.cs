using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using WpfUtilV2.Mvvm.Service;

namespace WpfUtilV2.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 列挙値の文字列表現を取得します。
        /// </summary>
        /// <typeparam name="T">列挙値の型</typeparam>
        /// <param name="target">列挙値</param>
        /// <returns></returns>
        public static string GetLabel<T>(this T target) where T : Enum
        {
            var targetString = target.ToString();

            return ServiceFactory.ResourceService?.ResourceManager.GetString($"E_{typeof(T).Name}_{targetString}") 
                ?? typeof(T).GetField(targetString).GetCustomAttribute<LabelAttribute>()?.Value
                ?? targetString;
        }

        /// <summary>
        /// 全列挙値を取得します。
        /// </summary>
        /// <typeparam name="T">取得したい列挙定義</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                yield return value;
            }
        }

    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class LabelAttribute : Attribute
    {
        public LabelAttribute(string label)
        {
            this.Value = label;
        }

        public string Value
        {
            get;
        }
    }
}
