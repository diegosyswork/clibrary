using System;
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
        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetByLambdaExpressionFilterAsync(filter, null, null, null);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return await GetByLambdaExpressionFilterAsync(filter, null, null, commandTimeOut);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection)
        {
            return await GetByLambdaExpressionFilterAsync(filter, dbConnection, null, null);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetByLambdaExpressionFilterAsync(filter, dbConnection, null, commandTimeOut);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbTransaction dbTransaction)
        {
            return await GetByLambdaExpressionFilterAsync(filter, null, dbTransaction, null);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetByLambdaExpressionFilterAsync(filter, null, dbTransaction, commandTimeOut);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetByLambdaExpressionFilterAsync(filter, dbConnection, dbTransaction, null);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity result = null;
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
                result = table.Where(filter).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new RepositoryException(e);
            }
            finally 
            {
                if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnectionInUse.Close();
                    dbConnectionInUse.Dispose();
                }
            }
            return result ;
        }
    }
}