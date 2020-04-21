using System;
using System.Data.Common;
using System.Linq.Expressions;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Counts records in the Table.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface ICount<TEntity>
    {
        /// <summary>
        /// Counts all records in the Table.
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Counts all records in the Table using a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        long Count(int commandTimeOut);
        
        /// <summary>
        /// Counts all records in the Table using an DbConnection.
        /// </summary>
        /// <returns></returns>
        long Count(DbConnection dbConnection);

        /// <summary>
        /// Counts all records in the Table using an DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        long Count(DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an DbTransaction.
        /// </summary>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <returns></returns>
        long Count(DbTransaction DbTransaction);

        /// <summary>
        /// Counts all records in the Table using an DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(DbTransaction DbTransaction, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <returns></returns>
        long Count(DbConnection dbConnection, DbTransaction DbTransaction);

        /// <summary>
        /// Counts all records in the Table using an DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(DbConnection dbConnection, DbTransaction DbTransaction, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        long Count(GenericWhereFilter whereFilter);

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(GenericWhereFilter whereFilter, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        long Count(GenericWhereFilter whereFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter and DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        long Count(GenericWhereFilter whereFilter, DbConnection dbConnection);

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        long Count(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter);

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter and DbTransacrion.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter, DbTransacrion and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter and DbConnection.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection);

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
