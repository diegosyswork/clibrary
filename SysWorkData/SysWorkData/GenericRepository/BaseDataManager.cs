using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.DbConnectionUtilities;

namespace SysWork.Data.GenericRepostory
{
    /// <summary>
    /// Abstract class to implement a DAOS manager 
    ///</summary>
    ///<example>
    ///<code>
    ///public class DataManager : DataManagerBase
    ///{
    ///    public DaoPerson DaoPerson { get; private set; }
    ///    
    ///    private static DataManager _dataManagerInstance = null;
    ///    
    ///    // Set's de DataBaseEngine
    ///    private EDataBaseEngine _eDataBaseEngine = EDataBaseEngine.MSSqlServer;
    ///
    ///    private DataManager()
    ///    {
    ///        InitDAOS();
    ///    }
    ///
    ///    public static DataManager GetInstance()
    ///    {
    ///        ValidateInstanceCreation();
    ///
    ///        if (_dataManagerInstance == null)
    ///            _dataManagerInstance = new DataManager();
    ///
    ///        return _dataManagerInstance;
    ///    }
    ///
    ///    public override void InitDAOS()
    ///    {
    ///        DaoPerson = new DaoPerson(ConnectionString, _eDataBaseEngine);
    ///    }
    ///}
    /// 
    /// </code>
    /// </example>
    public abstract class BaseDataManager
    {
        private static string _connectionString;

        /// <summary>Gets or Sets the ConnectionString.</summary>
        /// <value>The ConnectionString.</value>
        public static string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }

        private static EDataBaseEngine _dataBaseEngine;

        /// <summary>Gets or sets the data base engine.</summary>
        /// <value>The EDatabaseEngine.</value>
        public static EDataBaseEngine DataBaseEngine { get { return _dataBaseEngine; } set { _dataBaseEngine = value; } }

        /// <summary>Validates the instance creation.</summary>
        /// <exception cref="ArgumentNullException"></exception>
        protected static void ValidateInstanceCreation()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("The ConnectionString is not set.");
        }

        /// <summary>Initializes the DAOS.</summary>
        public abstract void InitDAOS();
        
        /// <summary>
        /// Devuelve una nueva instancia de un DbExecute con el motor de base de datos del proyecto y el connection string cargado.
        /// </summary>
        /// <returns></returns>
        public DbExecute GetDbExecute()
        {
            return new DbExecute(_connectionString, _dataBaseEngine);
        }

        /// <summary>Gets an DbConnection, corresponding to the database engine</summary>
        /// <returns></returns>
        public DbConnection GetDbConnection()
        {
            return StaticDbObjectProvider.GetDbConnection(_dataBaseEngine, _connectionString);
        }
    }
}
