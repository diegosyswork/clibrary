using System.Data;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Deletes all records that match with the GenericWhereFilter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDeleteByGenericWhereFilter<TEntity>
    {
        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, out long recordsAffected);


        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using a custom dbCommand timeout. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, out long recordsAffected, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, out long recordsAffected);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection and a custom dbCommand timeout. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, out long recordsAffected, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbTransaction. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, out long recordsAffected);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbTransaction and a custom dbCommand timeout. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection and DbTransaction. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);

        /// <summary>
        /// Deletes all records that match with the GenericWhereFilter, using an DbConnection, DbTransaction and a custom dbCommand timeout. Out parameter long returns the count of the records deleted.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
