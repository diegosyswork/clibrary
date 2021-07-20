using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter)
        {
            return GetDataTableByLambdaExpressionFilter(filter, null, null, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return GetDataTableByLambdaExpressionFilter(filter, null, null, commandTimeOut);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection)
        {
            return GetDataTableByLambdaExpressionFilter(filter, dbConnection, null, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetDataTableByLambdaExpressionFilter(filter, dbConnection, null, commandTimeOut);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction)
        {
            return GetDataTableByLambdaExpressionFilter(filter, null, dbTransaction, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetDataTableByLambdaExpressionFilter(filter, null, dbTransaction, commandTimeOut);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetDataTableByLambdaExpressionFilter(filter, dbConnection, dbTransaction, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            DataTable result = null;

            try
            {
                result = ConvertToDatatable(GetListByLambdaExpressionFilter(filter, dbConnection, dbTransaction, commandTimeOut));

            }
            catch (Exception e)
            {
                throw new RepositoryException(e);
            }
            
            return result;
        }
    }
}
