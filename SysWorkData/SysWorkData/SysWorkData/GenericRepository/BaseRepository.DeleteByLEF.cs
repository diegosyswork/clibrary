using System;
using System.Data;
using System.Linq.Expressions;
using System.Data.Common;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter)
        {
            return DeleteByLambdaExpressionFilter(filter,null,null,null);
        }
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return DeleteByLambdaExpressionFilter(filter, null, null, commandTimeOut);
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection)
        {
            return DeleteByLambdaExpressionFilter(filter, dbConnection,null , null);
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, int commandTimeOut)
        {
            return DeleteByLambdaExpressionFilter(filter, dbConnection, null, commandTimeOut);
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction)
        {
            return DeleteByLambdaExpressionFilter(filter, null, dbTransaction, null);
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return DeleteByLambdaExpressionFilter(filter, null, dbTransaction, commandTimeOut);
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return DeleteByLambdaExpressionFilter(filter, dbConnection, dbTransaction, null);
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));
            long result;

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
                result = table.Delete<TEntity>(filter);

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
        /*
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(filter);

            long recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandText = string.Format("DELETE FROM {0} {1}", _syntaxProvider.GetSecureTableName(TableName), query.QueryWhere);
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();


                recordsAffected = dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception, dbCommand);
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
        */


    }
}
