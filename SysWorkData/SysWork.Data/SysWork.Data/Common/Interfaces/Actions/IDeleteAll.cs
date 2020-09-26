using System.Data;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Deletes all records.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDeleteAll<TEntity>
    {
        /// <summary>
        /// Deletes all records in the table.
        /// </summary>
        /// <returns></returns>
        long DeleteAll();

        /// <summary>
        /// Deletes all records in the table, using custom dbCommand timeout.
        /// </summary>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long DeleteAll(int commandTimeOut);

        /// <summary>
        /// Deletes all records in the table, using an DbConnection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        long DeleteAll(IDbConnection dbConnection);

        /// <summary>
        /// Deletes all records in the table, using an DbConnection and custom dbCommand timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long DeleteAll(IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Deletes all records in the table, using an DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        long DeleteAll(IDbTransaction dbTransaction);

        /// <summary>
        /// Deletes all records in the table, using an DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long DeleteAll(IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Deletes all records in the table, using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        long DeleteAll(IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Deletes all records in the table, using an DbConnection and DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long DeleteAll(IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);

        /// <summary>
        /// Deletes all records in the table. No throws exceptions.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool DeleteAll(out string errMessage);

        /// <summary>
        /// Deletes all records in the table, using a custom dbCommand timeout. No throws exceptions.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteAll(out string errMessage, int commandTimeOut);

    }
}
