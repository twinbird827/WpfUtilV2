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
    public static class ListBoxMustSelectedBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty = BehaviorUtil.RegisterAttached(
            "IsEnabled", typeof(ListBoxMustSelectedBehavior), false, OnSetIsEnabledCallback
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
            var element = sender as ListBox;

            // PreviewKeyDownｲﾍﾞﾝﾄ時、選択を解除するｷｰ入力をｷｬﾝｾﾙする。
            BehaviorUtil.SetEventHandler(element,
                fe => fe.PreviewKeyDown += ListView_PreviewKeyDown,
                fe => fe.PreviewKeyDown -= ListView_PreviewKeyDown
            );
        }

        /// <summary>
        /// ｷｰﾀﾞｳﾝｲﾍﾞﾝﾄ
        /// </summary>
        private static void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!GetIsEnabled(sender as DependencyObject)) return;

            if (e.Key == Key.Space && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                e.Handled = true;
            }
        }
    }
}
