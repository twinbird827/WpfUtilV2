using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUtilV2.Common;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public class TextBoxOfDoubleBehavior
    {
        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定する数値の依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value",
                typeof(double),
                typeof(TextBoxOfDoubleBehavior), 
                new FrameworkPropertyMetadata((double)float.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSetValueCallback)
            );

        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定する数値を設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetValue(DependencyObject target, object value)
        {
            target.SetValue(ValueProperty, value);
        }

        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定する数値を取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static double GetValue(DependencyObject target)
        {
            return (double)target.GetValue(ValueProperty);
        }

        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定するﾌｫｰﾏｯﾄの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached("Format", 
                typeof(string), 
                typeof(TextBoxOfDoubleBehavior),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSetFormatCallback)
            );

        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定するﾌｫｰﾏｯﾄを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetFormat(DependencyObject target, object value)
        {
            target.SetValue(FormatProperty, value);
        }

        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定するﾌｫｰﾏｯﾄを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static string GetFormat(DependencyObject target)
        {
            return (string)target.GetValue(FormatProperty);
        }

        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定する数値の依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        private static DependencyProperty PreviousProperty =
            DependencyProperty.RegisterAttached("Previous", typeof(double), typeof(TextBoxOfDoubleBehavior), new UIPropertyMetadata());

        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定する数値を設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        private static void SetPrevious(DependencyObject target, object value)
        {
            target.SetValue(PreviousProperty, value);
        }

        /// <summary>
        /// ﾃｷｽﾄﾎﾞｯｸｽに設定する数値を取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        private static double GetPrevious(DependencyObject target)
        {
            return (double)target.GetValue(PreviousProperty);
        }

        /// <summary>
        /// Valueﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetValueCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var textbox = target as TextBox;

            BehaviorUtil.SetEventHandler(textbox,
                (fe) => fe.GotFocus += TextBox_GotFocus,
                (fe) => fe.GotFocus -= TextBox_GotFocus
            );
            BehaviorUtil.SetEventHandler(textbox,
                (fe) => fe.LostFocus += TextBox_LostFocus,
                (fe) => fe.LostFocus -= TextBox_LostFocus
            );

            BehaviorUtil.Loaded(textbox, 
                (sender, tmp) => SetText(textbox, (double)e.NewValue)
            );
        }

        /// <summary>
        /// Valueﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetFormatCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var textbox = target as TextBox;
            var value = WpfUtil.GetIsNotNull(GetValue(textbox), textbox.Text, "0");

            BehaviorUtil.Loaded(textbox, 
                (sender, tmp) => SetText(textbox, double.Parse(value))
            );
        }

        /// <summary>
        /// TextBox ﾌｫｰｶｽ取得時ｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;

            if (textbox != null)
            {
                SetPrevious(textbox, double.Parse(textbox.Text));
            }
        }

        /// <summary>
        /// TextBox ﾌｫｰｶｽ喪失時ｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;

            if (textbox != null)
            {
                double tmp; if (double.TryParse(textbox.Text, out tmp))
                {
                    SetText(textbox, tmp);
                    SetValue(textbox, double.Parse(string.Format(GetFormat(textbox), tmp)));
                }
                else
                {
                    SetText(textbox, GetPrevious(textbox));
                }
            }
        }

        /// <summary>
        /// 予め設定されたﾌｫｰﾏｯﾄで数値をﾃｷｽﾄに変換してｵﾌﾞｼﾞｪｸﾄに設定します。
        /// </summary>
        /// <param name="textbox">ｵﾌﾞｼﾞｪｸﾄ</param>
        /// <param name="value">数値</param>
        private static void SetText(TextBox textbox, double value)
        {
            textbox.Text = string.Format(GetFormat(textbox), value);
        }
    }
}
