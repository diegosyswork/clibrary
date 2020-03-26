using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;
using SysWork.Data.GenericRepository.Mapper;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IGetListByLambdaExpressionFilter<TEntity>
    {
        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, null, null, null);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, null, null);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, null, commandTimeOut);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction, null);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction, commandTimeOut);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, paramDbTransaction, null);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                dbCommand.CommandText = query.QueryString;
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader();
                result = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo, _dataBaseEngine);

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
            return result;
        }
    }
}
