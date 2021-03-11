using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Gets a record by identifier
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetByIdAsync<TEntity>
    {
        /// <summary>
        /// Gets a record by identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Gets a record by identifier using a custom dbCommand timeout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id, int commandTimeOut);

        /// <summary>
        /// Gets a record by identifier using an DbConnection.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id, DbConnection dbConnection);

        /// <summary>
        /// Gets a record by identifier using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Gets a record by identifier using an DbTransaction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id, DbTransaction dbTransaction);

        /// <summary>
        /// Gets a record by identifier using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Gets a record by identifier using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Gets a record by identifier using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
