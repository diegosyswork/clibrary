using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
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
    public abstract partial class BaseRepository<TEntity>
    { 
        /// <summary>
        /// Counts all records in the Table.
        /// </summary>
        /// <returns></returns>
        public async Task<long> RecordCountAsync()
        {
            return await RecordCountAsync(dbConnection: null, dbTransaction: null, commandTimeOut: null);
        }

        /// <summary>
        /// Counts all records in the Table using a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(int commandTimeOut)
        {
            return await RecordCountAsync(dbConnection:null, dbTransaction:null, commandTimeOut:commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection.
        /// </summary>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(DbConnection dbConnection)
        {
            return await RecordCountAsync(dbConnection, null, null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(DbConnection dbConnection, int commandTimeOut)
        {
            return await RecordCountAsync(dbConnection, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(DbTransaction dbTransaction)
        {
            return await RecordCountAsync(dbConnection: null, dbTransaction: dbTransaction, commandTimeOut: null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync( DbTransaction dbTransaction, int commandTimeOut)
        {
            return await RecordCountAsync(dbConnection: null, dbTransaction: dbTransaction, commandTimeOut: commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await RecordCountAsync(dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table using an DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long result = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            string selectCountQuery = _syntaxProvider.GetQuerySelectCOUNT(TableName);
            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                dbCommand.CommandText = selectCountQuery;
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                result = DbUtil.ParseToLong(await dbCommand.ExecuteScalarAsync());

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
        public async Task<long> RecordCountAsync(GenericWhereFilter whereFilter)
        {
            return await RecordCountAsync(whereFilter, null, null, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return await RecordCountAsync(whereFilter, null, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return await RecordCountAsync(whereFilter, null, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await RecordCountAsync(whereFilter, null, dbTransaction, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter and DbConnection.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return await RecordCountAsync(whereFilter, dbConnection, null, null);
        }


        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await RecordCountAsync(whereFilter, dbConnection, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await RecordCountAsync(whereFilter, dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the GenericWhereFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long result = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            if (!string.IsNullOrEmpty(whereFilter.Where))
            {
                string selectCountQuery = string.Format("{0} WHERE {1}", _syntaxProvider.GetQuerySelectCOUNT(TableName), whereFilter.Where);
                try
                {
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        await dbConnectionInUse.OpenAsync();

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

                    if (_databaseEngine == EDatabaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    result = DbUtil.ParseToLong(await dbCommand.ExecuteScalarAsync());

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
        public async Task<long> RecordCountAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return await RecordCountAsync(lambdaExpressionFilter, null, null, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, int commandTimeOut)
        {
            return await RecordCountAsync(lambdaExpressionFilter, null, null, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter and DbTransacrion.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction)
        {
            return await RecordCountAsync(lambdaExpressionFilter, null, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter, DbTransacrion and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await RecordCountAsync(lambdaExpressionFilter, null, dbTransaction, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter and DbConnection.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection)
        {
            return await RecordCountAsync(lambdaExpressionFilter, dbConnection, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter, DbConnection and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await RecordCountAsync(lambdaExpressionFilter, dbConnection, commandTimeOut);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter, DbConnection and DbTransaction.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await RecordCountAsync(lambdaExpressionFilter, dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Counts all records in the Table that match with the LambdaExpressionFilter, DbConnection, DbTransaction and a custom dbCommnad timeout.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> RecordCountAsync(Expression<Func<TEntity, bool>> lambdaExpressionFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long result = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            SetSqlLamAdapter();

            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            string countQuery = string.Format("{0} WHERE {1}", _syntaxProvider.GetQuerySelectCOUNT(TableName), query.QueryWhere);

            dbCommand.CommandText = countQuery.ToString();
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                foreach (var parameter in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameter.Key, parameter.Value));

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                    result = DbUtil.ParseToLong(await dbCommand.ExecuteScalarAsync());

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
