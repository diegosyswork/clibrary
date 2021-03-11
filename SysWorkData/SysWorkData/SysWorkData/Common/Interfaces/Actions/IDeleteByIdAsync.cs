using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Deletes a record by the identifier.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDeleteByIdAsync<TEntity>
    {
        /// <summary>
        /// Deletes a record by the identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id);

        /// <summary>
        /// Deletes a record by the identifier, using a custom dbCommand timeout.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, int commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier, using an DbConnection.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection);

        /// <summary>
        /// Deletes a record by the identifier, using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier, using an DbTransaction.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, DbTransaction dbTransaction);

        /// <summary>
        /// Deletes a record by the identifier, using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier,using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Deletes a record by the identifier,using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Deletes a record by the identifier,using an DbConnection and DbTransaction. Out parameter long returns the count of the records deleted
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, DbTransaction dbTransaction, out long recordsAffected);

        /// <summary>
        /// Deletes a record by the identifier,using an DbConnection, DbTransaction and a custom dbCommand timeout. Out parameter long returns the count of the records deleted
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, DbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
