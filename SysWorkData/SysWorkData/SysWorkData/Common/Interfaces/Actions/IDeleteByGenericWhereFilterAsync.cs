using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Deletes all records that match with the GenericWhereFilter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDeleteByGenericWhereFilterAsync<TEntity>
    {
        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, int commandTimeOut);


        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut);


        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut);


        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection, DbTransaction and a custom dbCommand timeout. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
