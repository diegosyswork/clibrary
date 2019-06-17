using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common.DataObjectProvider
{
    public class DataObjectCreatorOleDb: AbstractDataObjectCreator
    {
        public override DbConnection GetDbConnection()
        {
            return new OleDbConnection();
        }

        public override DbConnection GetDbConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }

        public override DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return new OleDbConnectionStringBuilder();
        }

        public override IDbConnection GetIDbConnection()
        {
            return new OleDbConnection();
        }
        public override IDbConnection GetIDbConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }
        public override IDbDataParameter GetIDbDataParameter()
        {
            return new OleDbParameter();
        }
    }
}
