using System;
using System.Data;
using System.Linq.Expressions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.Interfaces.Actions;
using System.Linq;
using System.Data.Common;

namespace SysWork.Data.GenericViewManager
{
    public abstract partial class BaseViewManager<TEntity> : IGetByLambdaExpressionFilter<TEntity>
    {
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter)
        {
            return GetByLambdaExpressionFilter(filter, null, null, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(filter, null, null, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection)
        {
            return GetByLambdaExpressionFilter(filter, dbConnection, null, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(filter, dbConnection, null, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction)
        {
            return GetByLambdaExpressionFilter(filter, null, dbTransaction, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(filter, null, dbTransaction, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetByLambdaExpressionFilter(filter, dbConnection, dbTransaction, null);
        }
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity result = null;
            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                DbEntityProvider entityProvider = DataObjectProvider.GetQueryProvider((DbConnection)dbConnectionInUse);
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
            return result;
        }
    }
}