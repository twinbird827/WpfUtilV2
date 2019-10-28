using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public class ScrollViewerSyncBehavior
    {
        /// <summary>
        /// 列ﾍｯﾀﾞの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty ColumnHeaderProperty = BehaviorUtil.RegisterAttached(
            "ColumnHeader", typeof(ScrollViewerSyncBehavior), default(FrameworkElement), OnHeaderCallback
        );

        /// <summary>
        /// 列ﾍｯﾀﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetColumnHeader(DependencyObject target, object value)
        {
            target.SetValue(ColumnHeaderProperty, value);
        }

        /// <summary>
        /// 列ﾍｯﾀﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static FrameworkElement GetColumnHeader(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(ColumnHeaderProperty);
        }

        /// <summary>
        /// 行ﾍｯﾀﾞの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty RowHeaderProperty = BehaviorUtil.RegisterAttached(
            "RowHeader", typeof(ScrollViewerSyncBehavior), default(FrameworkElement), OnHeaderCallback
        );

        /// <summary>
        /// 行ﾍｯﾀﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetRowHeader(DependencyObject target, object value)
        {
            target.SetValue(RowHeaderProperty, value);
        }

        /// <summary>
        /// 列ﾍｯﾀﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static FrameworkElement GetRowHeader(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(RowHeaderProperty);
        }

        /// <summary>
        /// 列ﾍｯﾀﾞの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty ColumnFooterProperty = BehaviorUtil.RegisterAttached(
            "ColumnFooter", typeof(ScrollViewerSyncBehavior), default(FrameworkElement), OnHeaderCallback
        );

        /// <summary>
        /// 列ﾍｯﾀﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetColumnFooter(DependencyObject target, object value)
        {
            target.SetValue(ColumnFooterProperty, value);
        }

        /// <summary>
        /// 列ﾍｯﾀﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static FrameworkElement GetColumnFooter(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(ColumnFooterProperty);
        }

        /// <summary>
        /// 行ﾍｯﾀﾞの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty RowFooterProperty = BehaviorUtil.RegisterAttached(
            "RowFooter", typeof(ScrollViewerSyncBehavior), default(FrameworkElement), OnHeaderCallback
        );

        /// <summary>
        /// 行ﾍｯﾀﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetRowFooter(DependencyObject target, object value)
        {
            target.SetValue(RowFooterProperty, value);
        }

        /// <summary>
        /// 列ﾍｯﾀﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static FrameworkElement GetRowFooter(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(RowFooterProperty);
        }

        /// <summary>
        /// ﾍｯﾀﾞﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="sender">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnHeaderCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewer = BehaviorUtil.GetScrollViewer(sender as FrameworkElement);

            BehaviorUtil.SetEventHandler(viewer,
                (sv) => sv.ScrollChanged += ScrollViewer_ScrollChanged,
                (sv) => sv.ScrollChanged -= ScrollViewer_ScrollChanged
            );
            
            var target = e.NewValue as FrameworkElement;

            BehaviorUtil.SetEventHandler(target,
                (sv) => sv.PreviewMouseWheel += FrameworkElement_MouseWheel,
                (sv) => sv.PreviewMouseWheel -= FrameworkElement_MouseWheel
            );

            BehaviorUtil.Loaded(viewer, ScrollViewer_Loaded);
        }

        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Refresh(sender);
        }

        private static void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh(sender);
        }

        private static void FrameworkElement_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private static void Refresh(object sender)
        {
            var sv = sender as ScrollViewer;
            var hfe = GetColumnHeader(sv);
            var hsv = BehaviorUtil.GetScrollViewer(hfe);
            var rfe = GetRowHeader(sv);
            var rsv = BehaviorUtil.GetScrollViewer(rfe);
            var hfo = GetColumnFooter(sv);
            var rfo = GetRowFooter(sv);

            if (hsv != null) hsv.ScrollToHorizontalOffset(sv.HorizontalOffset);
            if (rsv != null) rsv.ScrollToVerticalOffset(sv.VerticalOffset);
            if (hfo != null) hfo.Width = GetScrollWidth(sv);
            if (rfo != null) rfo.Height = GetScrollHeight(sv);
        }

        /// <summary>
        /// ｽｸﾛｰﾙﾊﾞｰの幅を取得します。
        /// </summary>
        /// <param name="lv">ListViewｲﾝｽﾀﾝｽ</param>
        /// <returns>ｽｸﾛｰﾙﾊﾞｰの幅</returns>
        private static double GetScrollWidth(ScrollViewer sv)
        {
            var sb = sv.Template.FindName("PART_VerticalScrollBar", sv) as ScrollBar;

            if (sb == null) return 0d;

            return sb.ActualWidth;
        }

        /// <summary>
        /// ｽｸﾛｰﾙﾊﾞｰの高さを取得します。
        /// </summary>
        /// <param name="lv">ListViewｲﾝｽﾀﾝｽ</param>
        /// <returns>ｽｸﾛｰﾙﾊﾞｰの高さ</returns>
        private static double GetScrollHeight(ScrollViewer sv)
        {
            var sb = sv.Template.FindName("PART_HorizontalScrollBar", sv) as ScrollBar;

            if (sb == null) return 0d;

            return sb.ActualHeight;
        }

    }
}
