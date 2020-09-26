using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IExists<TEntity>
    {
        public bool Exists(GenericWhereFilter whereFilter)
        {
            return Exists(whereFilter, null, null, null);
        }
        public bool Exists(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return Exists(whereFilter, null, null, commandTimeOut);
        }

        public bool Exists(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return Exists(whereFilter, null, dbTransaction, null);
        }
        public bool Exists(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return Exists(whereFilter, null, dbTransaction, commandTimeOut);
        }

        public bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return Exists(whereFilter, dbConnection, null, null);
        }
        public bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return Exists(whereFilter, dbConnection, null, commandTimeOut);
        }

        public bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return Exists(whereFilter, dbConnection, dbTransaction, null);
        }

        public bool Exists(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            bool result = false;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                string existsQuery = _syntaxProvider.GetQuerySelectTop_1_1(TableName);
                existsQuery += $" WHERE {whereFilter.Where}";

                dbCommand.CommandText = existsQuery;
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

                var reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                result = reader.Read();

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



        public bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return Exists(lambdaExpressionFilter, null, null, null);
        }
        public bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return Exists(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        public bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return Exists(lambdaExpressionFilter, null, dbTransaction, null);
        }
        public bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return Exists(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        public bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return Exists(lambdaExpressionFilter, dbConnection, null, null);
        }
        public bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return Exists(lambdaExpressionFilter, dbConnection, null, commandTimeOut);
        }

        public bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return Exists(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        public bool Exists(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            bool result = false;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                SetSqlLamAdapter();

                var query = new SqlLam<TEntity>(lambdaExpressionFilter);

                string existsQuery = string.Format("{0} {1}",_syntaxProvider.GetQuerySelectTop_1_1(TableName), query.QueryWhere);

                dbCommand.CommandText = existsQuery;
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));


                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                result = reader.Read();

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
