using System;
using System.Collections.Generic;
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
        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter)
        {
            return await GetListByGenericWhereFilterAsync(whereFilter, null, null, null);
        }

        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return await GetListByGenericWhereFilterAsync(whereFilter, null, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return await GetListByGenericWhereFilterAsync(whereFilter, dbConnection, null, null);
        }

        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetListByGenericWhereFilterAsync(whereFilter, dbConnection, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return await GetListByGenericWhereFilterAsync(whereFilter, null, dbTransaction, null);
        }

        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetListByGenericWhereFilterAsync(whereFilter, null, dbTransaction, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetListByGenericWhereFilterAsync(whereFilter, dbConnection, dbTransaction, null);
        }
        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int commantTimeOut)
        {
            return await GetListByGenericWhereFilterAsync(whereFilter, dbConnection, dbTransaction, commantTimeOut);
        }

        public async Task<IList<TEntity>> GetListByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();

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

                var reader = await dbCommand.ExecuteReaderAsync();
                result = await _mapper.MapAsync<TEntity>(reader, EntityProperties, _databaseEngine);

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
