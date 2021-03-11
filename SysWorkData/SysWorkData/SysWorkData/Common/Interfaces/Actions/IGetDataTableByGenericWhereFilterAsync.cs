using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Gets an DataTable with the records that match with the GenericWhereFilter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetDataTableByGenericWhereFilterAsync<TEntity>
    {
        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, int commandTimeout);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeout);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeout);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? CommandTimeout);
    }
}
