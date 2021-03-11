using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Updates a list of records.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUpdateRangeAsyc<TEntity>
    {
        /// <summary>
        /// Updates a list of records.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities);

        /// <summary>
        /// Updates a list of records using a custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities, int commandTimeOut);

        /// <summary>
        /// Updates a list of records using an DbConnection.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection);

        /// <summary>
        /// Updates a list of records using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Updates a list of records using an DbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbTransaction dbTransaction);

        /// <summary>
        /// Updates a list of records using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Updates a list of records using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Updates a list of records using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Updates a list of records using an DbConnection and DbTransaction. Out long parameter returns count of recordsAffected.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction, out long recordsAffected);
    }

}
