using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common.DataObjectProvider
{
    public class DbObjectProvider
    {
        AbstractDataObjectCreator _dataObjectCreator;
        public DbObjectProvider(EDataBaseEngine dataBaseEngine)
        {
            switch (dataBaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    _dataObjectCreator = new DataObjectCreatorSQLServer();
                    break;
                case EDataBaseEngine.SqLite:
                    _dataObjectCreator = new DataObjectCreatorSQLite();
                    break;
                case EDataBaseEngine.OleDb:
                    _dataObjectCreator = new DataObjectCreatorOleDb();
                    break;
                case EDataBaseEngine.MySql:
                    _dataObjectCreator = new DataObjectCreatorMySql();
                    break;
            }
        }

        public IDbConnection GetIDbConnection()
        {
            return _dataObjectCreator.GetIDbConnection();
        }
        public IDbConnection GetIDbConnection(string connectionString)
        {
            return _dataObjectCreator.GetIDbConnection(connectionString);
        }
        public DbConnection GetDbConnection()
        {
            return _dataObjectCreator.GetDbConnection();
        }
        public DbConnection GetDbConnection(string connectionString)
        {
            return _dataObjectCreator.GetDbConnection(connectionString);
        }
        public IDbDataParameter GetIDbDataParameter()
        {
            return _dataObjectCreator.GetIDbDataParameter();
        }
        public DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return _dataObjectCreator.GetDbConnectionStringBuilder();
        }
    }
}
