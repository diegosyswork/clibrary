using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common.DataObjectProvider
{
    public class DataObjectCreatorSQLServer : AbstractDataObjectCreator
    {
        public override DbConnection GetDbConnection()
        {
            return new SqlConnection();
        }
        public override DbConnection GetDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public override DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return new SqlConnectionStringBuilder();
        }

        public override IDbConnection GetIDbConnection()
        {
            return new SqlConnection();
        }
        public override IDbConnection GetIDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        public override IDbDataParameter GetIDbDataParameter()
        {
            return new SqlParameter();
        }
    }
}
