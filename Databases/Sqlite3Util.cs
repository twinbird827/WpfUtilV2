using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Databases
{
    public static class Sqlite3Util
    {
        /// <summary>
        /// Sqlite3用ﾊﾟﾗﾒｰﾀを作成します。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SQLiteParameter CreateParameter(DbType type, object value)
        {
            return new SQLiteParameter()
            {
                DbType = type,
                Value = value
            };
        }

    }
}
