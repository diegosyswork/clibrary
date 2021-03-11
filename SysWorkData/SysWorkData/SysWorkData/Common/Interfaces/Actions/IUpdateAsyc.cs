using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Updates a record.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUpdateAsync<TEntity>
    {
        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates a record using a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, int commandTimeOut);

        /// <summary>
        /// Updates a record using an DbConnection.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection);

        /// <summary>
        /// Updates a record using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Updates a record using an DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, DbTransaction dbTransaction);

        /// <summary>
        /// Updates a record using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Updates a record using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Updates a record using an DbConnection,DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction, int commandTimeOut);
    }
}
