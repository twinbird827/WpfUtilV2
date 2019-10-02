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
    public static class FrameworkElementChangeIsSelectedWithMouseClickBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty = BehaviorUtil.RegisterAttached(
            "IsEnabled", typeof(FrameworkElementChangeIsSelectedWithMouseClickBehavior), false, OnSetIsEnabledCallback
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

            // PreviewKeyDownｲﾍﾞﾝﾄ時、選択を解除するｷｰ入力をｷｬﾝｾﾙする。
            BehaviorUtil.SetEventHandler(element,
                fe => fe.MouseLeftButtonDown += FrameworkElement_MouseLeftButtonDown,
                fe => fe.MouseLeftButtonDown -= FrameworkElement_MouseLeftButtonDown
            );
        }

        private static void FrameworkElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;

            if (element == null) return;

            if (!GetIsEnabled(element)) return;

            var selectable = element.DataContext as ISingleSelectableItem;

            if (selectable == null) return;

            selectable.SelectableItems.AsParallel().ForAll(s => s.IsSelected = false);
            selectable.IsSelected = true;
        }
    }
}
