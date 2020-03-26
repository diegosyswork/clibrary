using System;
using System.Data;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;
using SysWork.Data.GenericRepository.Mapper;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IGetByLambdaExpressionFilter<TEntity>
    {
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, null, null, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, null, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, null, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, paramDbTransaction, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            TEntity entity = null;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            SetSqlLamAdapter();

            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            string getQuery = string.Format("SELECT {0} FROM {1} {2}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName), query.QueryWhere);

            dbCommand.CommandText = getQuery.ToString();
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                    entity = new MapDataReaderToEntity().MapSingle<TEntity>(reader, ListObjectPropertyInfo);
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
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return entity;
        }
    }
}
