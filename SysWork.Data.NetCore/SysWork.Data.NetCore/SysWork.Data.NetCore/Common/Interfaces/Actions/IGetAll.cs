using System.Collections.Generic;
using System.Data;

namespace SysWork.Data.NetCore.Common.Interfaces.Actions
{
    /// <summary>
    /// Gets all records of the table.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetAll<TEntity>
    {
        /// <summary>
        /// Gets all records of the table.
        /// </summary>
        /// <returns></returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// Gets all records of the table using a custom dbCommand timeout.
        /// </summary>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(int commandTimeOut);

        /// <summary>
        /// Gets all records of the table using an DbConnection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(IDbConnection dbConnection);

        /// <summary>
        /// Gets all records of the table using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Gets all records of the table using an DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(IDbTransaction dbTransaction);

        /// <summary>
        /// Gets all records of the table using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(IDbTransaction dbTransaction, int commandTimeOut);

        /// Gets all records of the table using an DbConnection and DbTransaction.
        IList<TEntity> GetAll(IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets all records of the table using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);
    }
}
