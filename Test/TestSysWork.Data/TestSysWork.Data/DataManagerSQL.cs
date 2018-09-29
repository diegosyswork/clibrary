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
    public class DataManagerSQL 
    {
        private static string _connectionString;
        private static EDataBaseEngine _dataBaseEngine;

        public static string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }
        public static EDataBaseEngine DatabaseEngine { get { return _dataBaseEngine; } set { _dataBaseEngine = value; } }

        private static DataManagerSQL _dataManagerInstance = null;

        public DaoPersonaSql DaoPersonaSql { get; private set; }

        private DataManagerSQL()
        {
            DaoPersonaSql = new DaoPersonaSql(ConnectionString);
        }

        public static DataManagerSQL GetInstance()
        {

            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("No se ha informado la cadena de conexion del DataManager");

            if (_dataManagerInstance == null)
                _dataManagerInstance = new DataManagerSQL();

            return _dataManagerInstance;
        }
        public DbExecute GetDbExecute()
        {
            return new DbExecute(_connectionString, _dataBaseEngine);
        }

    }
}
