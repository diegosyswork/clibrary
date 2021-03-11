using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{
    public interface IGetAllAsync<TEntity>
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets all records of the table using a custom dbCommand timeout.
        /// </summary>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync(int commandTimeOut);

        /// <summary>
        /// Gets all records of the table using an DbConnection asynchronous.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync(DbConnection dbConnection);

        /// <summary>
        /// Gets all records of the table using an DbConnection and a custom dbCommand timeout asynchronous.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync(DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Gets all records of the table using an DbTransaction asynchronous.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync(DbTransaction dbTransaction);

        /// <summary>
        /// Gets all records of the table using an DbTransaction and a custom dbCommand timeout asynchronous.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync(DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Gets all records of the table using an DbConnection and a DbTransaction asynchronous.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync(DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Gets all records of the table using an DbConnection, DbTransaction and a custom dbCommand timeout asynchronous.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync(DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
