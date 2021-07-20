using System.Data.Common;
using System.Threading.Tasks;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Counts records in the Table.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRecordCountAsync<TEntity>
    {
        /// <summary>
        /// Counts all records in the Table.
        /// </summary>
        /// <returns></returns>
        Task<long> RecordCountAsync();

        /// <summary>
        /// Counts all records in the Table using a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        Task<long> RecordCountAsync(int commandTimeOut);
        
        /// <summary>
        /// Counts all records in the Table using an DbConnection.
        /// </summary>
        /// <returns></returns>
        Task<long> RecordCountAsync(DbConnection dbConnection);

        /// <summary>
        /// Counts all records in the Table using an DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        Task<long> RecordCountAsync(DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(DbTransaction dbTransaction);

        /// <summary>
        /// Counts all records in the Table using an DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Counts all records in the Table using an DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(GenericWhereFilter whereFilter);

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(GenericWhereFilter whereFilter, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbConnection dbConnection);

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
