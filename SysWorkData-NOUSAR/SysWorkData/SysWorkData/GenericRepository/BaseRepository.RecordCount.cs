using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    /// <summary>
    /// Counts records in the Table.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract partial class BaseRepository<TEntity> : IRecordCount<TEntity>
    {
        /// <summary>
        /// Counts all records in the Table.
        /// </summary>
        /// <returns></returns>
        public long RecordCount()
        {
            return RecordCount(dbConnection: null, dbTransaction: null, commandTimeOut: null);
        }

        /// <summary>
        /// Counts all records in the Table using a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        public long RecordCount(int commandTimeOut)
        {
            return RecordCount(dbConnection:null, dbTransaction:null, commandTimeOut:commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection.
        /// </summary>
        /// <returns></returns>
        public long RecordCount(DbConnection dbConnection)
        {
            return RecordCount(dbConnection, null, null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        public long RecordCount(DbConnection dbConnection, int commandTimeOut)
        {
            return RecordCount(dbConnection, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long RecordCount(DbTransaction dbTransaction)
        {
            return RecordCount(dbConnection: null, dbTransaction: dbTransaction, commandTimeOut: null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount( DbTransaction dbTransaction, int commandTimeOut)
        {
            return RecordCount(dbConnection: null, dbTransaction: dbTransaction, commandTimeOut: commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long RecordCount(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return RecordCount(dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
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
        /// Counts all records in the Table that match with the GenericWhereFilter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        public long RecordCount(GenericWhereFilter whereFilter)
        {
            return RecordCount(whereFilter, null, null, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return RecordCount(whereFilter, null, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long RecordCount(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return RecordCount(whereFilter, null, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return RecordCount(whereFilter, null, dbTransaction, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public long RecordCount(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return RecordCount(whereFilter, dbConnection, null, null);
        }


        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return RecordCount(whereFilter, dbConnection, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long RecordCount(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return RecordCount(whereFilter, dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
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
        /// Counts all records in the Table that match with the LambdaExpressionFilter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        public long RecordCount(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return RecordCount(lambdaExpressionFilter, null, null, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return RecordCount(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter and DbTransacrion.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long RecordCount(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return RecordCount(lambdaExpressionFilter, null, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter, DbTransacrion and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return RecordCount(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter and DbConnection.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public long RecordCount(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return RecordCount(lambdaExpressionFilter, dbConnection, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return RecordCount(lambdaExpressionFilter, dbConnection, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public long RecordCount(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return RecordCount(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long RecordCount(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
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

                foreach (var parameter in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameter.Key, parameter.Value));

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
