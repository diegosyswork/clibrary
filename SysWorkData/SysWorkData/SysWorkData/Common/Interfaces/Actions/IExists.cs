using System;
using System.Data.Common;
using System.Linq.Expressions;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Checks if a record exists by a GenericWhereFilter
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IExists<TEntity>
    {
        /// <summary>
        /// Checks if a record exists using an GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter);

        /// <summary>
        /// Checks if a record exists using an GenericWhereFilter and custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists using an GenericWhereFilter and DbTransacion.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Checks if a record exists using an GenericWhereFilter, DbTransacion and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists using an GenericWhereFilter and DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection);

        /// <summary>
        /// Checks if a record exists using an GenericWhereFilter, DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists using an GenericWhereFilter, DbConnection and DbTransacion.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Checks if a record exists using an GenericWhereFilter, DbConnection, DbTransacion and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);

        /// <summary>
        /// Checks if a record exists using an LambdaExpressionFilter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter);

        /// <summary>
        /// Checks if a record exists using an LambdaExpressionFilter and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists using an LambdaExpressionFilter and DbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Checks if a record exists using an LambdaExpressionFilter, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists using an LambdaExpressionFilter and DbConnection.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection);

        /// <summary>
        /// Checks if a record exists using an LambdaExpressionFilter, DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists using an LambdaExpressionFilter, DbConnection and DbTransacion.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Checks if a record exists using an LambdaExpressionFilter, DbConnection, DbTransacion and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
