using System.Data;
using System.Data.Common;
using SysWork.Data;

namespace SysWork.Data.Common.DataObjectProvider
{
    /// <summary>
    /// Abstract Class to Implement an ObjectCreator.
    /// </summary>
    public abstract class AbstractDataObjectCreator
    {
        /// <summary>
        /// Gets an IDbConnection (Interface Type).
        /// </summary>
        /// <returns>An IDbConnection().</returns>
        public abstract IDbConnection GetIDbConnection();

        /// <summary>
        /// Gets an IDbConnection (Interface Type). With the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>An IDbConnection().</returns>
        public abstract IDbConnection GetIDbConnection(string connectionString);

        /// <summary>
        /// Gets an DbConnection.
        /// </summary>
        /// <returns>An DbConnection().</returns>
        public abstract DbConnection GetDbConnection();

        /// <summary>
        /// Gets an DbConnection with the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>An DbConnection().</returns>
        public abstract DbConnection GetDbConnection(string connectionString);

        /// <summary>
        /// Gets an IDbDataParameter.
        /// </summary>
        /// <returns>An IDbDataParameter().</returns>
        public abstract IDbDataParameter GetIDbDataParameter();
        
        /// <summary>
        /// Gets an DbConnectionStringBuilder.
        /// </summary>
        /// <returns>An DbConnectionStringBuilder().</returns>
        public abstract DbConnectionStringBuilder GetDbConnectionStringBuilder();

        /// <summary>
        /// Return an DbDataAdapter.
        /// </summary>
        /// <returns></returns>
        public abstract DbDataAdapter GetDbDataAdapter();

        public abstract DbEntityProvider GetQueryProvider(string connectionString);
        public abstract DbEntityProvider GetQueryProvider(DbConnection dbConnection);
    }
}
