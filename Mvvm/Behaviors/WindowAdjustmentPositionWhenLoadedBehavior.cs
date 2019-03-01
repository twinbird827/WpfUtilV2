using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class WindowAdjustmentPositionWhenLoadedBehavior
    {
        private const int Margin = 20;

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled",
                typeof(bool),
                typeof(WindowAdjustmentPositionWhenLoadedBehavior),
                new PropertyMetadata(OnSetIsEnabledCallback)
        );

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        private static void OnSetIsEnabledCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;

            // 初期表示時にSizeChangedｲﾍﾞﾝﾄが発生しなかったのでｺﾝﾄﾛｰﾙ読込後に1回だけ手動で実行する。
            window.Dispatcher.BeginInvoke(
                new Action(() => Window_Loaded(window)),
                DispatcherPriority.Loaded
            );
        }

        /// <summary>
        /// Window読込時ｲﾍﾞﾝﾄ
        /// </summary>
        private static void Window_Loaded(Window window)
        {
            var owner = window.Owner;

            if (owner == null) return;

            var ot = owner.WindowState == WindowState.Maximized ? 0 : owner.Top;
            var ol = owner.WindowState == WindowState.Maximized ? 0 : owner.Left;

            if (ot + owner.ActualHeight < window.Top + window.ActualHeight + Margin)
            {
                window.Top -= (window.Top + window.ActualHeight + Margin) - (ot + owner.ActualHeight);
            }
            if (ol + owner.ActualWidth < window.Left + window.ActualWidth + Margin)
            {
                window.Left -= (window.Left + window.ActualWidth + Margin) - (ol + owner.ActualWidth);
            }
        }
    }
}
