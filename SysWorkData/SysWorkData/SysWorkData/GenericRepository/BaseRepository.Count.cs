using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : ICount<TEntity>
    {
        public long Count()
        {
            throw new NotImplementedException();
        }

        public long Count(int commandTimeOut)
        {
            throw new NotImplementedException();
        }

        public long Count(DbConnection dbConnection)
        {
            throw new NotImplementedException();
        }

        public long Count(DbConnection dbConnection, int commandTimeOut)
        {
            throw new NotImplementedException();
        }

        public long Count(DbTransaction DbTransaction)
        {
            throw new NotImplementedException();
        }

        public long Count(DbTransaction DbTransaction, int commandTimeOut)
        {
            throw new NotImplementedException();
        }

        public long Count(DbConnection dbConnection, DbTransaction DbTransaction)
        {
            throw new NotImplementedException();
        }

        public long Count(DbConnection dbConnection, DbTransaction DbTransaction, int commandTimeOut)
        {
            throw new NotImplementedException();
        }

        public long Count(GenericWhereFilter whereFilter)
        {
            throw new NotImplementedException();
        }

        public long Count(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            throw new NotImplementedException();
        }

        public long Count(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public long Count(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            throw new NotImplementedException();
        }

        public long Count(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return Count(whereFilter, dbConnection, null, null);
        }

        public long Count(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return Count(whereFilter, dbConnection, null, commandTimeOut);
        } 

        public long Count(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return Count(whereFilter, dbConnection, dbTransaction, null);
        }

        public long Count(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long result = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            if (!string.IsNullOrEmpty(whereFilter.Where))
            {
                string selectCountQuery = string.Format("{0} WHERE {1}", _syntaxProvider.GetQuerySelectCOUNT(TableName), whereFilter.Where);
                try
                {
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        dbConnectionInUse.Open();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    dbCommand.CommandText = selectCountQuery;
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    foreach (var param in whereFilter.Parameters)
                    {
                        var dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = param.Key;
                        dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                        if (whereFilter.ParametersSize.TryGetValue(dbParameter.ParameterName, out int paramSize))
                            if (paramSize != 0)
                                dbParameter.Size = paramSize;

                        if (whereFilter.ParametersDbTye.TryGetValue(dbParameter.ParameterName, out DbType dbType))
                            dbParameter.DbType = dbType;

                        dbCommand.Parameters.Add(dbParameter);
                    }

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    result = DbUtil.ParseToLong(dbCommand.ExecuteScalar());

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
            }
            else
            {
                throw new ArgumentNullException("Where clause is not set");
            }
            return result;
        }

        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return Count(lambdaExpressionFilter, null, null, null);
        }

        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return Count(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return Count(lambdaExpressionFilter, null, dbTransaction, null);
        }

        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return Count(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return Count(lambdaExpressionFilter, dbConnection, null);
        }

        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return Count(lambdaExpressionFilter, dbConnection, commandTimeOut);
        }

        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return Count(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long result = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            SetSqlLamAdapter();

            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            string countQuery = string.Format("{0} WHERE {1}", _syntaxProvider.GetQuerySelectCOUNT(TableName), query.QueryWhere);

            dbCommand.CommandText = countQuery.ToString();
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

                IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                    result = DbUtil.ParseToLong(dbCommand.ExecuteScalar());

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
