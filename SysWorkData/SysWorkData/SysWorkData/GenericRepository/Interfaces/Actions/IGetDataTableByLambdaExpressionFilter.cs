using System;
using System.Data;
using System.Linq.Expressions;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IGetDataTableByLambdaExpressionFilter<TEntity>
    {
        DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter);
        DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut);
        DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection);
        DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, int commandTimeOut);
        DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction);
        DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction, int commandTimeOut);
        DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut);

    }
}
