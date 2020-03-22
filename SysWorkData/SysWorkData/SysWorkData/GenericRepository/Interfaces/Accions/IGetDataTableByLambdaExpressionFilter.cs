﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Accions
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