using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Adds a record.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IAddAsync<TEntity>
    {
        /// <summary>
        /// Adds a record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity);

        /// <summary>
        /// Adds a record and use a custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity, int commandTimeOut);

        /// <summary>
        /// Adds a record using an DbConnection.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The parameter database connection.</param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity, DbConnection dbConnection);

        /// <summary>
        /// Adds a record using an DbConnection and custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The parameter database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Adds a record using an DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity, DbTransaction dbTransaction);

        /// <summary>
        /// Adds a record using an DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Adds a record using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Adds a record using an DbConnection, DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command timeout.</param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
