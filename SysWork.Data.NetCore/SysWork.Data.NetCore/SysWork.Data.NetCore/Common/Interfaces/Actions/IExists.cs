using System;
using System.Data.Common;
using System.Linq.Expressions;
using SysWork.Data.NetCore.Common.Filters;

namespace SysWork.Data.NetCore.Common.Interfaces.Actions
{
    /// <summary>
    /// Checks if a record exists by a GenericWhereFilter
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IExists<TEntity>
    {
        /// <summary>
        /// Checks if a record exists that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter);

        /// <summary>
        /// Checks if a record exists that match with the GenericWhereFilter and custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists that match with the GenericWhereFilter and DbTransacion.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Checks if a record exists that match with the GenericWhereFilter, DbTransacion and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists that match with the GenericWhereFilter and DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection);

        /// <summary>
        /// Checks if a record exists that match with the GenericWhereFilter, DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists that match with the GenericWhereFilter, DbConnection and DbTransacion.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Checks if a record exists that match with the GenericWhereFilter, DbConnection, DbTransacion and a custom dbCommand timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);

        /// <summary>
        /// Checks if a record exists that match with the LambdaExpressionFilter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter);

        /// <summary>
        /// Checks if a record exists that match with the LambdaExpressionFilter and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists that match with the LambdaExpressionFilter and DbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Checks if a record exists that match with the LambdaExpressionFilter, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists that match with the LambdaExpressionFilter and DbConnection.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection);

        /// <summary>
        /// Checks if a record exists that match with the LambdaExpressionFilter, DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Checks if a record exists that match with the LambdaExpressionFilter, DbConnection and DbTransacion.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Checks if a record exists that match with the LambdaExpressionFilter, DbConnection, DbTransacion and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
