using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.Common.ValueObjects;

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

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, dbConnection, null, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, dbConnection, null, commandTimeOut);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction dbTransaction)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, null, dbTransaction, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetDataTableByLambdaExpressionFilter(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        public DataTable GetDataTableByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
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
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = (DbTransaction)dbTransaction;

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
