using System.Data;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    /// <summary>
    /// Updates a record.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUpdate<TEntity>
    {
        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        bool Update(TEntity entity);

        /// <summary>
        /// Updates a record using a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Update(TEntity entity, int commandTimeOut);

        /// <summary>
        /// Updates a record using an DbConnection.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection dbConnection);

        /// <summary>
        /// Updates a record using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Updates a record using an DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbTransaction dbTransaction);

        /// <summary>
        /// Updates a record using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Updates a record using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Updates a record using an DbConnection,DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Updates a record using an DbConnection,DbTransaction and out long parameter return the count of recordsAffected.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);

        /// <summary>
        /// Updates a record using an DbConnection,DbTransaction and a custom dbCommand timeout. A out long parameter return the count of recordsAffected.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);

        /// <summary>
        /// Updates a record. No throws exceptions.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool Update(TEntity entity, out string errMessage);

        /// <summary>
        /// Updates a record using a custom dbCommand timeout. No throws exceptions.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Update(TEntity entity, out string errMessage, int commandTimeOut);
    }
}
