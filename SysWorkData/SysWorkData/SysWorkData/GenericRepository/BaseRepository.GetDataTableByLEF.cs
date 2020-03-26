using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IGetDataTableByLambdaExpressionFilter<TEntity>
    {

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, null, null, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, null, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, null, commandTimeOut);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction, commandTimeOut);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, paramDbTransaction, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            DataTable result = new DataTable(TableName);
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            DbConnection dbConnection = (DbConnection)paramDbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = (DbTransaction)paramDbTransaction;

                dbCommand.CommandText = query.QueryString;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();


                DbDataAdapter dbDataAdapter = _dbObjectProvider.GetDbDataAdapter();
                dbDataAdapter.SelectCommand = dbCommand;
                dbDataAdapter.Fill(result);

                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception, dbCommand);
            }
            finally
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return result;
        }
    }
}
