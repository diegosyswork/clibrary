using System;
using System.Collections.Generic;
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
        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return await GetListByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, null, null);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return await GetListByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return await GetListByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, null, null);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetListByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return await GetListByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, dbTransaction, null);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetListByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetListByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        public async Task<IList<TEntity>> GetListByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                dbCommand.CommandText = query.QueryString;
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = await dbCommand.ExecuteReaderAsync();
                result = await _mapper.MapAsync<TEntity>(reader, EntityProperties, _databaseEngine);

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
            return result;
        }
    }
}
