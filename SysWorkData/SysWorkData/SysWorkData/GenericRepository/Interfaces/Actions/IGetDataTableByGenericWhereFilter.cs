using System.Data;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    /// <summary>
    /// Gets an DataTable with the records that match with the GenericWhereFilter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetDataTableByGenericWhereFilter<TEntity>
    {
        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns></returns>
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeout);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns></returns>
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeout);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns></returns>
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeout);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets an DataTable with the records that match with the GenericWhereFilter using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? CommandTimeout);
    }
}
