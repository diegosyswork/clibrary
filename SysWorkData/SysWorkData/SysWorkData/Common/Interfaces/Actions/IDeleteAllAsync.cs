using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{

    // TODO: Implementar agregado 11/03/2021

    /// <summary>
    /// Deletes all records.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDeleteAllAsync<TEntity>
    {
        /// <summary>
        /// Deletes all records in the table.
        /// </summary>
        /// <returns></returns>
        Task<long> DeleteAllAsync();

        /// <summary>
        /// Deletes all records in the table, using custom dbCommand timeout.
        /// </summary>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> DeleteAllAsync(int commandTimeOut);

        /// <summary>
        /// Deletes all records in the table, using an DbConnection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<long> DeleteAllAsync(DbConnection dbConnection);

        /// <summary>
        /// Deletes all records in the table, using an DbConnection and custom dbCommand timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long>  DeleteAllAsync(DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Deletes all records in the table, using an DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long>  DeleteAllAsync(DbTransaction dbTransaction);

        /// <summary>
        /// Deletes all records in the table, using an DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long>  DeleteAllAsync(DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Deletes all records in the table, using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long>  DeleteAllAsync(DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Deletes all records in the table, using an DbConnection and DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long>  DeleteAllAsync(DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);

    }
}
