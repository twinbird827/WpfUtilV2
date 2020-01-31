using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class ControlToolTipBehavior
    {
        /// <summary>
        /// ﾂｰﾙﾁｯﾌﾟの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty ToolTipProperty = BehaviorUtil.RegisterAttached(
            "ToolTip", typeof(ControlToolTipBehavior), "", OnSetToolTipCallback
        );

        /// <summary>
        /// ﾂｰﾙﾁｯﾌﾟを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">ﾂｰﾙﾁｯﾌﾟ</param>
        public static void SetToolTip(DependencyObject target, object value)
        {
            target.SetValue(ToolTipProperty, value);
        }

        /// <summary>
        /// ﾂｰﾙﾁｯﾌﾟを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ﾂｰﾙﾁｯﾌﾟ</returns>
        public static object GetToolTip(DependencyObject target)
        {
            return target.GetValue(ToolTipProperty);
        }

        /// <summary>
        /// ToolTipﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetToolTipCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var control = target as Control;

            BehaviorUtil.Loaded(control, Control_Loaded);
        }

        private static void Control_Loaded(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;

            if (control == null)
            {
                // 対応するｴﾚﾒﾝﾄが許容する型ではない場合は中断
                return;
            }

            var tooltip = GetToolTip(control);

            if (tooltip == null)
            {
                return;
            }

            if (tooltip is FrameworkElement)
            {
                var formattedText = BehaviorUtil.GetFormattedText((FrameworkElement)tooltip);

                if (!string.IsNullOrEmpty(formattedText.Text))
                {
                    control.ToolTip = tooltip;
                }
            }
            else if (tooltip is string && !string.IsNullOrEmpty((string)tooltip))
            {
                control.ToolTip = tooltip;
            }
            else
            {
                control.ToolTip = tooltip;
            }
        }

    }
}
