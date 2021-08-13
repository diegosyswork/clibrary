using System.Data;
using System.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.GenericDataManager
{
    public abstract class BaseInstantiableDataManager
    {
        private readonly string _connectionString;
        public string ConnectionString { get { return _connectionString; } }

        private readonly EDatabaseEngine _databaseEngine;
        public EDatabaseEngine DatabaseEngine { get { return _databaseEngine; } }

        public BaseInstantiableDataManager(string connectionString)
        {
            _databaseEngine = DefaultValues.DefaultDatabaseEngine;
            _connectionString = connectionString;
        }

        public BaseInstantiableDataManager(EDatabaseEngine databaseEngine, string connectionString)
        {
            _databaseEngine = databaseEngine;
            _connectionString = connectionString;
        }

        /// <summary>
        /// Returns a new instance of DbExecutor with the DatabaseEngine.
        /// </summary>
        /// <returns></returns>
        public DbExecutor GetDbExecutor()
        {
            return new DbExecutor(_connectionString, _databaseEngine);
        }

        /// <summary>
        /// Gets the database executor.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public DbExecutor GetDbExecutor(DbConnection dbConnection)
        {
            return new DbExecutor(dbConnection);
        }

        /// <summary>
        /// Gets the database executor.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public DbExecutor GetDbExecutor(DbTransaction dbTransaction)
        {
            return new DbExecutor(dbTransaction);
        }


        /// <summary>Gets an DbConnection, corresponding to the database engine</summary>
        /// <returns></returns>
        public DbConnection GetDbConnection()
        {
            return StaticDbObjectProvider.GetDbConnection(_databaseEngine, _connectionString);
        }

        private DbConnection _persistentConnection = null;

        /// <summary>
        /// Gets a persistent connection.
        /// </summary>
        /// <value>
        /// An persistent DbConnection.
        /// </value>
        public DbConnection PersistentConnection { get { return GetPersistentDbConnection();}}

        private DbConnection GetPersistentDbConnection()
        {
            if ((_persistentConnection == null) || (_persistentConnection.State != ConnectionState.Open))
            {
                _persistentConnection = GetDbConnection();
                _persistentConnection.Open();
            }

            return _persistentConnection;
        }
    }
}
