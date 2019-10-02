using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class ListViewSelectionChangedWhenButtonClickBehavior
    {
        public static readonly DependencyProperty ListViewProperty = BehaviorUtil.RegisterAttached(
            "ListView", typeof(ListViewSelectionChangedWhenButtonClickBehavior), default(ListView), OnSetListViewCallback
        );

        public static ListView GetListView(DependencyObject obj)
        {
            return (ListView)obj.GetValue(ListViewProperty);
        }

        public static void SetListView(DependencyObject obj, bool value)
        {
            obj.SetValue(ListViewProperty, value);
        }

        private static void OnSetListViewCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as Button;

            // 基本的にListView.SizeChangedｲﾍﾞﾝﾄで列幅を変更する。
            BehaviorUtil.SetEventHandler(element,
                fe => fe.Click += Button_Click,
                fe => fe.Click -= Button_Click
            );
        }

        /// <summary>
        /// ﾎﾞﾀﾝ押下時ｲﾍﾞﾝﾄ
        /// </summary>
        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button == null) return;

            var lv = GetListView(button) as ListView;

            if (lv == null) return;

            lv.SelectedItem = button.DataContext;
        }

    }
}
