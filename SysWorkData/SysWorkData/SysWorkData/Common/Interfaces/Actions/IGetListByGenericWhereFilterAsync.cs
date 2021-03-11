using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    // TODO: Implementar agregado el 10/03/2021

    /// <summary>
    /// Gets an IList with the records that match with the GenericWhereFilter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetListByGenericWhereFilterAsync<TEntity>
    {
        /// <summary>
        /// Gets an IList with the records that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter);

        /// <summary>
        /// Gets an IList with the records that match with the GenericWhereFilter using a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, int commandTimeOut);

        /// <summary>
        /// Gets an IList with the records that match with the GenericWhereFilter using and DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, IDbConnection dbConnection);

        /// <summary>
        /// Gets an IList with the records that match with the GenericWhereFilter using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Gets an IList with the records that match with the GenericWhereFilter using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets an IList with the records that match with the GenericWhereFilter using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Gets an IList with the records that match with the GenericWhereFilter using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets an IList with the records that match with the GenericWhereFilter using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction,int? commandTimeOut);
    }
}
