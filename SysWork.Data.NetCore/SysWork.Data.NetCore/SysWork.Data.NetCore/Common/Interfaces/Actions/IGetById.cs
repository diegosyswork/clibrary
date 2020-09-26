using System.Data;

namespace SysWork.Data.NetCore.Common.Interfaces.Actions
{
    /// <summary>
    /// Gets a record by identifier
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetById<TEntity>
    {
        /// <summary>
        /// Gets a record by identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Gets a record by identifier using a custom dbCommand timeout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetById(object id, int commandTimeOut);

        /// <summary>
        /// Gets a record by identifier using an DbConnection.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbConnection dbConnection);

        /// <summary>
        /// Gets a record by identifier using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Gets a record by identifier using an DbTransaction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets a record by identifier using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Gets a record by identifier using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets a record by identifier using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);
    }
}
