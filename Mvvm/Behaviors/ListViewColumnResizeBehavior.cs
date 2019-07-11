using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using WpfUtilV2.Extensions;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class ListViewColumnResizeBehavior
    {
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.RegisterAttached("Width", 
                typeof(string), 
                typeof(ListViewColumnResizeBehavior),
                new PropertyMetadata()
        );

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled",
                typeof(bool), 
                typeof(ListViewColumnResizeBehavior),
                new PropertyMetadata(OnSetIsEnabledCallback)
        );

        public static string GetWidth(DependencyObject obj)
        {
            return (string)obj.GetValue(WidthProperty);
        }

        public static void SetWidth(DependencyObject obj, string value)
        {
            obj.SetValue(WidthProperty, value);
        }

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
                fe => fe.SizeChanged += ListView_SizeChanged,
                fe => fe.SizeChanged -= ListView_SizeChanged
            );

            // 初期表示時にSizeChangedｲﾍﾞﾝﾄが発生しなかったのでｺﾝﾄﾛｰﾙ読込後に1回だけ手動で実行する。
            BehaviorUtil.Loaded(element,
                (listview, tmp) => ListView_SizeChanged(listview, null)
            );
        }

        /// <summary>
        /// ListViewｻｲｽﾞ変更時ｲﾍﾞﾝﾄ
        /// </summary>
        private static void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var lv = sender as ListView;

            if (lv == null) return;

            if (!GetIsEnabled(lv)) return;

            var gv = lv.View as GridView;

            if (gv == null) return;

            var spacePercentage = lv.ActualWidth - GetTotalStaticWidth(gv) - GetScrollWidth(lv);
            var totalPercentage = GetTotalPercentage(gv);

            foreach (var column in gv.Columns)
            {
                double width;
                var behaviorWidth = GetWidth(column);

                if (behaviorWidth == null)
                {
                    continue;
                }
                else if (behaviorWidth.EndsWith("*"))
                {
                    if (0 < (width = spacePercentage / totalPercentage * GetPercentage(behaviorWidth)))
                    {
                        column.Width = width;
                        spacePercentage -= width;
                    }
                }
                else if (double.TryParse(behaviorWidth, out width))
                {
                    column.Width = width;
                }
            }

        }

        /// <summary>
        /// 静的に設定された幅の合計を取得します。
        /// </summary>
        /// <param name="gv">GridViewｲﾝｽﾀﾝｽ</param>
        /// <returns>静的に設定された幅の合計</returns>
        private static double GetTotalStaticWidth(GridView gv)
        {
            return gv.Columns
                .Sum(column =>
                {
                    double width;
                    var behaviorWidth = GetWidth(column);
                    if (behaviorWidth == null)
                    {
                        // Behaviorに幅が設定されていない場合はActualWidthを返却
                        return column.ActualWidth;
                    }
                    else if (double.TryParse(behaviorWidth, out width))
                    {
                        // 設定された幅がdoubleに変換できる＝静的な幅
                        return width;
                    }
                    else
                    {
                        // それ以外は比率を表しているので0を返却
                        return 0d;
                    }
                });
        }

        /// <summary>
        /// 全体の比率を取得します。
        /// </summary>
        /// <param name="gv">GridViewｲﾝｽﾀﾝｽ</param>
        /// <returns>全体の比率</returns>
        private static int GetTotalPercentage(GridView gv)
        {
            return gv.Columns.Sum(column => GetPercentage(GetWidth(column)));
        }

        /// <summary>
        /// 比率を取得します。
        /// </summary>
        /// <param name="behaviorWidth">Behaviorに設定された幅</param>
        /// <returns>幅に[\d]*と設定されていた場合：[\d]の値 / それ以外：0</returns>
        /// <remarks>*と設定されていた場合は1を返却する</remarks>
        private static int GetPercentage(string behaviorWidth)
        {
            int percentage;

            if (behaviorWidth == null)
            {
                return 0;
            }
            else if (behaviorWidth == "*" || behaviorWidth == "1*")
            {
                return 1;
            }
            else if (behaviorWidth.EndsWith("*") && int.TryParse(behaviorWidth.Substring(0, behaviorWidth.Length - 1), out percentage))
            {
                return percentage;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// ｽｸﾛｰﾙﾊﾞｰの幅を取得します。
        /// </summary>
        /// <param name="lv">ListViewｲﾝｽﾀﾝｽ</param>
        /// <returns>ｽｸﾛｰﾙﾊﾞｰの幅</returns>
        private static double GetScrollWidth(ListView lv)
        {
            var fe = VisualTreeHelper.GetChild(lv, 0) as FrameworkElement;

            if (fe == null) return 0d;

            var sv = VisualTreeHelper.GetChild(fe, 0) as ScrollViewer;

            if (sv == null) return 0d;

            var sb = sv.Template.FindName("PART_VerticalScrollBar", sv) as ScrollBar;

            if (sb == null) return 0d;

            return sb.ActualWidth;
        }
    }
}
