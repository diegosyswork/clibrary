using System;
using System.Data;
using System.Linq.Expressions;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    /// <summary>
    /// Gets a record that match with the LambdaExpressionFilter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetByLambdaExpressionFilter<TEntity>
    {
        /// <summary>
        /// Gets a record that match with the LambdaExpressionFilter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter);

        /// <summary>
        /// Gets a record that match with the LambdaExpressionFilter using a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut);

        /// <summary>
        /// Gets a record that match with the LambdaExpressionFilter using an DbConnection.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection);

        /// <summary>
        /// Gets a record that match with the LambdaExpressionFilter using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Gets a record that match with the LambdaExpressionFilter using an DbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets a record that match with the LambdaExpressionFilter using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Gets a record that match with the LambdaExpressionFilter using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets a record that match with the LambdaExpressionFilter using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);
    }
}
