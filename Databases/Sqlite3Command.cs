using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV2.Common;
using WpfUtilV2.Extensions;
using WpfUtilV2.Mvvm.Service;

namespace WpfUtilV2.Databases
{
    public class Sqlite3Command : IDbControl
    {
        /// <summary>
        /// SQLite3ｱｸｾｽ用ｲﾝｽﾀﾝｽを生成します。
        /// </summary>
        public Sqlite3Command(Sqlite3Accessor accessor, SQLiteConnection conn)
        {
            this.accessor = accessor;
            this.command = conn.CreateCommand();
        }

        /// <summary>
        /// ｱｸｾｻ
        /// </summary>
        private Sqlite3Accessor accessor;

        /// <summary>
        /// ｺﾏﾝﾄﾞ
        /// </summary>
        private SQLiteCommand command;

        /// <summary>
        /// 他ｽﾚｯﾄﾞの実行を待機するかどうか
        /// </summary>
        private bool IsWaitAsync { get; set; } = false;

        /// <summary>
        /// ﾄﾗﾝｻﾞｸｼｮﾝを開始します。
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransaction()
        {
            if (!IsWaitAsync)
            {
                IsWaitAsync = true;
                await accessor.WaitAsync();
                await ExecuteNonQueryAsync("BEGIN");
            }
            else
            {
                // ﾄﾗﾝｻﾞｸｼｮﾝを2回開始しようとした場合
                throw new SQLiteException("You have already started a transaction.");
            }
        }

        /// <summary>
        /// SQLを実行します。
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">SQLﾊﾟﾗﾒｰﾀ</param>
        /// <returns>影響を及ぼした件数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params DbParameter[] parameters)
        {
            return await ExecuteAsync(() => command.ExecuteNonQueryAsync(), sql, parameters);
        }

        /// <summary>
        /// SQLを実行します。
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">SQLﾊﾟﾗﾒｰﾀ</param>
        /// <returns>影響を及ぼした件数</returns>
        public async Task<object> ExecuteScalarAsync(string sql, params DbParameter[] parameters)
        {
            return await ExecuteAsync(() => command.ExecuteScalarAsync(), sql, parameters);
        }

        /// <summary>
        /// SQLを実行して結果をDataReaderとして取得します。
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">SQLﾊﾟﾗﾒｰﾀ</param>
        /// <returns>SQLiteDataReader</returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string sql, params DbParameter[] parameters)
        {
            return await ExecuteAsync(() => command.ExecuteReaderAsync(), sql, parameters);
        }

        /// <summary>
        /// SQLの実行結果を戻します。
        /// </summary>
        public async Task Rollback()
        {
            if (IsWaitAsync)
            {
                IsWaitAsync = false;
                await ExecuteNonQueryAsync("ROLLBACK");
                accessor.Release();
            }
            else
            {
                // ﾄﾗﾝｻﾞｸｼｮﾝが開始していない場合
                throw new SQLiteException("Transaction has not started yet.");
            }
        }

        /// <summary>
        /// SQLの実行結果を確定します。
        /// </summary>
        public async Task Commit()
        {
            if (IsWaitAsync)
            {
                IsWaitAsync = false;
                await ExecuteNonQueryAsync("COMMIT");
                accessor.Release();
            }
            else
            {
                // ﾄﾗﾝｻﾞｸｼｮﾝが開始していない場合
                throw new SQLiteException("Transaction has not started yet.");
            }
        }

        /// <summary>
        /// 同期処理用のﾛｯｸｷｰを取得します。
        /// </summary>
        /// <returns></returns>
        private string GetLockKey()
        {
            return $"{accessor.GetLockKey()}.{typeof(Sqlite3Command).FullName}";
        }

        /// <summary>
        /// SQLを実行します。
        /// </summary>
        /// <typeparam name="T">戻り値</typeparam>
        /// <param name="execute">実行内容を表すｱｸｼｮﾝ</param>
        /// <param name="sql">SQL文</param>
        /// <param name="parameters">SQLﾊﾟﾗﾒｰﾀ</param>
        /// <returns></returns>
        private async Task<T> ExecuteAsync<T>(Func<Task<T>> execute, string sql, DbParameter[] parameters)
        {
            var sp = new Stopwatch();
            sp.Start();

            try
            {
                await SemaphoreManager.WaitAsync(GetLockKey());
                command.CommandText = sql;
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
                return await execute();
            }
            catch
            {
                // ｴﾗｰが発生したらｺﾝｿｰﾙに表示
                var sb = new StringBuilder();
                sb.AppendLine($"{sp.Elapsed} ******************************************************************");
                sb.AppendLine(sql.ToString());
                sb.AppendLine(parameters.Select(p => p.Value.ToString()).GetString(","));
                ServiceFactory.MessageService.Debug(sb.ToString());
                throw;
            }
            finally
            {
                if (1000 < sp.ElapsedMilliseconds)
                {
                    // 1秒超えたらｺﾝｿｰﾙに表示
                    var sb = new StringBuilder();
                    sb.AppendLine($"{sp.Elapsed} ******************************************************************");
                    sb.AppendLine(sql.ToString());
                    sb.AppendLine(parameters.Select(p => p.Value.ToString()).GetString(","));
                    ServiceFactory.MessageService.Debug(sb.ToString());
                }
                SemaphoreManager.Release(GetLockKey());
            }
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
                    if (command != null)
                    {
                        command.Dispose();
                        command = null;
                    }

                    if (IsWaitAsync)
                    {
                        accessor.Release();
                    }
                    accessor = null;

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
