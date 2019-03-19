using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WpfUtilV2.Common
{
    public class AsyncTimer : IDisposable
    {
        public AsyncTimer()
        {
            Timer = new Timer(1);
            Timer.Elapsed += (sender, e) =>
            {
                try
                {
                    if (CanExecute && NextExecuteDate <= DateTime.Now)
                    {
                        CanExecute = false;
                        NextExecuteDate = NextExecuteDate + Interval;

                        Tick.Invoke(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    if (UnhandledException != null)
                    {
                        UnhandledException.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
                    }
                    else
                    {
                        throw;
                    }
                }
            };
        }

        /// <summary>
        /// 内部的に利用するﾀｲﾏｰ
        /// </summary>
        private Timer Timer { get; set; }

        /// <summary>
        /// 最後にｲﾍﾞﾝﾄを発行した日時
        /// </summary>
        private DateTime NextExecuteDate { get; set; }

        /// <summary>
        /// 1回のｲﾍﾞﾝﾄが完了したかどうか
        /// </summary>
        private bool CanExecute { get; set; } = true;

        /// <summary>
        /// ﾀｲﾏｰの起動間隔
        /// </summary>
        public TimeSpan Interval //{ get; set; } = TimeSpan.FromMilliseconds(1);
        {
            get { return _Interval; }
            set
            {
                _Interval = value;
                NextExecuteDate = DateTime.Now + value;
            }
        }
        private TimeSpan _Interval = TimeSpan.FromMilliseconds(1);

        /// <summary>
        /// ﾀｲﾏｰを起動します。
        /// </summary>
        public void Start()
        {
            WaitCompleted(false);
            NextExecuteDate = DateTime.Now + Interval;
            Timer.Start();
        }

        /// <summary>
        /// ﾀｲﾏｰを停止します。
        /// </summary>
        public void Stop()
        {
            Timer.Stop();
        }

        /// <summary>
        /// ｲﾍﾞﾝﾄが完了したことを通知します。
        /// </summary>
        public void Completed()
        {
            while (NextExecuteDate + Interval < DateTime.Now)
            {
                // 長い処理を考慮してNextExecuteDateが直前になるように調整
                NextExecuteDate += Interval;
            }

            CanExecute = true;
        }

        /// <summary>
        /// 実行中のｲﾍﾞﾝﾄが完了するまで待機します。
        /// </summary>
        private void WaitCompleted(bool disposing)
        {
            // 破棄中は1回分の間隔だけ待って完了にする。
            var begin = DateTime.Now;
            while (!CanExecute && (!disposing || DateTime.Now < begin + Interval))
            {
                System.Threading.Thread.Sleep(16);
            }
        }

        /// <summary>
        /// 指定した間隔で実行するｲﾍﾞﾝﾄ
        /// </summary>
        public event EventHandler Tick;

        public event UnhandledExceptionEventHandler UnhandledException;

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。

                    // ﾀｲﾏｰ停止
                    Timer.Stop();

                    // 最後の処理が完了するまで待機
                    WaitCompleted(true);

                    // ﾘｿｰｽ破棄
                    Tick = null;
                    Timer = null;
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~AsyncTimer() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
