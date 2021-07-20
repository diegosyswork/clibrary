using System;
using System.Data;
using System.Linq.Expressions;
using System.Linq;
using System.Data.Common;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter)
        {
            return GetByLambdaExpressionFilter(filter, null, null, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(filter, null, null, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection)
        {
            return GetByLambdaExpressionFilter(filter, dbConnection, null, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(filter, dbConnection, null, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction)
        {
            return GetByLambdaExpressionFilter(filter, null, dbTransaction, null);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetByLambdaExpressionFilter(filter, null, dbTransaction, commandTimeOut);
        }

        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetByLambdaExpressionFilter(filter, dbConnection, dbTransaction, null);
        }


        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity result = null;
            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

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
                result = table.Where(filter).FirstOrDefault();
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
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> filter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity entity = null;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            SetSqlLamAdapter();

            var query = new SqlLam<TEntity>(filter);

            string getQuery = string.Format("SELECT {0} FROM {1} {2}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName), query.QueryWhere);

            dbCommand.CommandText = getQuery.ToString();
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

                IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                    entity = _mapper.MapSingle<TEntity>(reader, EntityProperties);
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
        }*/
    }
}