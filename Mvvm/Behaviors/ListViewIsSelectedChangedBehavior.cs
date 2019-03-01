using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfUtilV2.Mvvm.CustomControls;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class ListViewIsSelectedChangedBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled",
                typeof(bool),
                typeof(ListViewIsSelectedChangedBehavior),
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
            var element = sender as ListView;

            // 基本的にListView.SizeChangedｲﾍﾞﾝﾄで列幅を変更する。
            BehaviorUtil.SetEventHandler(element,
                fe => fe.SelectionChanged += ListView_SelectionChanged,
                fe => fe.SelectionChanged -= ListView_SelectionChanged
            );
        }

        /// <summary>
        /// 選択行変更時ｲﾍﾞﾝﾄ
        /// </summary>
        private static void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!GetIsEnabled(sender as DependencyObject)) return;

            foreach (var item in e.RemovedItems.Cast<ISelectableItem>())
                item.IsSelected = false;
            foreach (var item in e.AddedItems.Cast<ISelectableItem>())
                item.IsSelected = true;
        }

    }
}
