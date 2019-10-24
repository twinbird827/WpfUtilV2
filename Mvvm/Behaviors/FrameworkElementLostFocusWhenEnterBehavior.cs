using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class FrameworkElementLostFocusWhenEnterBehavior
    {
        /// <summary>
        /// このﾋﾞﾍｲﾋﾞｱが有効かどうかの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty IsEnabledProperty = BehaviorUtil.RegisterAttached(
            "IsEnabled", typeof(FrameworkElementLostFocusWhenEnterBehavior), false, OnSetIsEnableCallback
        );

        /// <summary>
        /// このﾋﾞﾍｲﾋﾞｱが有効かどうかを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetIsEnabled(DependencyObject target, object value)
        {
            target.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// このﾋﾞﾍｲﾋﾞｱが有効かどうかを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static bool GetIsEnabled(DependencyObject target)
        {
            return (bool)target.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// ﾛｽﾄﾌｫｰｶｽ時の挙動を表す依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty FocusableProperty = BehaviorUtil.RegisterAttached(
            "Focusable", typeof(FrameworkElementLostFocusWhenEnterBehavior), default(FrameworkElement), null
        );

        /// <summary>
        /// ﾛｽﾄﾌｫｰｶｽ時の挙動を設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetFocusable(DependencyObject target, object value)
        {
            target.SetValue(FocusableProperty, value);
        }

        /// <summary>
        /// ﾛｽﾄﾌｫｰｶｽ時の挙動を取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static FrameworkElement GetFocusable(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(FocusableProperty);
        }

        /// <summary>
        /// IsEnabledﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetIsEnableCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = target as FrameworkElement;
            
            BehaviorUtil.SetEventHandler(element,
                (fe) => fe.PreviewKeyDown += FrameworkElement_PreviewKeyDown,
                (fe) => fe.PreviewKeyDown -= FrameworkElement_PreviewKeyDown
            );
        }

        /// <summary>
        /// ｴﾚﾒﾝﾄ PreviewKeyDown ｲﾍﾞﾝﾄ (Enterｷｰ押下時にﾀﾌﾞを移動します。)
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void FrameworkElement_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                // Enterｷｰ以外は中断
                return;
            }

            var element = sender as FrameworkElement;

            if (element == null)
            {
                // 対応するｴﾚﾒﾝﾄが許容する型ではない場合は中断
                return;
            }

            if (!GetIsEnabled(element))
            {
                return;
            }

            var target = GetFocusable(element);
            if (target != null)
            {
                target.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                Keyboard.ClearFocus();
            }
            else
            {
                var shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

                // 次のﾌｫｰｶｽへ移動する。
                element.MoveFocus(new TraversalRequest(shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next));
            }
            e.Handled = true;
        }
    }
}
