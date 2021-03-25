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
        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return await GetByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, null, null);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return await GetByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return await GetByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, null, null);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, null, commandTimeOut);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return await GetByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, dbTransaction, null);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        public async Task<TEntity> GetByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity entity = null;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            SetSqlLamAdapter();

            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            string getQuery = string.Format("SELECT {0} FROM {1} {2}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName), query.QueryWhere);

            dbCommand.CommandText = getQuery.ToString();
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

                IDataReader reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.SingleRow);
                if (reader.Read())
                    entity = await _mapper.MapSingleAsync<TEntity>(reader, EntityProperties);
                else
                    entity = default(TEntity);

                reader.Close();
                reader.Dispose();

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
            return entity;
        }
    }
}