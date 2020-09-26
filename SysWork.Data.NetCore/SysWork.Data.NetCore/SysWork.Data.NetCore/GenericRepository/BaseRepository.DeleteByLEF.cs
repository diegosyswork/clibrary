using System;
using System.Data;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.NetCore.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.NetCore.Common.LambdaSqlBuilder;
using SysWork.Data.NetCore.GenericRepository.Exceptions;
using SysWork.Data.NetCore.Common.Interfaces.Actions;
using SysWork.Data.NetCore.Common.ValueObjects;

namespace SysWork.Data.NetCore.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IDeleteByLambdaExpressionFilter<TEntity>
    {
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return 0;
        }
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return 0;
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection)
        {
            return 0;
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, int commandTimeOut)
        {
            return 0;
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction dbTransaction)
        {
            return 0;
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return 0;
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return 0;
        }

        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

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

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
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



    }
}
