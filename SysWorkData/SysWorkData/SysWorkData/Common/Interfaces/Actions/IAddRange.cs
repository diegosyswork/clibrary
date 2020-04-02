using System.Collections.Generic;
using System.Data;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Adds a list of records.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IAddRange<TEntity>
    {
        /// <summary>
        /// Adds a list of records.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities);

        /// <summary>
        /// Adds a list of records, using custom timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, int commandTimeOut);

        /// <summary>
        /// Adds a list of records, out parameter IEnumerable with the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out IEnumerable<object> addedIds);

        /// <summary>
        /// Adds a list of records, using custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out IEnumerable<object> addedIds, int commandTimeOut);

        /// <summary>
        /// Adds a list of records. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out long recordsAffected);

        /// <summary>
        /// Adds a list of records using a custom dbCommand timeout. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out long recordsAffected, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbConnection. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection);

        /// <summary>
        /// Adds a list of records using an DbConnection, and custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbConnection. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out IEnumerable<object> addedIds);

        /// <summary>
        /// Adds a list of records using an DbConnection, and custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out IEnumerable<object> addedIds, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbConnection. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out long recordsAffected);

        /// <summary>
        /// Adds a list of records using an DbConnection and custom dbCommand timeout. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out long recordsAffected, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction);

        /// <summary>
        /// Adds a list of records using an DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbTransaction. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out IEnumerable<object> addedIds);


        /// <summary>
        /// Adds a list of records using an DbTransaction and custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out IEnumerable<object> addedIds, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbTransaction. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out long recordsAffected);

        /// <summary>
        /// Adds a list of records using an DbTransaction and custom dbCommand timeOut. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and custom dbCommand timeOut.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbConnection and DbTransaction. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out IEnumerable<object> addedIds);


        /// <summary>
        /// Adds a list of records using an DbConnetion, dbTransaction and custom dbCommand timeOut. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out IEnumerable<object> addedIds, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbConnection and DbTransaction. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);

        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and custom dbCommand timeout. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut);

        /// <summary>
        /// Adds a list of records. No Throws exceptions.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out string errMessage);

        /// <summary>
        /// Adds a list of records using an custom dbCommand timeOut. No Throws exceptions.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out string errMessage, int commandTimeOut);

        /// <summary>
        /// Adds a list of records. No Throws exceptions. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds);

        /// <summary>
        /// Adds a list of records using an custom dbCommand timeOut. No Throws exceptions. Out parameter IEnumerable returns the Ids of the entities added
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds, int commandTimeOut);


        /// <summary>
        /// Adds a list of records. No Throws exceptions. Out parameter long returns the count of the records added
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="recordsAffected"></param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected);

        /// <summary>
        /// Adds a list of records using custom dbCommand timeout. No Throws exceptions. Out parameter long returns the count of the records added
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected, int commandTimeOut);

        /// <summary>
        /// Adds a list of records using an DbConnection and DbTransaction. Out parameter long returns the count of the records added. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds);

        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and custom dbCommand timeout. Out parameter long returns the count of the records added. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, int? commandTimeOut);
    }
}
