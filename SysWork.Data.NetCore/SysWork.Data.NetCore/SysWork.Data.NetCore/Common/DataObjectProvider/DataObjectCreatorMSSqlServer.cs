using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SysWork.Data.NetCore.Common.DataObjectProvider
{
    /// <summary>
    /// Implementation of AbstractDataObjectCreator Class for MSSqlServer.
    /// </summary>
    /// <seealso cref="SysWork.Data.NetCore.Common.DataObjectProvider.AbstractDataObjectCreator" />
    public class DataObjectCreatorMSSqlServer : AbstractDataObjectCreator
    {
        /// <summary>
        /// Gets an DbConnection.
        /// </summary>
        /// <returns>DbConnection (SqlConnection).</returns>
        public override DbConnection GetDbConnection()
        {
            return new SqlConnection();
        }

        /// <summary>
        /// Gets an DbConnection with connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>An DbConnection (SqlConnection)</returns>
        public override DbConnection GetDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Gets an DbConnectionStringBuilder.
        /// </summary>
        /// <returns>
        /// An DbConnectionStringBuilder (SqlConnectionStringBuilder)
        /// </returns>
        public override DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return new SqlConnectionStringBuilder();
        }

        /// <summary>
        /// Gets an IDbConnection (Interface Type).
        /// </summary>
        /// <returns>An IDbConnection (SqlConnection).</returns>
        public override IDbConnection GetIDbConnection()
        {
            return new SqlConnection();
        }

        /// <summary>
        /// Gets an IDbConnection (Interface Type). With the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>IDbConnection (SqlConnection) </returns>
        public override IDbConnection GetIDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Gets an IDbDataParameter.
        /// </summary>
        /// <returns>An IDbDataParameter (SqlParameter)</returns>
        public override IDbDataParameter GetIDbDataParameter()
        {
            return new SqlParameter();
        }

        /// <summary>
        /// Gets the SqlDataAdapter.
        /// </summary>
        /// <returns></returns>
        public override DbDataAdapter GetDbDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
