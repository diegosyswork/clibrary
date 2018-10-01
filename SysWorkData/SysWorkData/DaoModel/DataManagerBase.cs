using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common;
using SysWork.Data.Common.DbConnectionUtilities;

namespace SysWork.Data.DaoModel
{
    public abstract class DataManagerBase
    {
        private static string _connectionString;
        public static string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }

        private static EDataBaseEngine _dataBaseEngine;
        public static EDataBaseEngine DataBaseEngine { get { return _dataBaseEngine; } set { _dataBaseEngine = value; } }

        protected static void ValidacionesAlCrearLaInstancia()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("No se ha informado la cadena de conexion del DataManager");
        }

        public abstract void InstanciarDaos();

        public DbExecute GetDbExecute()
        {
            return new DbExecute(_connectionString, _dataBaseEngine);
        }

    }
}
