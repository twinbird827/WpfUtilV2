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

        public static DependencyProperty ElapsedProperty = BehaviorUtil.Register(
            nameof(Elapsed), typeof(TimeSpanPicker), TimeSpan.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnElapsedChanged
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public double Day
        {
            get { return (double)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        public static DependencyProperty DayProperty = BehaviorUtil.Register(
            nameof(Day), typeof(TimeSpanPicker), -1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDayChanged
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public double Hour
        {
            get { return (double)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        public static DependencyProperty HourProperty = BehaviorUtil.Register(
            nameof(Hour), typeof(TimeSpanPicker), -1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHourChanged
        );

        /// <summary>
        /// ﾊﾞｲﾝﾃﾞｨﾝｸﾞﾊﾟﾗﾒｰﾀ
        /// </summary>
        public double Minute
        {
            get { return (double)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        public static DependencyProperty MinuteProperty = BehaviorUtil.Register(
            nameof(Minute), typeof(TimeSpanPicker), -1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMinuteChanged
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
