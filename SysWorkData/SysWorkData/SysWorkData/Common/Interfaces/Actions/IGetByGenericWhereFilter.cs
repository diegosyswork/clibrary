using System.Data;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Gets a record that match with the GenericWhereFilter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetByGenericWhereFilter<TEntity>
    {
        /// <summary>
        /// Gets a record that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter);

        /// <summary>
        /// Gets a record that match with the GenericWhereFilter using a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut);

        /// <summary>
        /// Get all an record match with the GenericWhereFilter using an DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection);

        /// <summary>
        /// Gets a record that match with the GenericWhereFilter using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Gets a record that match with the GenericWhereFilter using an DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets a record that match with the GenericWhereFilter using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Gets a record that match with the GenericWhereFilter using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets a record that match with the GenericWhereFilter using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);
    }
}
