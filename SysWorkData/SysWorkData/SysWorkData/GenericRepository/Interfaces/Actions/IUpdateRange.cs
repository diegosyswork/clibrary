using System.Collections.Generic;
using System.Data;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    /// <summary>
    /// Updates a list of records.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUpdateRange<TEntity>
    {
        /// <summary>
        /// Updates a list of records.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities);

        /// <summary>
        /// Updates a list of records using a custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, int commandTimeOut);

        /// <summary>
        /// Updates a list of records using an DbConnection.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection);

        /// <summary>
        /// Updates a list of records using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Updates a list of records using an DbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbTransaction dbTransaction);

        /// <summary>
        /// Updates a list of records using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Updates a list of records using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Updates a list of records using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Updates a list of records using an DbConnection and DbTransaction. Out long parameter returns count of recordsAffected.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);

        /// <summary>
        /// Updates a list of records using an DbConnection, DbTransaction and a custom dbCommand timeout. Out long parameter returns count of recordsAffected.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);

        /// <summary>
        /// Updates a list of records. No throws exceptions.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, out string errMessage);

        /// <summary>
        /// Updates a list of records using a custom dbCommand timeout. No throws exceptions.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, out string errMessage, int commandTimeOut);
    }

}
