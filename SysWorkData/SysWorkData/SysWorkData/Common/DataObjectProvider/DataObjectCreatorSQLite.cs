using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace SysWork.Data.Common.DataObjectProvider
{
    /// <summary>
    /// Implementation of AbstractDataObjectCreator Class for SQLite.
    /// </summary>
    /// <seealso cref="SysWork.Data.Common.DataObjectProvider.AbstractDataObjectCreator" />
    public class DataObjectCreatorSQLite : AbstractDataObjectCreator
    {
        /// <summary>
        /// Gets an DbConnection.
        /// </summary>
        /// <returns>
        /// An DbConnection(SQLiteConnection).
        /// </returns>
        public override DbConnection GetDbConnection()
        {
            return new SQLiteConnection();
        }

        /// <summary>
        /// Gets an DbConnection with the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// An DbConnection(SQLiteConnection).
        /// </returns>
        public override DbConnection GetDbConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// Gets an DbConnectionStringBuilder.
        /// </summary>
        /// <returns>
        /// An DbConnectionStringBuilder(SQLiteConnectionStringBuilder).
        /// </returns>
        public override DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            return new SQLiteConnectionStringBuilder();
        }

        /// <summary>
        /// Gets an IDbConnection (Interface Type).
        /// </summary>
        /// <returns>
        /// An IDbConnection(SQLiteConnection).
        /// </returns>
        public override IDbConnection GetIDbConnection()
        {
            return new SQLiteConnection();
        }

        /// <summary>
        /// Gets an IDbConnection (Interface Type). With the connectionString.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// An IDbConnection(SQLiteConnection).
        /// </returns>
        public override IDbConnection GetIDbConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// Gets an IDbDataParameter.
        /// </summary>
        /// <returns>
        /// An IDbDataParameter(SQLiteParameter).
        /// </returns>
        public override IDbDataParameter GetIDbDataParameter()
        {
            return new SQLiteParameter();
        }
    }
}
