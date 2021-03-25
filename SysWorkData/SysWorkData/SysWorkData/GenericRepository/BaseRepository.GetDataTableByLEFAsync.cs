using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, null, null);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, null, null);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, null, commandTimeOut);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, dbTransaction, null);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetDataTableByLambdaExpressionFilterAsync(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        public async Task<DataTable> GetDataTableByLambdaExpressionFilterAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            DataTable result = new DataTable(TableName);
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;
            
            DbConnection dbConnectionInUse = (DbConnection)dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

                if (dbTransaction != null)
                    dbCommand.Transaction = (DbTransaction)dbTransaction;

                dbCommand.CommandText = query.QueryString;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();


                DbDataAdapter dbDataAdapter = _dbObjectProvider.GetDbDataAdapter();
                dbDataAdapter.SelectCommand = dbCommand;
                await Task.Run(() => dbDataAdapter.Fill(result));

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
