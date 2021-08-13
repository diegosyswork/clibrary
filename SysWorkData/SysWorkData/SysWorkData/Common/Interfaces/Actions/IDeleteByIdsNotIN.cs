using System.Collections.Generic;
using System.Data;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Deletes a record by the identifier.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDeleteByIdsNotIN<TEntity>
    {
        /// <summary>
        /// Deletes a record by the identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids);

        /// <summary>
        /// Deletes a record by the identifier, using a custom dbCommand timeout.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, int commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier, using an DbConnection.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, IDbConnection dbConnection);

        /// <summary>
        /// Deletes a record by the identifier, using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier, using an DbTransaction.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, IDbTransaction dbTransaction);

        /// <summary>
        /// Deletes a record by the identifier, using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier,using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Deletes a record by the identifier,using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier,using an DbConnection and DbTransaction. Out parameter long returns the count of the records deleted
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);

        /// <summary>
        /// Deletes a record by the identifier,using an DbConnection, DbTransaction and a custom dbCommand timeout. Out parameter long returns the count of the records deleted
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier. No throws exceptions.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, out string errMessage);

        /// <summary>
        /// Deletes a record by the identifier using a custom dbCommand timeout. No throws exceptions.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByIdsNotIN(IEnumerable<object> ids, out string errMessage, int commandTimeOut);
    }
}
