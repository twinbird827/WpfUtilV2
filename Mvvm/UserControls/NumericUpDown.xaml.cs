using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfUtilV2.Mvvm.Behaviors;

namespace WpfUtilV2.Mvvm.UserControls
{
    /// <summary>
    /// NumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();

            baseContainer.DataContext = this;

            // ﾌｫｰｶｽ更新時
            IsKeyboardFocusWithinChanged += (sender, e) =>
            {
                // DateTimePicker内の全てのｺﾝﾄﾛｰﾙからﾌｫｰｶｽが離れたら
                if ((bool)e.NewValue == false)
                {
                    var value = Parse(this, Tb.Text, Format);

                    if (value.HasValue)
                    {
                        Value = value.Value;
                    }
                    SetText(this);
                }
            };

            // ﾏｳｽﾎｲｰﾙ時
            MouseWheel += (sender, e) =>
            {
                if (!IsKeyboardFocusWithin) return;

                if (0 < e.Delta)
                {
                    Value++;
                }
                else
                {
                    Value--;
                }
                e.Handled = true;
            };

            // ﾃｷｽﾄﾎﾞｯｸｽ.ｷｰ入力時　Enterｷｰ押下したらﾌｫｰｶｽを移動する。
            Tb.PreviewKeyDown += (sender, e) =>
            {
                if (e.Key != Key.Enter)
                {
                    return;
                }
                var shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

                // 次のﾌｫｰｶｽへ移動する。
                Tb.MoveFocus(new TraversalRequest(shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next));

                // 次のﾌｫｰｶｽへENTERｷｰｲﾍﾞﾝﾄを渡さない。
                e.Handled = true;
            };

            // ﾃｷｽﾄﾎﾞｯｸｽ　ﾌｫｰｶｽ取得時に全選択する。
            TextBoxSelectAllWhenGotFocusBehavior.SetIsEnabled(Tb, true);

        }

        /// <summary>
        /// 上下ﾎﾞﾀﾝ押下時の移動量
        /// </summary>
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static DependencyProperty IntervalProperty = BehaviorUtil.Register(
            nameof(Interval), typeof(NumericUpDown), 1d, null
        );

        /// <summary>
        /// 入力できる数値の許容最小値
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static DependencyProperty MinimumProperty = BehaviorUtil.Register(
            nameof(Minimum), typeof(NumericUpDown), double.MinValue, null
        );

        /// <summary>
        /// 入力できる数値の許容最大値
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static DependencyProperty MaximumProperty = BehaviorUtil.Register(
            nameof(Maximum), typeof(NumericUpDown), double.MaxValue, null
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty ValueProperty = BehaviorUtil.Register(
            nameof(Value), typeof(NumericUpDown), -1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged
        );

        /// <summary>
        /// 数値の表示形式
        /// </summary>
        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static DependencyProperty FormatProperty = BehaviorUtil.Register(
            nameof(Format), typeof(NumericUpDown), "F0", OnFormatChanged, IsValidFormat
        );

        /// <summary>
        /// 入力された日付変更時
        /// </summary>
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SetText(obj as NumericUpDown);
        }

        /// <summary>
        /// 日付書式変更時
        /// </summary>
        private static void OnFormatChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SetText(obj as NumericUpDown);
        }

        /// <summary>
        /// 日付書式変更時の入力値検証
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns>OK: true / NG:false</returns>
        private static bool IsValidFormat(object value)
        {
            var format = value as string;

            if (format == null)
            {
                return false;
            }

            // 適当な数値をﾌｫｰﾏｯﾄしてみて、それが可能かどうかが検証条件
            return Parse(float.MinValue, format).HasValue;
        }

        /// <summary>
        /// 日付を表示項目に設定する。
        /// </summary>
        /// <param name="picker">本ｲﾝｽﾀﾝｽ</param>
        private static void SetText(NumericUpDown updown)
        {
            if (updown != null)
            {
                var value = updown.Value;
                var minimum = updown.Minimum;
                var maximum = updown.Maximum;
                var target = value < minimum
                    ? minimum
                    : maximum < value
                    ? maximum
                    : value;
                SetText(updown, target);
            }
        }

        /// <summary>
        /// 日付を表示項目に設定する。
        /// </summary>
        /// <param name="updown">本ｲﾝｽﾀﾝｽ</param>
        /// <param name="target">表示する日付</param>
        private static void SetText(NumericUpDown updown, double target)
        {
            updown.Tb.Text = target.ToString(updown.Format);
        }

        /// <summary>
        /// 日付文字を日付型に変換する。
        /// </summary>
        /// <param name="target">日付文字</param>
        /// <param name="format">ﾌｫｰﾏｯﾄ</param>
        /// <returns>変換可能：DateTime / 変換不可：null</returns>
        private static double? Parse(string target)
        {
            double value;
            if (double.TryParse(target, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        private static double? Parse(double value, string format)
        {
            try
            {
                return Parse(value.ToString(format));
            }
            catch
            {
                return null;
            }
        }

        private static double? Parse(NumericUpDown updown, string value, string format)
        {
            return Parse(Parse(value) ?? updown.Value, format);
        }

    }
}
