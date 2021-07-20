using System;
using System.Data;
using System.Linq.Expressions;
using SysWork.Data.GenericRepository.Exceptions;
using System.Threading.Tasks;
using System.Data.Common;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await DeleteByLambdaExpressionFilterAsync(filter,null,null,null);
        }
        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return await DeleteByLambdaExpressionFilterAsync(filter, null, null, commandTimeOut);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection)
        {
            return await DeleteByLambdaExpressionFilterAsync(filter, dbConnection,null , null);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, int commandTimeOut)
        {
            return await DeleteByLambdaExpressionFilterAsync(filter, dbConnection, null, commandTimeOut);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbTransaction dbTransaction)
        {
            return await DeleteByLambdaExpressionFilterAsync(filter, null, dbTransaction, null);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await DeleteByLambdaExpressionFilterAsync(filter, null, dbTransaction, commandTimeOut);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await DeleteByLambdaExpressionFilterAsync(filter, dbConnection, dbTransaction, null);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> filter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long recordsAffected = 0;
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
                recordsAffected = table.Delete<TEntity>(filter);

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
            return recordsAffected;
        }
    }
}
