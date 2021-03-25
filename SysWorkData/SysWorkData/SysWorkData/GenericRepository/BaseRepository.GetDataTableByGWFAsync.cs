using System;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.Filters;
using System.Data.Common;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public async Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter)
        {
            return await GetDataTableByGenericWhereFilterAsync(whereFilter, null, null, null);
        }

        public async Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, int commandTimeout)
        {
            return await GetDataTableByGenericWhereFilterAsync(whereFilter, null, null, commandTimeout);
        }

        public async Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return await GetDataTableByGenericWhereFilterAsync(whereFilter, dbConnection, null, null);
        }

        public async Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeout)
        {
            return await GetDataTableByGenericWhereFilterAsync(whereFilter, dbConnection, null, commandTimeout);
        }

        public async Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return await GetDataTableByGenericWhereFilterAsync(whereFilter, null, dbTransaction, null);
        }

        public async Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeout)
        {
            return await GetDataTableByGenericWhereFilterAsync(whereFilter, null, dbTransaction, commandTimeout);
        }

        public async Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetDataTableByGenericWhereFilterAsync(whereFilter, dbConnection, dbTransaction, null);
        }

        public async Task<DataTable> GetDataTableByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? CommandTimeout)
        {
            DataTable result = new DataTable(TableName);

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandTimeout = CommandTimeout ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

                if (dbTransaction != null)
                    dbCommand.Transaction = (DbTransaction)dbTransaction;

                dbCommand.CommandText = whereFilter.SelectQueryString;

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

                DbDataAdapter dbDataAdapter = _dbObjectProvider.GetDbDataAdapter();
                dbDataAdapter.SelectCommand = dbCommand;
                await Task.Run(() => dbDataAdapter.Fill(result));

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




