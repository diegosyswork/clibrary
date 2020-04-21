using System;
using System.Data.Common;
using System.Linq.Expressions;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.Common.Interfaces.Actions
{
    public interface IExists<TEntity>
    {
        bool Exists(GenericWhereFilter whereFilter);
        bool Exists(GenericWhereFilter whereFilter, int commandTimeOut);
        bool Exists(GenericWhereFilter whereFilter, DbTransaction dbTransaction);
        bool Exists(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut);
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection);
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut);
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction);
        bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);

        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter);
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut);
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction);
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut);
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection);
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut);
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction);
        bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut);
    }
}
