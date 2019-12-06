using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace SysWork.Data.Common.DataObjectProvider
{
    /// <summary>
    /// Implementation of AbstractDataObjectCreator Class for MySql.
    /// </summary>
    /// <seealso cref="SysWork.Data.Common.DataObjectProvider.AbstractDataObjectCreator" />
    public class DataObjectCreatorMySql : AbstractDataObjectCreator
    {
        /// <summary>
        /// Gets an DbConnection.
        /// </summary>
        /// <returns>An DbConnection (MySqlConnection).</returns>
        public override DbConnection GetDbConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// Gets an DbConnection with the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>An DbConnection (MySqlConnection).</returns>
        public override DbConnection GetDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Gets an DbConnectionStringBuilder.
        /// </summary>
        /// <returns>
        /// An DbConnectionStringBuilder (MySqlConnectionStringBuilder).
        /// </returns>
        public override DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return new MySqlConnectionStringBuilder();
        }

        /// <summary>
        /// Gets an IDbConnection (Interface Type).
        /// </summary>
        /// <returns>An IDbConnection (MySqlConnection).</returns>
        public override IDbConnection GetIDbConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// Gets an IDbConnection (Interface Type). With the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>An IDbConnection(MySqlConnection).</returns>
        public override IDbConnection GetIDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
        /// <summary>
        /// Gets an IDbDataParameter.
        /// </summary>
        /// <returns>An IDbDataParameter (MySqlParameter).</returns>
        public override IDbDataParameter GetIDbDataParameter()
        {
            return new MySqlParameter();
        }
    }
}
