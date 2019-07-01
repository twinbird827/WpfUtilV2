using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfUtilV2.Common;

namespace WpfUtilV2.Mvvm
{
    [DataContract]
    public class BindableBase : INotifyPropertyChanged, IDisposable
    {
        protected Stopwatch Stopwatch { get; set; }
        protected void StartStopwatch()
        {
            Stopwatch.Restart();
        }
        protected void Writeline([CallerMemberName] String methodname = null)
        {
            Console.WriteLine($"{DateTime.Now.ToString("MMdd HHmmss")} {Stopwatch.Elapsed.ToString(@"mm\:ss\.fffffff")} {WpfUtil.TotalKBString.PadLeft(10, ' ')} {string.Concat(GetType().Name, ".", methodname)}");
        }
        public BindableBase()
        {
            Stopwatch = new Stopwatch();
            //StartStopwatch();
            //Writeline();
        }

        /// <summary>
        /// プロパティの変更を通知するためのマルチキャスト イベント。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティが既に目的の値と一致しているかどうかを確認します。必要な場合のみ、
        /// プロパティを設定し、リスナーに通知します。
        /// </summary>
        /// <typeparam name="T">プロパティの型。</typeparam>
        /// <param name="storage">get アクセス操作子と set アクセス操作子両方を使用したプロパティへの参照。</param>
        /// <param name="value">プロパティに必要な値。</param>
        /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
        /// この値は省略可能で、
        /// CallerMemberName をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
        /// <returns>値が変更された場合は true、既存の値が目的の値に一致した場合は
        /// false です。</returns>
        protected bool SetProperty<T>(ref T storage, T value, bool isDiposeOld = false, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            if (isDiposeOld)
            {
                // 入替前にDisposeできるものはする。
                var disposable = storage as IDisposable; if (disposable != null) disposable.Dispose();
            }

            // ﾌﾟﾛﾊﾟﾃｨ値変更
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// プロパティ値が変更されたことをリスナーに通知します。
        /// </summary>
        /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
        /// この値は省略可能で、
        /// <see cref="CallerMemberNameAttribute"/> をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, GetPropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// ﾌﾟﾛﾊﾟﾃｨ変更時ｲﾍﾞﾝﾄを追加します。
        /// </summary>
        /// <param name="bindable">追加元のｲﾝｽﾀﾝｽ</param>
        /// <param name="handler">追加するｲﾍﾞﾝﾄの中身</param>
        public void AddOnPropertyChanged(BindableBase bindable, PropertyChangedEventHandler handler)
        {
            if (handler != null)
            {
                PropertyChanged -= handler;
                PropertyChanged += handler;

                bindable.Disposed += (sender, e) =>
                {
                    PropertyChanged -= handler;
                };
            }
        }

        /// <summary>
        /// ﾌﾟﾛﾊﾟﾃｨ変更時のｲﾍﾞﾝﾄ引数を使いまわすためのﾘｽﾄ
        /// </summary>
        private static Dictionary<string, PropertyChangedEventArgs> CreatePropertyChangedEventArgs { get; set; } = new Dictionary<string, PropertyChangedEventArgs>();

        /// <summary>
        /// ﾌﾟﾛﾊﾟﾃｨ変更時のｲﾍﾞﾝﾄ引数を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private PropertyChangedEventArgs GetPropertyChangedEventArgs(string name)
        {
            if (CreatePropertyChangedEventArgs.ContainsKey(name))
            {
                // 既に作成済なら使いまわす
                return CreatePropertyChangedEventArgs[name];
            }
            else
            {
                // 作成していない場合は作成する
                CreatePropertyChangedEventArgs[name] = new PropertyChangedEventArgs(name);
                return GetPropertyChangedEventArgs(name);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    //StartStopwatch();
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    Disposed?.Invoke(this, new EventArgs());
                    //Writeline();
                    Stopwatch = null;
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~BindableBase() {
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

        /// <summary>
        /// ｲﾝｽﾀﾝｽ破棄時のｲﾍﾞﾝﾄ
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// GUID
        /// </summary>
        protected string Guid { get; set; } = System.Guid.NewGuid().ToString();

        public override string ToString()
        {
            return $"{base.ToString()} {Guid}";
        }
        /// <summary>
        /// ｲﾝｽﾀﾝｽと指定した別のBindableBaseの値が同値か比較します。
        /// </summary>
        /// <param name="obj">比較対象のｲﾝｽﾀﾝｽ</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var bindable = obj as BindableBase;

            if (bindable == null)
            {
                return false;
            }
            else
            {
                return Guid.Equals(bindable.Guid);
            }
        }

        /// <summary>
        /// このｲﾝｽﾀﾝｽのﾊｯｼｭｺｰﾄﾞを返却します。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        #endregion

    }
}
