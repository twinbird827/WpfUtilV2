using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WpfUtilV2.Mvvm;

namespace WpfUtilV2.Common
{
    /// <summary>
    /// 経過時間を編集可能なｽﾄｯﾌﾟｳｫｯﾁを提供します。
    /// </summary>
    public class EditableStopwatch : BindableBase
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public EditableStopwatch()
        {
            IsRunning = false;
            Elapsed = TimeSpan.Zero;
            Timer = new Timer(32);
            Timer.Elapsed += (sender, e) =>
            {
                var Next = DateTime.Now;

                Ticks += (Next - Prev).Ticks;
                Prev = Next;
            };
        }

        /// <summary>
        /// 内部的に利用するﾀｲﾏｰ
        /// </summary>
        private Timer Timer { get; set; }

        /// <summary>
        /// 前回ﾀｲﾏｰ日付
        /// </summary>
        private DateTime Prev { get; set; }

        /// <summary>
        /// ﾀｲﾏｰ起動中かどうか
        /// </summary>
        public bool IsRunning
        {
            get => _IsRunning;
            private set => SetProperty(ref _IsRunning, value);
        }
        private bool _IsRunning;

        /// <summary>
        /// 経過時間 (long)
        /// </summary>
        public long Ticks
        {
            get => _Ticks;
            set { if (SetProperty(ref _Ticks, value)) OnPropertyChanged(nameof(Elapsed)); }
        }
        private long _Ticks;

        /// <summary>
        /// 経過時間
        /// </summary>
        public TimeSpan Elapsed
        {
            get => new TimeSpan(Ticks);
            set { Ticks = value.Ticks; }
        }

        /// <summary>
        /// ｽﾄｯﾌﾟｳｫｯﾁを開始します。
        /// </summary>
        public virtual void Start()
        {
            IsRunning = true;
            Prev = DateTime.Now;
            Timer.Start();
        }

        /// <summary>
        /// ｽﾄｯﾌﾟｳｫｯﾁを中断します。
        /// </summary>
        public virtual void Pause()
        {
            IsRunning = false;
            Timer.Stop();
        }

        /// <summary>
        /// ｽﾄｯﾌﾟｳｫｯﾁを停止します。
        /// </summary>
        public virtual void Stop()
        {
            IsRunning = false;
            Timer.Stop();
            Ticks = 0;
        }
    }
}
