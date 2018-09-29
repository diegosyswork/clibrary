using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common.ObjectResolver
{
    public class DataObjectCreatorSQLite : DataObjectCreator
    {
        public override DbConnection GetDbConnection()
        {
            return new SQLiteConnection();
        }

        public override DbConnection GetDbConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        public override DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return new SQLiteConnectionStringBuilder();
        }

        public override IDbConnection GetIDbConnection()
        {
            return new SQLiteConnection();
        }
        public override IDbConnection GetIDbConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }
        public override IDbDataParameter GetIDbDataParameter()
        {
            return new SQLiteParameter();
        }
    }
}
