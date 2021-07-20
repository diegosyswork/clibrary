using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Threading.Tasks;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity>
    {
        public async Task<bool> ExistsAsync(GenericWhereFilter whereFilter)
        {
            return await ExistsAsync(whereFilter, null, null, null);
        }
        public async Task<bool> ExistsAsync(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return await ExistsAsync(whereFilter, null, null, commandTimeOut);
        }

        public async Task<bool> ExistsAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return await ExistsAsync(whereFilter, null, dbTransaction, null);
        }
        public async Task<bool> ExistsAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await ExistsAsync(whereFilter, null, dbTransaction, commandTimeOut);
        }

        public async Task<bool> ExistsAsync(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return await ExistsAsync(whereFilter, dbConnection, null, null);
        }
        public async Task<bool> ExistsAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await ExistsAsync(whereFilter, dbConnection, null, commandTimeOut);
        }

        public async Task<bool> ExistsAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await ExistsAsync(whereFilter, dbConnection, dbTransaction, null);
        }

        public async Task<bool> ExistsAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            bool result = false;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

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

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                var reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.SingleRow);
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
