using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(filter, null, null, null);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(filter, null, null, commandTimeOut);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(filter, dbConnection, null, null);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(filter, dbConnection, null, commandTimeOut);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbTransaction dbTransaction)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(filter, null, dbTransaction, null);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(filter, null, dbTransaction, commandTimeOut);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(filter, dbConnection, dbTransaction, null);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            DataTable result = null;

            try
            {
                result = ConvertToDatatable(await GetListByLambdaExpressionFilterAsync(filter, dbConnection, dbTransaction, commandTimeOut));

            }
            catch (Exception e)
            {
                throw new RepositoryException(e);
            }

            return result;
        }
    }
}
