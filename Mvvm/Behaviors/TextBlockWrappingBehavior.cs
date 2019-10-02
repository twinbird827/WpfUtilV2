using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public static class TextBlockWrappingBehavior
    {
        /// <summary>
        /// ﾃｷｽﾄﾌﾞﾛｯｸに表示可能な最大行数の依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty MaxLinesProperty = BehaviorUtil.RegisterAttached(
            "MaxLines", typeof(TextBlockWrappingBehavior), int.MinValue, OnSetMaxLinesCallback
        );

        /// <summary>
        /// ﾃｷｽﾄﾌﾞﾛｯｸに表示可能な最大行数を設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">ﾃｷｽﾄﾌﾞﾛｯｸに表示可能な最大行数</param>
        public static void SetMaxLines(DependencyObject target, object value)
        {
            target.SetValue(MaxLinesProperty, value);
        }

        /// <summary>
        /// ﾃｷｽﾄﾌﾞﾛｯｸに表示可能な最大行数を取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        public static int GetMaxLines(DependencyObject target)
        {
            return (int)target.GetValue(MaxLinesProperty);
        }

        /// <summary>
        /// MaxLinesﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        private static void OnSetMaxLinesCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var textblock = target as TextBlock;

            if (textblock == null)
            {
                return;
            }
            if (textblock.TextWrapping == TextWrapping.NoWrap)
            {
                // 改行設定
                textblock.TextWrapping = TextWrapping.Wrap;
            }
            if (textblock.TextTrimming == TextTrimming.None)
            {
                // 省略文字設定
                textblock.TextTrimming = TextTrimming.CharacterEllipsis;
            }

            BehaviorUtil.SetEventHandler(textblock,
                (block) => block.TargetUpdated += TextBlock_TargetUpdated,
                (block) => block.TargetUpdated -= TextBlock_TargetUpdated
            );

            BehaviorUtil.Loaded(textblock, TextBlock_Loaded);
        }

        /// <summary>
        /// TextBlockのLoadedｲﾍﾞﾝﾄ
        /// </summary>
        private static void TextBlock_Loaded(object sender, EventArgs e)
        {
            var textblock = sender as TextBlock;

            if (textblock == null)
            {
                return;
            }

            var binding = BindingOperations.GetBinding(textblock, TextBlock.TextProperty);

            if (binding == null)
            {
                return;
            }

            // Textﾌﾟﾛﾊﾟﾃｨ変更時に通知されるようにする
            BindingOperations.SetBinding(textblock, TextBlock.TextProperty, new Binding()
            {
                Path = binding.Path,
                NotifyOnTargetUpdated = true
            });
        }

        /// <summary>
        /// TextBlockのTargetUpdatedｲﾍﾞﾝﾄ
        /// </summary>
        private static void TextBlock_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var block = sender as TextBlock;

            // 行の高さが設定されている場合はその値、未設定であればFormattedTextの高さを行の高さとする
            var line = double.IsInfinity(block.LineHeight) || double.IsNaN(block.LineHeight)
                ? GetFormattedTextHeight(block)
                : block.LineHeight;

            // 最大高さ設定
            block.MaxHeight = line * GetMaxLines(block);
            block.Height = block.MaxHeight;
        }

        /// <summary>
        /// TextBlockの文字をFormattedText化して、その高さを取得します。
        /// </summary>
        /// <param name="block">ｵﾌﾞｼﾞｪｸﾄ</param>
        private static double GetFormattedTextHeight(TextBlock block)
        {
            var formatted = new FormattedText("1",
                new CultureInfo("ja-jp"),
                FlowDirection.LeftToRight,
                new Typeface(block.FontFamily, block.FontStyle, block.FontWeight, block.FontStretch),
                block.FontSize,
                block.Foreground
            );
            return formatted.Height;
        }
    }
}
