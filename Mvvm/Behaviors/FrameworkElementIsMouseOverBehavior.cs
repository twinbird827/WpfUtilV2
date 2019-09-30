using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfUtilV2.Mvvm.CustomControls;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class FrameworkElementIsMouseOverBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled",
                typeof(bool),
                typeof(FrameworkElementIsMouseOverBehavior),
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
            var element = sender as FrameworkElement;

            BehaviorUtil.SetEventHandler(element,
                fe => fe.MouseEnter += FrameworkElement_MouseEnter,
                fe => fe.MouseEnter -= FrameworkElement_MouseEnter
            );
            BehaviorUtil.SetEventHandler(element,
                fe => fe.MouseLeave += FrameworkElement_MouseLeave,
                fe => fe.MouseLeave -= FrameworkElement_MouseLeave
            );
        }

        /// <summary>
        /// ﾏｳｽが要素の境界内に入った際のｲﾍﾞﾝﾄ
        /// </summary>
        private static void FrameworkElement_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIsMouseOver(sender, true);
        }

        /// <summary>
        /// ﾏｳｽが要素の境界内から出た際のｲﾍﾞﾝﾄ
        /// </summary>
        private static void FrameworkElement_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIsMouseOver(sender, false);
        }

        private static void ChangeIsMouseOver(object sender, bool isMouseOver)
        {
            if (!GetIsEnabled(sender as DependencyObject)) return;

            var fe = sender as FrameworkElement;
            var dc = fe.DataContext as IIsMouseOverItem;

            if (dc != null)
            {
                dc.IsMouseOver = isMouseOver;
            }
        }
    }
}
