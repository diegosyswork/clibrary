using System;
using System.Data;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;
using System.Data.Common;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return await DeleteByLambdaExpressionFilterAsync(lambdaExpressionFilter,null,null,null);
        }
        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return await DeleteByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return await DeleteByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection,null , null);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await DeleteByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, null, commandTimeOut);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return await DeleteByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, dbTransaction, null);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await DeleteByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await DeleteByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        public async Task<long> DeleteByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            long recordsAffected = 0;
            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandText = string.Format("DELETE FROM {0} {1}", _syntaxProvider.GetSecureTableName(TableName), query.QueryWhere);
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();


                recordsAffected = await dbCommand.ExecuteNonQueryAsync();
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
