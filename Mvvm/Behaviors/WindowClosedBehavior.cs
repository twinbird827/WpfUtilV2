using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfUtilV2.Mvvm.Behaviors
{
    /// <summary>
    /// WindowのClosedｲﾍﾞﾝﾄでViewModelをDisposeするためのﾋﾞﾍｲﾋﾞｱです。
    /// </summary>
    public class WindowClosedBehavior
    {
        /// <summary>
        /// Disposableの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty DisposableProperty =
            DependencyProperty.RegisterAttached("Disposable", typeof(IDisposable), typeof(WindowClosedBehavior), new UIPropertyMetadata(OnChangeViewModel));

        /// <summary>
        /// Disposableを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">IDisposable</param>
        public static void SetDisposable(DependencyObject target, object value)
        {
            target.SetValue(DisposableProperty, value);
        }

        /// <summary>
        /// Disposableを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>IDisposable</returns>
        public static IDisposable GetDisposable(DependencyObject target)
        {
            return (IDisposable)target.GetValue(DisposableProperty);
        }

        /// <summary>
        /// Disposableﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnChangeViewModel(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var win = target as Window;

            BehaviorUtil.SetEventHandler(win,
                (fe) => fe.Closed += Window_Closed,
                (fe) => fe.Closed -= Window_Closed
            );
        }

        /// <summary>
        /// ｳｨﾝﾄﾞｳを閉じた時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void Window_Closed(object sender, EventArgs e)
        {
            var win = sender as Window;

            if (win == null)
            {
                return;
            }

            var vm = GetDisposable(win);

            if (vm != null)
            {
                vm.Dispose();
            }
        }
    }
}
