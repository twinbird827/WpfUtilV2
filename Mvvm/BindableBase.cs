using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm
{
    [DataContract]
    public class BindableBase : INotifyPropertyChanged, IDisposable
    {
        private BindableBase Source { get; set; }

        public BindableBase()
            : this(null)
        {

        }

        public BindableBase(BindableBase source)
        {
            Source = source;

            if (Source != null)
            {
                Source.PropertyChanged += OnPropertyChanged;
            }
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
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if ((storage == null && value == null) ||
                (storage != null && value != null && storage.Equals(value))) return false;

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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀでｿｰｽを指定した際に、ｿｰｽ内のPropertyChangedｲﾍﾞﾝﾄ発行時に実行されるようになります。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public void AddOnPropertyChanged(PropertyChangedEventHandler handler)
        {
            if (handler != null)
            {
                PropertyChanged += handler;

                Handlers.Add(handler);
            }
        }

        private List<PropertyChangedEventHandler> Handlers { get; set; } = new List<PropertyChangedEventHandler>();

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        private static List<string> DisposedArrays = new List<string>();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //var disposables = this.GetType().GetProperties()
                    //    .Where(p => Attribute.GetCustomAttribute(p, typeof(ExclusionAttribute)) == null)
                    //    .Select(p => p.GetValue(this, null))
                    //    .Where(o => o != null && !this.Equals(o))
                    //    .OfType<BindableBase>()
                    //    .Where(b => !DisposedArrays.Contains(b.Guid))
                    //    .ToArray();

                    //DisposedArrays.Add(this.Guid);

                    // Disposeするﾌﾟﾛﾊﾟﾃｨを取得
                    var disposables = this.GetType().GetProperties()
                        .Where(p => Attribute.GetCustomAttribute(p, typeof(ExclusionAttribute)) == null)
                        .Select(p => p.GetValue(this, null))
                        .OfType<BindableBase>()
                        .Where(o => o != null && !this.Equals(o));

                    foreach (var d in disposables)
                    {
                        try
                        {
                            if (!d.disposedValue)
                            {
                                d.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }

                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    OnDisposing();
                }

                disposedValue = true;

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。
                OnDisposed();
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
        /// Disposeによりﾏﾈｰｼﾞ状態を破棄します。
        /// </summary>
        protected virtual void OnDisposing()
        {

        }

        /// <summary>
        /// Disposeによりｱﾝﾏﾈｰｼﾞﾘｿｰｽを開放します。
        /// </summary>
        protected virtual void OnDisposed()
        {
            // ｿｰｽの後始末
            if (Source != null)
            {
                Source.PropertyChanged -= OnPropertyChanged;
            }
            Source = null;

            // handlerの後始末
            foreach (var handler in Handlers)
            {
                PropertyChanged -= handler;
            }
        }

        /// <summary>
        /// GUID
        /// </summary>
        private string Guid { get; set; } = System.Guid.NewGuid().ToString();

        #endregion

    }
}
