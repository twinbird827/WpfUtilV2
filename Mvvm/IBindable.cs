﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm
{
    public interface IBindable : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// プロパティ値が変更されたことをリスナーに通知します。
        /// </summary>
        /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
        /// この値は省略可能で、
        /// <see cref="CallerMemberNameAttribute"/> をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
        void OnPropertyChanged([CallerMemberName] string propertyName = null);

        /// <summary>
        /// ﾌﾟﾛﾊﾟﾃｨ変更時ｲﾍﾞﾝﾄを追加します。
        /// </summary>
        /// <param name="bindable">追加元のｲﾝｽﾀﾝｽ</param>
        /// <param name="handler">追加するｲﾍﾞﾝﾄの中身</param>
        void AddOnPropertyChanged(BindableBase bindable, PropertyChangedEventHandler handler);

    }
}
