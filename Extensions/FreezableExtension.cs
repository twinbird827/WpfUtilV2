﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfUtilV2.Extensions
{
    public static class FreezableExtension
    {
        /// <summary>
        /// FreezableをFrozenした値を取得します。
        /// </summary>
        /// <typeparam name="T">返却する際のFreezable</typeparam>
        /// <param name="target">Freezableｲﾝｽﾀﾝｽ</param>
        /// <returns></returns>
        public static T Frozen<T>(this T target) where T : Freezable
        {
            return target.GetAsFrozen() as T;
        }
    }
}
