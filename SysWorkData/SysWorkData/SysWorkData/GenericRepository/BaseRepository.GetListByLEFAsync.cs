using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using SysWork.Data.GenericRepository.Exceptions;
using System.Threading.Tasks;
using System.Data.Common;
using System.Linq;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetListByLambdaExpressionFilterAsync(filter, null, null, null);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return await GetListByLambdaExpressionFilterAsync(filter, null, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection)
        {
            return await GetListByLambdaExpressionFilterAsync(filter, dbConnection, null, null);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetListByLambdaExpressionFilterAsync(filter, dbConnection, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbTransaction dbTransaction)
        {
            return await GetListByLambdaExpressionFilterAsync(filter, null, dbTransaction, null);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetListByLambdaExpressionFilterAsync(filter, null, dbTransaction, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetListByLambdaExpressionFilterAsync(filter, dbConnection, dbTransaction, null);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();
            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

                DbEntityProvider entityProvider = _dbObjectProvider.GetQueryProvider((DbConnection)dbConnectionInUse);
                if (dbTransaction != null)
                {
                    entityProvider.Transaction = (DbTransaction)dbTransaction;
                    entityProvider.Isolation = dbTransaction.IsolationLevel;
                }

                var table = entityProvider.GetTable<TEntity>();
                result = table.Where(filter).ToList();

            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception);
            }
            finally
            {
                if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnectionInUse.Close();
                    dbConnectionInUse.Dispose();
                }
            }
            return result;
        }
    }
}
