using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfUtilV2.Common;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class WindowAdjustmentPositionWhenLoadedBehavior
    {
        private const int Margin = 20;

        public static readonly DependencyProperty IsEnabledProperty = BehaviorUtil.RegisterAttached(
            "IsEnabled", typeof(WindowAdjustmentPositionWhenLoadedBehavior), false, OnSetIsEnabledCallback
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
            if (!WpfUtil.IsDesignMode())
            {
                // Window読込時ｲﾍﾞﾝﾄ
                BehaviorUtil.Loaded(sender as Window, Window_Loaded);
            }
        }

        /// <summary>
        /// Window読込時ｲﾍﾞﾝﾄ
        /// </summary>
        private static void Window_Loaded(object sender, EventArgs e)
        {
            var window = sender as Window;
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
