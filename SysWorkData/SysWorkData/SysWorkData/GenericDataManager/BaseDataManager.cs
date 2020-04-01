using System;
using System.Data.Common;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericDataManager.Intefaces;

namespace SysWork.Data.GenericDataManager
{
    #region DOCUMENTATION class
    /// <summary>
    /// Generic Repository Manager, to use, inherits from this, 
    /// and implement IDataManager interface. Create public property's
    /// of dataObjects and Init those.
    /// </summary>
    /// <typeparam name="T">The Manager Class</typeparam>
    /// <example>
    /// <![CDATA[
    ///   <summary>
    ///   Inherits of BaseDataManager and Implement IDataManager
    ///   </summary>
    ///   public class DataManager : BaseDataManager<DataManager>, IDataManager
    ///   {
    ///
    ///    /// <summary>
    ///    /// Add's a dataObjects to Manage.
    ///    /// </summary>
    ///
    ///    public PersonRepository PersonRepository { get; private set; }
    ///    public StateRepository StateRepository { get; private set; }
    ///
    ///    /// <summary>
    ///    /// Prevents a default instance of the <see cref="DataManager"/> class from being created.
    ///    /// </summary>
    ///    private DataManager()
    ///    {
    ///    }
    ///
    ///    /// <summary>
    ///    /// Initializes the DataObjects.
    ///    /// </summary>
    ///    void IDataManager.InitDataObjects()
    ///    {
    ///        StateRepository = new StateRepository(ConnectionString, DataBaseEngine);
    ///        PersonRepository = new PersonRepository(ConnectionString, DataBaseEngine);
    ///    }
    ///}
    /// ]]>
    /// </example>
    #endregion
    public abstract class BaseDataManager<T> where T: class, IDataManager
    {
        private static T _TInstance = null;

        /// <summary>
        /// Gets the instance of this singleton.
        /// </summary>
        public static T GetInstance()
        {
            ValidateInstanceCreation();


            if (_TInstance == null)
            {
                _TInstance = CreateInstanceOfT();
                _TInstance.InitDataObjects();
            }

            return _TInstance;
        }

        /// <summary>
        /// Creates an instance of T via reflection since T's constructor is expected to be private.
        /// </summary>
        /// <returns></returns>
        private static T CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }

        private static string _connectionString = null;
        /// <summary>Gets or Sets the ConnectionString.</summary>
        /// <value>The ConnectionString.</value>
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                if ((_connectionString != value) && (_TInstance != null))
                    _TInstance = null;

                _connectionString = value;
            }
        }

        private static EDataBaseEngine _dataBaseEngine;
        /// <summary>Gets or sets the dataBase engine.</summary>
        /// <value>The EDatabaseEngine.</value>
        public static EDataBaseEngine DataBaseEngine
        {
            get
            {
                return _dataBaseEngine;
            }
            set
            {
                if ((_dataBaseEngine != value) && (_TInstance !=null))
                    _TInstance = null;

                _dataBaseEngine = value;
            }
        }

        /// <summary>Validates the instance creation.</summary>
        /// <exception cref="ArgumentNullException"></exception>
        protected static void ValidateInstanceCreation()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("The ConnectionString is not set.");
        }

        /// <summary>
        /// Returns a new instance of DbExecutor with the DataBaseEngine.
        /// </summary>
        /// <returns></returns>
        public DbExecutor GetDbExecutor()
        {
            return new DbExecutor(_connectionString, _dataBaseEngine);
        }

        /// <summary>Gets an DbConnection, corresponding to the database engine</summary>
        /// <returns></returns>
        public DbConnection GetDbConnection()
        {
            return StaticDbObjectProvider.GetDbConnection(_dataBaseEngine, _connectionString);
        }

        private DbConnection _persistentConnection = null;

        /// <summary>
        /// Gets a persistent connection.
        /// </summary>
        /// <value>
        /// An persistent DbConnection.
        /// </value>
        public DbConnection PersistentConnection { get { return GetPersistentDbConnection(); } }

        private DbConnection GetPersistentDbConnection()
        {
            if (_persistentConnection == null)
            {
                _persistentConnection = GetDbConnection();
            }
            else
            {
                if (_persistentConnection.State != System.Data.ConnectionState.Open)
                    _persistentConnection = GetDbConnection();
            }
            return _persistentConnection;
        }

    }
}
