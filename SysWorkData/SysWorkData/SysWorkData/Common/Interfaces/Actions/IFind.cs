using System.Collections.Generic;
using System.Data;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Finds a list of records by identifier.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IFind<TEntity>
    {
        /// <summary>
        /// Finds a list of records by identifier.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids);
        
        /// <summary>
        /// Finds a list of records by identifier using a custom dbCommand timeout.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, int commandTimeOut);

        /// <summary>
        /// Finds a list of records by identifier using an DbConnection.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection);


        /// <summary>
        /// Finds a list of records by identifier using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Finds a list of records by identifier using an DbTransaction.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction dbTransaction);

        /// <summary>
        /// Finds a list of records by identifier using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Finds a list of records by identifier using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Finds a list of records by identifier using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction,int? commandTimeOut);
    }
}
