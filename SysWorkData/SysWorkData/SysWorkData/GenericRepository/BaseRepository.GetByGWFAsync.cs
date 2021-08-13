using System;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;
using System.Data.Common;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public async Task<TEntity> GetByGenericWhereFilterAsync(GenericWhereFilter whereFilter)
        {
            return await GetByGenericWhereFilterAsync(whereFilter, null, null, null);
        }

        public async Task<TEntity> GetByGenericWhereFilterAsync(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return await GetByGenericWhereFilterAsync(whereFilter, null, null, commandTimeOut);
        }

        public async Task<TEntity> GetByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return await GetByGenericWhereFilterAsync(whereFilter, dbConnection, null, null);
        }

        public async Task<TEntity> GetByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetByGenericWhereFilterAsync(whereFilter, dbConnection, null, commandTimeOut);
        }

        public async Task<TEntity> GetByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return await GetByGenericWhereFilterAsync(whereFilter, null, dbTransaction, null);
        }

        public async Task<TEntity> GetByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetByGenericWhereFilterAsync(whereFilter, null, dbTransaction, commandTimeOut);
        }

        public async Task<TEntity> GetByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetByGenericWhereFilterAsync(whereFilter, dbConnection, dbTransaction, null);
        }

        public async Task<TEntity> GetByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity entity = new TEntity();

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

                dbCommand.CommandText = whereFilter.SelectQueryString;
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

                IDataReader reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.SingleRow);
                if (reader.Read())
                    entity = await _mapper.MapSingleAsync<TEntity>(reader, _entityProperties);
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
        }
    }
}
