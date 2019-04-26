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
using WpfUtilV2.Extensions;
using WpfUtilV2.Mvvm.Behaviors;

namespace WpfUtilV2.Mvvm.UserControls
{
    /// <summary>
    /// TimeTextPicker.xaml の相互作用ロジック
    /// </summary>
    public partial class TimeTextPicker : UserControl
    {
        public TimeTextPicker()
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

                    if (value.HasValue && Elapsed != value.Value)
                    {
                        Elapsed = value.Value;
                    }
                    else
                    {
                        Tb.Text = Previous;
                    }
                    //SetText(this);
                }
            };

            // ﾏｳｽﾎｲｰﾙ時
            MouseWheel += (sender, e) =>
            {
                if (!IsKeyboardFocusWithin) return;

                if (0 < e.Delta)
                {
                    Value += Interval;
                }
                else
                {
                    Value -= Interval;
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

        public static DependencyProperty IntervalProperty = DependencyProperty.Register(nameof(Interval),
            typeof(double),
            typeof(TimeTextPicker),
            new FrameworkPropertyMetadata(1d, null)
        );

        /// <summary>
        /// 入力できる数値の許容最小値
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum),
            typeof(double),
            typeof(TimeTextPicker),
            new FrameworkPropertyMetadata(0d, null)
        );

        /// <summary>
        /// 入力できる数値の許容最大値
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum),
            typeof(double),
            typeof(TimeTextPicker),
            new FrameworkPropertyMetadata(63072000d, null)
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value),
            typeof(double),
            typeof(TimeTextPicker),
            new FrameworkPropertyMetadata(
                default(double),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnValueChanged)
            )
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public TimeSpan Elapsed
        {
            get { return (TimeSpan)GetValue(ElapsedProperty); }
            set { SetValue(ElapsedProperty, value); }
        }

        public static DependencyProperty ElapsedProperty = DependencyProperty.Register(nameof(Elapsed),
            typeof(TimeSpan),
            typeof(TimeTextPicker),
            new FrameworkPropertyMetadata(
                TimeSpan.MinValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnElapsedChanged)
            )
        );

        /// <summary>
        /// 数値の表示形式
        /// </summary>
        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format),
            typeof(string),
            typeof(TimeTextPicker),
            new FrameworkPropertyMetadata("{0:d'日'hh':'mm':'ss}", OnFormatChanged),
            IsValidFormat
        );

        private string Previous { get; set; }

        /// <summary>
        /// 入力された日付変更時
        /// </summary>
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var picker = obj as TimeTextPicker;

            if (picker == null) return;

            if (picker.Elapsed.TotalSeconds != (double)args.NewValue)
            {
                picker.Elapsed = TimeSpan.FromSeconds((double)args.NewValue);
            }
        }

        /// <summary>
        /// 入力された日付変更時
        /// </summary>
        private static void OnElapsedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var picker = obj as TimeTextPicker;
            var timespan = (TimeSpan)args.NewValue;

            if (picker == null) return;

            if (picker.Value != timespan.TotalSeconds)
            {
                picker.Value = timespan.TotalSeconds;
            }
            SetText(obj as TimeTextPicker);
        }

        /// <summary>
        /// 日付書式変更時
        /// </summary>
        private static void OnFormatChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SetText(obj as TimeTextPicker);
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
            return Parse(TimeSpan.MinValue, format).HasValue;
        }

        /// <summary>
        /// 日付を表示項目に設定する。
        /// </summary>
        /// <param name="picker">本ｲﾝｽﾀﾝｽ</param>
        private static void SetText(TimeTextPicker updown)
        {
            if (updown != null)
            {
                var value = updown.Elapsed;
                var minimum = TimeSpan.FromSeconds(updown.Minimum);
                var maximum = TimeSpan.FromSeconds(updown.Maximum);
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
        private static void SetText(TimeTextPicker updown, TimeSpan target)
        {
            updown.Tb.Text = string.Format(updown.Format, target);
            updown.Previous = updown.Tb.Text;
        }

        /// <summary>
        /// 日付文字を日付型に変換する。
        /// </summary>
        /// <param name="target">日付文字</param>
        /// <param name="format">ﾌｫｰﾏｯﾄ</param>
        /// <returns>変換可能：DateTime / 変換不可：null</returns>
        private static TimeSpan? Parse(string target)
        {
            return target.ToTimeSpan();
        }

        private static TimeSpan? Parse(TimeSpan value, string format)
        {
            try
            {
                return Parse(string.Format(format, value));
            }
            catch
            {
                return null;
            }
        }

        private static TimeSpan? Parse(TimeTextPicker updown, string value, string format)
        {
            return Parse(Parse(value) ?? updown.Elapsed, format);
        }
    }
}
