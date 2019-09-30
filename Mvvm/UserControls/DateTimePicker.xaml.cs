using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
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
using WpfUtilV2.Common;
using WpfUtilV2.Mvvm.Behaviors;

namespace WpfUtilV2.Mvvm.UserControls
{
    /// <summary>
    /// DateTimePicker.xaml の相互作用ロジック
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();

            baseContainer.DataContext = this;

            // ｶﾚﾝﾀﾞｰで値を選択時
            CalCalendar.SelectedDatesChanged += (sender, e) =>
            {
                CalButton.IsChecked = false;
                SelectedDate = CalCalendar.SelectedDate.Value;
            };

            // ﾌｫｰｶｽ更新時
            IsKeyboardFocusWithinChanged += (sender, e) =>
            {
                // DateTimePicker内の全てのｺﾝﾄﾛｰﾙからﾌｫｰｶｽが離れたら
                if ((bool)e.NewValue == false)
                {
                    var datetimes = DateFormats
                        .Select(format => Parse(CalText.Text, format))
                        .Where(datetime => datetime.HasValue)
                        .ToArray();

                    if (datetimes.Any())
                    {
                        SelectedDate = datetimes.First().Value;
                    }
                    SetText(this);
                }
            };

            // ﾃｷｽﾄﾎﾞｯｸｽ.ｷｰ入力時　Enterｷｰ押下したらﾌｫｰｶｽを移動する。
            CalText.PreviewKeyDown += (sender, e) =>
            {
                if (e.Key != Key.Enter)
                {
                    return;
                }
                var shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

                // 次のﾌｫｰｶｽへ移動する。
                CalText.MoveFocus(new TraversalRequest(shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next));

                // 次のﾌｫｰｶｽへENTERｷｰｲﾍﾞﾝﾄを渡さない。
                e.Handled = true;
            };

