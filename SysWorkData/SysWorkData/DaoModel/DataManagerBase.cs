using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.DbConnectionUtilities;

namespace SysWork.Data.DaoModel
{
    public abstract class DataManagerBase
    {
        /// <summary>
        /// 
        ///     Para implementar esta clase se debera crear el patron singleton
        /// 
        ///     Ej:
        /*
        public class DataManager : DataManagerBase
        {
            public DaoCARD DaoCard { get; private set; }

            private static DataManager _dataManagerInstance = null;
            private EDataBaseEngine _eDataBaseEngine = EDataBaseEngine.MSSqlServer;
            
            private DataManager()
            {
                InstanciarDaos();
            }

            public static DataManager GetInstance()
            {
                ValidacionesAlCrearLaInstancia();

                if (_dataManagerInstance == null)
                    _dataManagerInstance = new DataManager();

                return _dataManagerInstance;
            }

            public override void InstanciarDaos()
            {
                DaoCard = new DaoCARD(ConnectionString, _eDataBaseEngine);
            }
        }
        */
        /// 
        /// 
        /// 
        /// </summary>
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
        /// <summary>
        /// Devuelve una nueva instancia de un DbExecute con el motor de base de datos del proyecto y el connection string cargado.
        /// </summary>
        /// <returns></returns>
        public DbExecute GetDbExecute()
        {
            return new DbExecute(_connectionString, _dataBaseEngine);
        }

        public DbConnection GetDbConnection()
        {
            return StaticDbObjectProvider.GetDbConnection(_dataBaseEngine, _connectionString);
        }
    }
}
