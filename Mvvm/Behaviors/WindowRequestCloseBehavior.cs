using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public class WindowRequestCloseBehavior
    {
        /// <summary>
        /// Disposableの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(WindowRequestCloseBehavior), new UIPropertyMetadata(OnChangeDialogResult));

        /// <summary>
        /// Disposableを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">IDisposable</param>
        public static void SetDialogResult(DependencyObject target, object value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        /// <summary>
        /// Disposableを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>IDisposable</returns>
        public static bool? GetDialogResult(DependencyObject target)
        {
            return (bool?)target.GetValue(DialogResultProperty);
        }

        /// <summary>
        /// Disposableﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnChangeDialogResult(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var win = target as Window;

            win.DialogResult = (bool?)e.NewValue;
            win.Close();
        }
    }
}
