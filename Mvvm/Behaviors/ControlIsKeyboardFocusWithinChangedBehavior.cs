﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUtilV2.Mvvm.CustomControls;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public class ControlIsKeyboardFocusWithinChangedBehavior
    {
        /// <summary>
        /// Commandの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty FocusableProperty =
            DependencyProperty.RegisterAttached("Focusable", typeof(IFocusableItem), typeof(ControlIsKeyboardFocusWithinChangedBehavior), new UIPropertyMetadata(FocusableProperty_Changed));

        /// <summary>
        /// ｺﾏﾝﾄﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetFocusable(DependencyObject target, object value)
        {
            target.SetValue(FocusableProperty, value);
        }

        /// <summary>
        /// ｺﾏﾝﾄﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static IFocusableItem GetFocusable(DependencyObject target)
        {
            return (IFocusableItem)target.GetValue(FocusableProperty);
        }

        /// <summary>
        /// Commandﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void FocusableProperty_Changed(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var control = target as FrameworkElement;

            BehaviorUtil.SetEventHandler(control,
                (fe) => fe.IsKeyboardFocusWithinChanged += Control_IsKeyboardFocusWithinChanged,
                (fe) => 
                fe.IsKeyboardFocusWithinChanged -= Control_IsKeyboardFocusWithinChanged
            );
        }

        /// <summary>
        /// ﾏｳｽﾀﾞﾌﾞﾙｸﾘｯｸ時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void Control_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as FrameworkElement;
            if (control != null)
            {
                IFocusableItem command = (IFocusableItem)control.GetValue(FocusableProperty);

                command.IsFocused = control.IsKeyboardFocusWithin;
            }
        }

    }
}