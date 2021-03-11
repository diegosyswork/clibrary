using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{

    // TODO: Implementar agregado el 10/03/2021
    /// <summary>
    /// Adds a list of records.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IAddRangeAsync<TEntity>
    {
        /// <summary>
        /// Adds a list of records.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IList<TEntity> entities);

        /// <summary>
        /// Adds a list of records, using custom timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IList<TEntity> entities, int commandTimeOut);


        /// <summary>
        /// Adds a list of records using an DbConnection. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection);

        /// <summary>
        /// Adds a list of records using an DbConnection, and custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection, int commandTimeOut);


        /// <summary>
        /// Adds a list of records using an DbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IList<TEntity> entities, DbTransaction dbTransaction);

        /// <summary>
        /// Adds a list of records using an DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IList<TEntity> entities, DbTransaction dbTransaction, int commandTimeOut);


        /// <summary>
        /// Adds a list of records using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and custom dbCommand timeOut.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction, int commandTimeOut);
    }
}
