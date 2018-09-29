using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.DaoModel;
using GerdannaDataManager.Daos;
using SysWork.Data.Common;
using SysWork.Data.Common.DbConnectionUtilities;

namespace TestDaoModelDataCommon
{
    public class DataManagerMySQL 
    {
        private static string _connectionString;
        private static EDataBaseEngine _dataBaseEngine;

        public static string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }
        public static EDataBaseEngine DatabaseEngine { get { return _dataBaseEngine; } set { _dataBaseEngine = value; } }

        private static DataManagerMySQL _dataManagerInstance = null;

        public DaoPersonaMySql DaoPersonaMySql { get; private set; }

        private DataManagerMySQL()
        {
            DaoPersonaMySql = new DaoPersonaMySql(ConnectionString);
        }

        public static DataManagerMySQL GetInstance()
        {

            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("No se ha informado la cadena de conexion del DataManager");

            if (_dataManagerInstance == null)
                _dataManagerInstance = new DataManagerMySQL();

            return _dataManagerInstance;
        }

        public DbExecute GetDbExecute()
        {
            return new DbExecute(_connectionString, _dataBaseEngine);
        }
    }
}
