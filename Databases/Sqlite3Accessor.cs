using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV2.Common;

namespace WpfUtilV2.Databases
{
    public class Sqlite3Accessor : IDisposable
    {
        /// <summary>
        /// SQLite3ｱｸｾｽ用ｲﾝｽﾀﾝｽを生成します。
        /// </summary>
        protected Sqlite3Accessor(string path, bool isReadOnly)
        {
            var work = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            var full = Path.Combine(work, path);

            Directory.CreateDirectory(Path.GetDirectoryName(full));

            var connectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = full,
                DefaultIsolationLevel = System.Data.IsolationLevel.ReadCommitted,
                SyncMode = SynchronizationModes.Off,
                JournalMode = SQLiteJournalModeEnum.Wal,
                //FailIfMissing = true,
                ReadOnly = isReadOnly,
                Pooling = true,
                CacheSize = 65535
            };

            Sqlite3File = path;

            conn = new SQLiteConnection(connectionString.ToString());
            conn.Open();
        }

        /// <summary>
        /// ｺﾈｸｼｮﾝ
        /// </summary>
        private SQLiteConnection conn;

        /// <summary>
        /// ｲﾝｽﾀﾝｽが開いているSqlite3ﾌｧｲﾙﾊﾟｽ
        /// </summary>
        private string Sqlite3File { get; set; }

        /// <summary>
        /// SQLｺﾏﾝﾄﾞを取得します。
        /// </summary>
        /// <returns></returns>
        public Sqlite3Command GetCommand()
        {
            return new Sqlite3Command(this, conn);
        }

        /// <summary>
        /// 同期実行が必要なSQL実行を待機します。
        /// </summary>
        /// <returns></returns>
        internal async Task WaitAsync()
        {
            await SemaphoreManager.WaitAsync(GetLockKey());
        }

        /// <summary>
        /// 実行完了を他ｽﾚｯﾄﾞに通知します。
        /// </summary>
        internal void Release()
        {
            SemaphoreManager.Release(GetLockKey());
        }

        /// <summary>
        /// 同期処理用のﾛｯｸｷｰを取得します。
        /// </summary>
        /// <returns></returns>
        internal string GetLockKey()
        {
            return $"{typeof(Sqlite3Accessor).FullName}.{Sqlite3File}";
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                        conn = null;
                    }
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~Sqlite3Accessor() {
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
