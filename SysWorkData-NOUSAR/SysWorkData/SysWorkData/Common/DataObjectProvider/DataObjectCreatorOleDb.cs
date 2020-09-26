using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace SysWork.Data.Common.DataObjectProvider
{
    /// <summary>
    /// Implementation of AbstractDataObjectCreator Class for OleDb.
    /// </summary>
    /// <seealso cref="SysWork.Data.Common.DataObjectProvider.AbstractDataObjectCreator" />
    public class DataObjectCreatorOleDb: AbstractDataObjectCreator
    {
        /// <summary>
        /// Gets an DbConnection.
        /// </summary>
        /// <returns>An DbConnection(OleDbConnection).</returns>
        public override DbConnection GetDbConnection()
        {
            return new OleDbConnection();
        }

        /// <summary>
        /// Gets an DbConnection with the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// An DbConnection(OleDbConnection).
        /// </returns>
        public override DbConnection GetDbConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }

        /// <summary>
        /// Gets an DbConnectionStringBuilder.
        /// </summary>
        /// <returns>
        /// An DbConnectionStringBuilder(OleDbConnectionStringBuilder)
        /// </returns>
        public override DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return new OleDbConnectionStringBuilder();
        }

        /// <summary>
        /// Gets an IDbConnection (Interface Type).
        /// </summary>
        /// <returns>
        /// An IDbConnection(OleDbConnection).
        /// </returns>
        public override IDbConnection GetIDbConnection()
        {
            return new OleDbConnection();
        }

        /// <summary>
        /// Gets an IDbConnection (Interface Type). With the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// An IDbConnection(OleDbConnection).
        /// </returns>
        public override IDbConnection GetIDbConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }

        /// <summary>
        /// Gets an IDbDataParameter.
        /// </summary>
        /// <returns>
        /// An IDbDataParameter(OleDbParameter).
        /// </returns>
        public override IDbDataParameter GetIDbDataParameter()
        {
            return new OleDbParameter();
        }

        public override DbDataAdapter GetDbDataAdapter()
        {
            return new OleDbDataAdapter();
        }
    }
}
