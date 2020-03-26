using System.Data;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    /// <summary>
    /// Interface to Add Entities
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IAdd<TEntity>
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        long Add(TEntity entity);
        
        /// <summary>
        /// Adds the specified entity, and use a custom timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Add(TEntity entity, int commandTimeOut);

        /// <summary>
        /// Adds the specified entity using an DbConnection.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The parameter database connection.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection dbConnection);

        /// <summary>
        /// Adds the specified entity using an DbConnection and custom command timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The parameter database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Adds the specified entity using an DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        long Add(TEntity entity, IDbTransaction dbTransaction);

        /// <summary>
        /// Adds the specified entity using an DbTransaction and custom command timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Adds the specified entity, using and DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Adds the specified entity using an DbConnection and DbTransaction and custom command timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command timeout.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);

        /// <summary>
        /// Adds the specified entity, no throws exceptions.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        long Add(TEntity entity, out string errMessage);

        /// <summary>
        /// Adds the specified entity, no thows exceptions using custom command timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Add(TEntity entity, out string errMessage, int commandTimeOut);

    }
}
