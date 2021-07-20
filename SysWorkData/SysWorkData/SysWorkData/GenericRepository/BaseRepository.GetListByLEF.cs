using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using System.Linq;
using System.Data.Common;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter)
        {
            return GetListByLambdaExpressionFilter(filter, null, null, null);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return GetListByLambdaExpressionFilter(filter, null, null, commandTimeOut);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection)
        {
            return GetListByLambdaExpressionFilter(filter, dbConnection, null, null);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetListByLambdaExpressionFilter(filter, dbConnection, null, commandTimeOut);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction)
        {
            return GetListByLambdaExpressionFilter(filter, null, dbTransaction, null);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetListByLambdaExpressionFilter(filter, null, dbTransaction, commandTimeOut);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetListByLambdaExpressionFilter(filter, dbConnection, dbTransaction, null);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();
            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

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
