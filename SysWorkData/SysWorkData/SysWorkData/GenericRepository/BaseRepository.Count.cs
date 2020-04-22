using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    /// <summary>
    /// Counts records in the Table.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract partial class BaseRepository<TEntity> : ICount<TEntity>
    {
        /// <summary>
        /// Counts all records in the Table.
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            return Count(dbConnection: null, dbTransaction: null, commandTimeOut: null);
        }

        /// <summary>
        /// Counts all records in the Table using a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        public long Count(int commandTimeOut)
        {
            return Count(dbConnection:null, dbTransaction:null, commandTimeOut:commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection.
        /// </summary>
        /// <returns></returns>
        public long Count(DbConnection dbConnection)
        {
            return Count(dbConnection, null, null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        public long Count(DbConnection dbConnection, int commandTimeOut)
        {
            return Count(dbConnection, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long Count(DbTransaction dbTransaction)
        {
            return Count(dbConnection: null, dbTransaction: dbTransaction, commandTimeOut: null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long Count( DbTransaction dbTransaction, int commandTimeOut)
        {
            return Count(dbConnection: null, dbTransaction: dbTransaction, commandTimeOut: commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long Count(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return Count(dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long Count(DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long result = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            string selectCountQuery = _syntaxProvider.GetQuerySelectCOUNT(TableName);
            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                dbCommand.CommandText = selectCountQuery;
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

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
            return result;
        }

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        public long Count(GenericWhereFilter whereFilter)
        {
            return Count(whereFilter, null, null, null);
        }

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long Count(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return Count(whereFilter, null, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long Count(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return Count(whereFilter, null, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long Count(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return Count(whereFilter, null, dbTransaction, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter and DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public long Count(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return Count(whereFilter, dbConnection, null, null);
        }


        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long Count(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return Count(whereFilter, dbConnection, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long Count(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return Count(whereFilter, dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table using an GenericWhereFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return Count(lambdaExpressionFilter, null, null, null);
        }

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return Count(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter and DbTransacrion.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return Count(lambdaExpressionFilter, null, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter, DbTransacrion and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return Count(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter and DbConnection.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return Count(lambdaExpressionFilter, dbConnection, null);
        }

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return Count(lambdaExpressionFilter, dbConnection, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return Count(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table using an LambdaExpressionFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
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
