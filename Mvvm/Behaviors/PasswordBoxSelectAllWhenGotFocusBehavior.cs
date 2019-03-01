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
    public static class PasswordBoxSelectAllWhenGotFocusBehavior
    {
        /// <summary>
        /// このﾋﾞﾍｲﾋﾞｱが有効かどうかの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PasswordBoxSelectAllWhenGotFocusBehavior), new UIPropertyMetadata(OnSetIsEnabledCallback));

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
        /// IsEnabledﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetIsEnabledCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var control = target as PasswordBox;

            BehaviorUtil.SetEventHandler(control,
                (fe) => fe.GotFocus += PasswordBox_GotFocus,
                (fe) => fe.GotFocus -= PasswordBox_GotFocus
            );
            BehaviorUtil.SetEventHandler(control,
                (fe) => fe.PreviewMouseLeftButtonDown += PasswordBox_PreviewMouseLeftButtonDown,
                (fe) => fe.PreviewMouseLeftButtonDown -= PasswordBox_PreviewMouseLeftButtonDown
            );
        }

        /// <summary>
        /// PasswordBox ﾌｫｰｶｽ取得時ｲﾍﾞﾝﾄ(入力内容を全選択します)
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var passwordbox = sender as PasswordBox;

            if (passwordbox != null && GetIsEnabled(passwordbox))
            {
                passwordbox.SelectAll();
            }
        }

        /// <summary>
        /// PasswordBox ﾏｳｽ左ｸﾘｯｸ時ｲﾍﾞﾝﾄ(ﾌｫｰｶｽを強制的に取得し、ｲﾍﾞﾝﾄﾁｪｰﾝを切断します)
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void PasswordBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var passwordbox = sender as PasswordBox;

            if (passwordbox != null && GetIsEnabled(passwordbox))
            {
                if (!passwordbox.IsFocused)
                {
                    passwordbox.Focus();
                    e.Handled = true;
                }
            }
        }
    }
}