            // ﾃｷｽﾄﾎﾞｯｸｽ　ﾌｫｰｶｽ取得時に全選択する。
            TextBoxSelectAllWhenGotFocusBehavior.SetIsEnabled(CalText, true);
        }

        /// <summary>
        /// 入力された日付
        /// </summary>
        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(nameof(SelectedDate),
            typeof(DateTime),
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(
                DateTime.Now,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSelectedDateChanged)
            )
        );

        /// <summary>
        /// 入力された日付
        /// </summary>
        public string DisplayFormat
        {
            get { return (string)GetValue(DisplayFormatProperty); }
            set { SetValue(DisplayFormatProperty, value); }
        }

        public static readonly DependencyProperty DisplayFormatProperty = DependencyProperty.Register(nameof(DisplayFormat),
            typeof(string),
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(DefaultDateFormats.First(), OnDateFormatChanged)
        );

        /// <summary>
        /// 日付書式ﾘｽﾄ(先頭の書式で表示する)
        /// </summary>
        public string[] DateFormats
        {
            get { return (string[])GetValue(DateFormatsProperty); }
            set { SetValue(DateFormatsProperty, value); }
        }

        public static readonly DependencyProperty DateFormatsProperty = DependencyProperty.Register(nameof(DateFormats),
            typeof(string[]), 
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(DefaultDateFormats, OnDateFormatChanged),
            IsValidDateFormats
        );

        private static string[] DefaultDateFormats => new[]
        {
            // 最終的なﾌｫｰﾏｯﾄ
            "yy/MM/dd HH:mm:ss",
            // 許容するﾌｫｰﾏｯﾄ(年月日区切り="/")
            "yyyy/M/d H:m:s",
            "yyyy/M/d H:m:s",
            "yyyy/M/d H:m",
            "yyyy/M/d H",
            "yyyy/M/d",
            "yy/M/d H:m:s",
            "yy/M/d H:m:s",
            "yy/M/d H:m",
            "yy/M/d H",
            "yy/M/d",
            "M/d H:m:s",
            "M/d H:m",
            "M/d H",
            "M/d",
            // 許容するﾌｫｰﾏｯﾄ(年月日区切り="-")
            "yyyy-M-d H:m:s",
            "yyyy-M-d H:m",
            "yyyy-M-d H",
            "yyyy-M-d",
            "yy-M-d H:m:s",
            "yy-M-d H:m",
            "yy-M-d H",
            "yy-M-d",
            "M-d H:m:s",
            "M-d H:m",
            "M-d H",
            "M-d",
            // 許容するﾌｫｰﾏｯﾄ(年月日区切りなし
            "yyyyMMdd HHmmss",
            "yyyyMMdd HHmm",
            "yyyyMMdd HH",
            "yyyyMMdd",
            "yyMMdd HHmmss",
            "yyMMdd HHmm",
            "yyMMdd HH",
            "yyMMdd",
            "MMdd HHmmss",
            "MMdd HHmm",
            "MMdd HH",
            "MMdd",
            // 許容するﾌｫｰﾏｯﾄ(時分秒のみ)
            "H:m:s",
            "H:m",
            "HHmmss"
        };

        /// <summary>
        /// 入力された日付の許容最小値
        /// </summary>
        public DateTime Minimum
        {
            get { return (DateTime)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum),
            typeof(DateTime),
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(DateTime.Now.AddYears(-1), null)
        );

        /// <summary>
        /// 入力された日付の許容最大値
        /// </summary>
        public DateTime Maximum
        {
            get { return (DateTime)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum),
            typeof(DateTime),
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(DateTime.Now.AddYears(+1), null)
        );

        /// <summary>
        /// 入力された日付の許容最大値
        /// </summary>
        public bool IsShowIcon
        {
            get { return (bool)GetValue(IsShowIconProperty); }
            set { SetValue(IsShowIconProperty, value); }
        }

        public static DependencyProperty IsShowIconProperty = DependencyProperty.Register(nameof(IsShowIcon),
            typeof(bool),
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(true, null)
        );

        /// <summary>
        /// 入力された日付の許容最大値
        /// </summary>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
            typeof(ImageSource),
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(WpfUtil.ToImageSource(Properties.Resources.calendar), null)
        );

        /// <summary>
        /// 入力された日付変更時
        /// </summary>
        private static void OnSelectedDateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SetText(obj as DateTimePicker);
        }

        /// <summary>
        /// 日付書式変更時
        /// </summary>
        private static void OnDateFormatChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SetText(obj as DateTimePicker);
        }

        /// <summary>
        /// 日付書式変更時の入力値検証
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns>OK: true / NG:false</returns>
        private static bool IsValidDateFormats(object value)
        {
            var formats = value as string[];

            if (formats == null)
            {
                return false;
            }

            // 設定された日付初期全て変換できることが検証条件
            return formats.All(format => Parse(DateTime.Now.ToString(format), format).HasValue);
        }

        /// <summary>
        /// 日付を表示項目に設定する。
        /// </summary>
        /// <param name="picker">本ｲﾝｽﾀﾝｽ</param>
        private static void SetText(DateTimePicker picker)
        {
            if (picker != null)
            {
                var datetime = picker.SelectedDate;
                var minimum = picker.Minimum;
                var maximum = picker.Maximum;
                var target = datetime < minimum
                    ? minimum
                    : maximum < datetime
                    ? maximum
                    : datetime;
                SetText(picker, target);
            }
        }

        /// <summary>
        /// 日付を表示項目に設定する。
        /// </summary>
        /// <param name="picker">本ｲﾝｽﾀﾝｽ</param>
        /// <param name="target">表示する日付</param>
        private static void SetText(DateTimePicker picker, DateTime target)
        {
            picker.CalText.Text = target.ToString(picker.DisplayFormat);
        }

        /// <summary>
        /// 日付文字を日付型に変換する。
        /// </summary>
        /// <param name="value">日付文字</param>
        /// <param name="format">ﾌｫｰﾏｯﾄ</param>
        /// <returns>変換可能：DateTime / 変換不可：null</returns>
        private static DateTime? Parse(string value, string format)
        {
            DateTime datetime;
            if (DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out datetime))
            {
                return datetime;
            }
            else
            {
                return null;
            }
        }

    }
}
