using System;
using System.Data.Common;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepostoryManager.Intefaces;

namespace SysWork.Data.GenericRepostoryManager
{
    /// <summary>
    /// Generic Repository Manager, to use, inherits from this, 
    /// and implement IRepositoryManager interface. Create public property's
    /// of repositories and Init those.
    /// </summary>
    /// <typeparam name="T">The Manager Class</typeparam>
    /// <example>
    /// <![CDATA[
    /// <summary>
    /// Inherits of BaseGenericRepositoryManager and Implement IRepositoryManager
    /// </summary>
    ///public class RepositoryManager : BaseGenericRepositoryManager<RepositoryManager>, IRepositoryManager
    ///{
    ///
    ///    /// <summary>
    ///    /// Add's a Repositories to Manage.
    ///    /// </summary>
    ///
    ///    public PersonRepository PersonRepository { get; private set; }
    ///    public StateRepository StateRepository { get; private set; }
    ///
    ///    /// <summary>
    ///    /// Prevents a default instance of the <see cref="RepositoryManager"/> class from being created.
    ///    /// </summary>
    ///    private RepositoryManager()
    ///    {
    ///    }
    ///
    ///    /// <summary>
    ///    /// Initializes the repositories.
    ///    /// </summary>
    ///    void IRepositoryManager.InitRepositories()
    ///    {
    ///        StateRepository = new StateRepository(ConnectionString, DataBaseEngine);
    ///        PersonRepository = new PersonRepository(ConnectionString, DataBaseEngine);
    ///    }
    ///}

    /// ]]>
    /// </example>
    public abstract class BaseGenericRepositoryManager<T> where T: class, IRepositoryManager
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
                _TInstance.InitRepositories();
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
        protected DbExecutor GetDbExecutor()
        {
            return new DbExecutor(_connectionString, _dataBaseEngine);
        }

        /// <summary>Gets an DbConnection, corresponding to the database engine</summary>
        /// <returns></returns>
        protected DbConnection GetDbConnection()
        {
            return StaticDbObjectProvider.GetDbConnection(_dataBaseEngine, _connectionString);
        }
    }

}
