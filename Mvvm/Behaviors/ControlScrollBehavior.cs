using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class ControlScrollBehavior
    {
        /// <summary>
        /// ｽｸﾛｰﾙを上部先頭まで移動するための依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty ScrollTopProperty = BehaviorUtil.RegisterAttached(
            "ScrollTop", typeof(ControlScrollBehavior), new object(), OnScrollTopCallback
        );

        /// <summary>
        /// ｽｸﾛｰﾙを上部先頭まで移動するﾌﾗｸﾞを設定します。
        /// </summary>
        public static void SetScrollTop(DependencyObject obj, object value)
        {
            obj.SetValue(ScrollTopProperty, value);
        }

        /// <summary>
        /// ｽｸﾛｰﾙを上部先頭まで移動します。
        /// </summary>
        private static void OnScrollTopCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OnScrollCallback(sender, e, (sv) => sv.ScrollToTop());
        }

        /// <summary>
        /// ｽｸﾛｰﾙを左部先頭まで移動するための依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty ScrollLeftProperty = BehaviorUtil.RegisterAttached(
            "ScrollLeft", typeof(ControlScrollBehavior), new object(), OnScrollLeftCallback
        );

        /// <summary>
        /// ｽｸﾛｰﾙを左部先頭まで移動するﾌﾗｸﾞを設定します。
        /// </summary>
        public static void SetScrollLeft(DependencyObject obj, object value)
        {
            obj.SetValue(ScrollLeftProperty, value);
        }

        /// <summary>
        /// ｽｸﾛｰﾙを左部先頭まで移動します。
        /// </summary>
        private static void OnScrollLeftCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OnScrollCallback(sender, e, (sv) => sv.ScrollToLeftEnd());
        }

        /// <summary>
        /// ｽｸﾛｰﾙを下部終端まで移動するための依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty ScrollBottomProperty = BehaviorUtil.RegisterAttached(
            "ScrollBottom", typeof(ControlScrollBehavior), new object(), OnScrollBottomCallback
        );

        /// <summary>
        /// ｽｸﾛｰﾙを下部終端まで移動するﾌﾗｸﾞを設定します。
        /// </summary>
        public static void SetScrollBottom(DependencyObject obj, object value)
        {
            obj.SetValue(ScrollBottomProperty, value);
        }

        /// <summary>
        /// ｽｸﾛｰﾙを下部終端まで移動します。
        /// </summary>
        private static void OnScrollBottomCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OnScrollCallback(sender, e, (sv) => sv.ScrollToBottom());
        }

        /// <summary>
        /// ｽｸﾛｰﾙを右部終端まで移動するための依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty ScrollRightProperty = BehaviorUtil.RegisterAttached(
            "ScrollRight", typeof(ControlScrollBehavior), new object(), OnScrollRightCallback
        );

        /// <summary>
        /// ｽｸﾛｰﾙを右部終端まで移動するﾌﾗｸﾞを設定します。
        /// </summary>
        public static void SetScrollRight(DependencyObject obj, object value)
        {
            obj.SetValue(ScrollRightProperty, value);
        }

        /// <summary>
        /// ｽｸﾛｰﾙを右部終端まで移動します。
        /// </summary>
        private static void OnScrollRightCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OnScrollCallback(sender, e, (sv) => sv.ScrollToRightEnd());
        }

        /// <summary>
        /// ScrollViewerの処理
        /// </summary>
        /// <param name="action">ｽｸﾛｰﾙ移動処理</param>
        private static void OnScrollCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e, Action<ScrollViewer> action)
        {
            var sv = BehaviorUtil.GetScrollViewer(sender);

            if (sv != null)
            {
                action(sv);
            }
        }
    }
}
