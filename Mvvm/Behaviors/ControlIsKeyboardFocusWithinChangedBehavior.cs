using System;
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
    /// <summary>
    /// ｺﾝﾄﾛｰﾙのIsKeyboardFocusWithinﾌﾟﾛﾊﾟﾃｨ値をﾋﾞｭｰﾓﾃﾞﾙに伝達するための添付ﾋﾞﾍｲﾋﾞｱ
    /// </summary>
    public class ControlIsKeyboardFocusWithinChangedBehavior
    {
        /// <summary>
        /// ﾌｫｰｶｽを判別できるﾋﾞｭｰﾓﾃﾞﾙの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty FocusableProperty = BehaviorUtil.RegisterAttached(
            "Focusable", typeof(ControlIsKeyboardFocusWithinChangedBehavior), default(IFocusableItem), OnSetFocusableCallback
        );

        /// <summary>
        /// ﾌｫｰｶｽを判別できるﾋﾞｭｰﾓﾃﾞﾙを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetFocusable(DependencyObject target, object value)
        {
            target.SetValue(FocusableProperty, value);
        }

        /// <summary>
        /// ﾌｫｰｶｽを判別できるﾋﾞｭｰﾓﾃﾞﾙを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static IFocusableItem GetFocusable(DependencyObject target)
        {
            return (IFocusableItem)target.GetValue(FocusableProperty);
        }

        /// <summary>
        /// Focusableﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetFocusableCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var control = target as FrameworkElement;

            BehaviorUtil.SetEventHandler(control,
                (fe) => fe.IsKeyboardFocusWithinChanged += Control_IsKeyboardFocusWithinChanged,
                (fe) => fe.IsKeyboardFocusWithinChanged -= Control_IsKeyboardFocusWithinChanged
            );
        }

        /// <summary>
        /// 対象ｺﾝﾄﾛｰﾙ IsKeyboardFocusWithin ﾌﾟﾛﾊﾟﾃｨ変更時ｲﾍﾞﾝﾄ(ﾋﾞｭｰﾓﾃﾞﾙに伝達します)
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void Control_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as FrameworkElement;

            if (control != null)
            {
                GetFocusable(control).IsFocused = control.IsKeyboardFocusWithin;
            }
        }

    }
}
