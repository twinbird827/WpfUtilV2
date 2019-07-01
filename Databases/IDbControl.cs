using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Databases
{
    public interface IDbControl : IDisposable
    {
        Task BeginTransaction();

        Task<int> ExecuteNonQueryAsync(string sql, params DbParameter[] parameters);

        Task<object> ExecuteScalarAsync(string sql, params DbParameter[] parameters);

        Task<DbDataReader> ExecuteReaderAsync(string sql, params DbParameter[] parameters);

        Task Rollback();

        Task Commit();
    }
}
