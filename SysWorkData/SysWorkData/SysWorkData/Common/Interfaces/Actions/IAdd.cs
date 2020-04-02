using System.Data;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Adds a record.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IAdd<TEntity>
    {
        /// <summary>
        /// Adds a record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        long Add(TEntity entity);
        
        /// <summary>
        /// Adds a record and use a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Add(TEntity entity, int commandTimeOut);

        /// <summary>
        /// Adds a record using an DbConnection.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The parameter database connection.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection dbConnection);

        /// <summary>
        /// Adds a record using an DbConnection and custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The parameter database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Adds a record using an DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        long Add(TEntity entity, IDbTransaction dbTransaction);

        /// <summary>
        /// Adds a record using an DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Adds a record using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Adds a record using an DbConnection, DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command timeout.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);

        /// <summary>
        /// Adds a record. No throws exceptions.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        long Add(TEntity entity, out string errMessage);

        /// <summary>
        /// Adds a record using custom dbCommand timeout. No thows exceptions.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Add(TEntity entity, out string errMessage, int commandTimeOut);
    }
}
