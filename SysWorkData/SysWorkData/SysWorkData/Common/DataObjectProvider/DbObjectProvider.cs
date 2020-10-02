using System;
using System.Data;
using System.Data.Common;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.DataObjectProvider
{
    /// <summary>
    /// This Class provides Database Objects to diferents Database Engines.
    /// Implement the Abstract Factory Method Pattern.
    /// </summary>
    ///<remarks>
    /// This class, depending of the databaseEngine <see cref="Common.ValueObjects.EDatabaseEngine"/> passed in the constructor, 
    /// is responsible for providing the different Database Objects 
    /// using the "DataObjectCreators" that implement AbstractDataObjectCreator.
    /// </remarks> 
    public class DbObjectProvider
    {
        private AbstractDataObjectCreator _dataObjectCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbObjectProvider"/> class.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public DbObjectProvider(EDatabaseEngine databaseEngine)
        {
            switch (databaseEngine)
            {
                case EDatabaseEngine.MSSqlServer:
                    _dataObjectCreator = new DataObjectCreatorMSSqlServer();
                    break;
                case EDatabaseEngine.SqLite:
                    _dataObjectCreator = new DataObjectCreatorSQLite();
                    break;
                case EDatabaseEngine.OleDb:
                    _dataObjectCreator = new DataObjectCreatorOleDb();
                    break;
                case EDatabaseEngine.MySql:
                    _dataObjectCreator = new DataObjectCreatorMySql();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
            }
        }

        /// <summary>
        /// Gets an IDbConnection. The object is closed.
        /// </summary>
        /// <returns>
        /// An instantiated IDbConnection depending of the database engine. The object is closed.
        /// </returns>
        public IDbConnection GetIDbConnection()
        {
            return _dataObjectCreator.GetIDbConnection();
        }

        /// <summary>
        /// Gets an IDbConnection. The object is closed.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// An instantiated IdbConnection depending of the database engine with the connectionString atribute. The object is closed.
        /// </returns>
        public IDbConnection GetIDbConnection(string connectionString)
        {
            return _dataObjectCreator.GetIDbConnection(connectionString);
        }

        /// <summary>
        /// Gets an DbConnection. The object is closed.
        /// </summary>
        /// <returns>
        /// An instantiated DbConnection depending of the database engine. The object is closed.
        /// </returns>
        public DbConnection GetDbConnection()
        {
            return _dataObjectCreator.GetDbConnection();
        }

        /// <summary>
        /// Gets an DbConnection. The object is closed.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// An instantiated DbConnection depending of the database engine with the connectionString. The object is closed.
        /// </returns>
        public DbConnection GetDbConnection(string connectionString)
        {
            return _dataObjectCreator.GetDbConnection(connectionString);
        }

        /// <summary>
        /// Gets an IDbDataParameter.
        /// </summary>
        /// <returns>
        /// An instantiated IDbDataParameter depending of the database engine.
        /// </returns>
        public IDbDataParameter GetIDbDataParameter()
        {
            return _dataObjectCreator.GetIDbDataParameter();
        }

        /// <summary>
        /// Gets an DbConnectionStringBuilder.
        /// </summary>
        /// <returns>
        /// An instantiated DbConnectionStringBuilder depending of the database engine..
        /// </returns>
        public DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return _dataObjectCreator.GetDbConnectionStringBuilder();
        }

        /// <summary>
        /// Gets an DbDataAdapter.
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter GetDbDataAdapter()
        {
            return _dataObjectCreator.GetDbDataAdapter();
        }
    }
}
