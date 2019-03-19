using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace WpfUtilV2.Extensions
{
    public static class EnumExtensions
    {
        public static string GetLabel<T>(this T target) where T : Enum
        {
            var targetString = target.ToString();
            return typeof(T).GetField(targetString).GetCustomAttribute<LabelAttribute>()?.Value
                ?? targetString;
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
