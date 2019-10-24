using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class LabelToolTipBehavior
    {
        /// <summary>
        /// このﾋﾞﾍｲﾋﾞｱが有効かどうかの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty IsEnabledProperty = BehaviorUtil.RegisterAttached(
            "IsEnabled", typeof(LabelToolTipBehavior), false, OnSetIsEnableCallback
        );

        /// <summary>
        /// このﾋﾞﾍｲﾋﾞｱが有効かどうかを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetIsEnabled(DependencyObject target, object value)
        {
            target.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// このﾋﾞﾍｲﾋﾞｱが有効かどうかを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static bool GetIsEnabled(DependencyObject target)
        {
            return (bool)target.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// IsEnabledﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetIsEnableCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var label = target as Label;

            BehaviorUtil.SetEventHandler(label,
                (fe) => fe.SizeChanged += Label_SizeChanged,
                (fe) => fe.SizeChanged -= Label_SizeChanged
            );

            BehaviorUtil.Loaded(label, Label_Loaded);
        }

        private static void Label_Loaded(object sender, RoutedEventArgs e)
        {
            Label_SizeChanged(sender, null);
        }

        /// <summary>
        /// SizeChanged ｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="sender">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void Label_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var label = sender as Label;

            if (label == null)
            {
                // 対応するｴﾚﾒﾝﾄが許容する型ではない場合は中断
                return;
            }

            if (!GetIsEnabled(label))
            {
                return;
            }

            var formattedText = BehaviorUtil.GetFormattedText(label);

            if (label.ActualWidth < formattedText.Width + label.Padding.Left + label.Padding.Right)
            {
                label.ToolTip = label.Content;
            }
            else
            {
                label.ToolTip = null;
            }
        }
    }
}
