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
        public static DependencyProperty ColumnHeaderProperty =
            DependencyProperty.RegisterAttached("ColumnHeader",
                typeof(FrameworkElement),
                typeof(ScrollViewerSyncBehavior),
                new UIPropertyMetadata(OnHeaderCallback)
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
        public static DependencyProperty RowHeaderProperty =
            DependencyProperty.RegisterAttached("RowHeader",
                typeof(FrameworkElement),
                typeof(ScrollViewerSyncBehavior),
                new UIPropertyMetadata(OnHeaderCallback)
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
        public static DependencyProperty ColumnFooterProperty =
            DependencyProperty.RegisterAttached("ColumnFooter",
                typeof(FrameworkElement),
                typeof(ScrollViewerSyncBehavior),
                new UIPropertyMetadata()
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
        public static DependencyProperty RowFooterProperty =
            DependencyProperty.RegisterAttached("RowFooter",
                typeof(FrameworkElement),
                typeof(ScrollViewerSyncBehavior),
                new UIPropertyMetadata()
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
            var viewer = sender as ScrollViewer ?? ScrollViewerFromFrameworkElement(sender as FrameworkElement);

            BehaviorUtil.SetEventHandler(viewer,
                (sv) => sv.ScrollChanged += ScrollViewer_ScrollChanged,
                (sv) => sv.ScrollChanged -= ScrollViewer_ScrollChanged
            );

            var target = e.NewValue as FrameworkElement;

            BehaviorUtil.SetEventHandler(target,
                (sv) => sv.PreviewMouseWheel += FrameworkElement_MouseWheel,
                (sv) => sv.PreviewMouseWheel -= FrameworkElement_MouseWheel
            );
        }

        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            var hfe = GetColumnHeader(sv);
            var hsv = hfe as ScrollViewer ?? ScrollViewerFromFrameworkElement(hfe);
            var rfe = GetRowHeader(sv);
            var rsv = rfe as ScrollViewer ?? ScrollViewerFromFrameworkElement(rfe);
            var hfo = GetColumnFooter(sv);
            var rfo = GetRowFooter(sv);

            hsv.ScrollToHorizontalOffset(sv.HorizontalOffset);
            rsv.ScrollToVerticalOffset(sv.VerticalOffset);
            if (hfo != null) hfo.Width = GetScrollWidth(sv);
            if (rfo != null) rfo.Height = GetScrollHeight(sv);
        }

        private static void FrameworkElement_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private static ScrollViewer ScrollViewerFromFrameworkElement(FrameworkElement frameworkElement)
        {
            if (VisualTreeHelper.GetChildrenCount(frameworkElement) == 0) return null;

            FrameworkElement child = VisualTreeHelper.GetChild(frameworkElement, 0) as FrameworkElement;

            if (child == null) return null;

            if (child is ScrollViewer)
            {
                return (ScrollViewer)child;
            }

            return ScrollViewerFromFrameworkElement(child);
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
