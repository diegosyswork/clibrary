using System;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Deletes all records that match with the LambdaExpressionFilter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDeleteByLambdaExpressionFilterAsync<TEntity>
    {
        /// <summary>
        /// Deletes all records that match with the LambdaExpressionFilter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter .</param>
        /// <returns></returns>
        Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter);

        /// <summary>
        /// Deletes all records that match with the LambdaExpressionFilter, using a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter .</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the LambdaExpressionFilter using an DnConnection.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter .</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection);

        /// <summary>
        /// Deletes all records that match with the LambdaExpressionFilter, using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter .</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the LambdaExpressionFilter, using an dbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter .</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction);

        /// <summary>
        /// Deletes all records that match with the LambdaExpressionFilter, using an dbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter .</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Deletes all records that match with the LambdaExpressionFilter, using an dbConnection and dbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter .</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction);

        /// <summary>
        /// Deletes all records that match with the LambdaExpressionFilter, using an dbConnection, dbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter .</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
