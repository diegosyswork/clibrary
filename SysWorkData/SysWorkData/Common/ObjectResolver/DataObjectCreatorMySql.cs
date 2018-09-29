using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common.ObjectResolver
{
    public class DataObjectCreatorMySql : DataObjectCreator
    {
        public override DbConnection GetDbConnection()
        {
            return new MySqlConnection();
        }

        public override DbConnection GetDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public override DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return new MySqlConnectionStringBuilder();
        }

        public override IDbConnection GetIDbConnection()
        {
            return new MySqlConnection();
        }
        public override IDbConnection GetIDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
        public override IDbDataParameter GetIDbDataParameter()
        {
            return new MySqlParameter();
        }
    }
}
