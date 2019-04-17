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

namespace WpfUtilV2.Mvvm.UserControls
{
    /// <summary>
    /// TimeSpanPicker.xaml の相互作用ロジック
    /// </summary>
    public partial class TimeSpanPicker : UserControl
    {
        public TimeSpanPicker()
        {
            InitializeComponent();

            baseContainer.DataContext = this;
        }

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
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                TimeSpan.MinValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnElapsedChanged)
            )
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public double Day
        {
            get { return (double)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        public static DependencyProperty DayProperty = DependencyProperty.Register(nameof(Day),
            typeof(double),
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                -1d,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnDayChanged)
            )
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public double Hour
        {
            get { return (double)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        public static DependencyProperty HourProperty = DependencyProperty.Register(nameof(Hour),
            typeof(double),
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                -1d,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnHourChanged)
            )
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public double Minute
        {
            get { return (double)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        public static DependencyProperty MinuteProperty = DependencyProperty.Register(nameof(Minute),
            typeof(double),
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                -1d,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnMinuteChanged)
            )
        );

        /// <summary>
        /// 入力された経過時間変更時
        /// </summary>
        private static void OnElapsedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var picker = obj as TimeSpanPicker;
            var oldv = ((TimeSpan)picker.GetValue(ElapsedProperty));
            var newv = (TimeSpan)args.NewValue;

            if (newv < TimeSpan.Zero)
            {
                picker.Elapsed = TimeSpan.Zero;
            }
            else
            {
                picker.Day = Math.Floor(newv.TotalDays);
                picker.Hour = (double)newv.Hours;
                picker.Minute = (double)newv.Minutes;
            }
        }

        /// <summary>
        /// 入力された日付変更時
        /// </summary>
        private static void OnDayChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var picker = obj as TimeSpanPicker;
            var oldv = Math.Floor(((TimeSpan)picker.GetValue(ElapsedProperty)).TotalDays);
            var newv = (double)args.NewValue;

            if (oldv != newv)
            {
                picker.Elapsed = picker.Elapsed - TimeSpan.FromDays(oldv - newv);
            }
        }

        /// <summary>
        /// 入力された時間変更時
        /// </summary>
        private static void OnHourChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var picker = obj as TimeSpanPicker;
            var oldv = ((TimeSpan)picker.GetValue(ElapsedProperty)).Hours;
            var newv = (double)args.NewValue;

            if (oldv != newv)
            {
                picker.Elapsed = picker.Elapsed - TimeSpan.FromHours(oldv - newv);
            }
        }

        /// <summary>
        /// 入力された分変更時
        /// </summary>
        private static void OnMinuteChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var picker = obj as TimeSpanPicker;
            var oldv = ((TimeSpan)picker.GetValue(ElapsedProperty)).Minutes;
            var newv = (double)args.NewValue;

            if (oldv != newv)
            {
                picker.Elapsed = picker.Elapsed - TimeSpan.FromMinutes(oldv - newv);
            }
        }
    }
}
