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
        public async Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter)
        {
            return await DeleteByGenericWhereFilterAsync(whereFilter, null, null, null);
        }
        public async Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return await DeleteByGenericWhereFilterAsync(whereFilter, null, null, commandTimeOut);
        }


        public async Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection)
        {
            return await DeleteByGenericWhereFilterAsync(whereFilter, dbConnection, null, null);
        }

        public async Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, int commandTimeOut)
        {
            return await DeleteByGenericWhereFilterAsync(whereFilter, dbConnection, null,  commandTimeOut);
        }


        public async Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction)
        {
            return await DeleteByGenericWhereFilterAsync(whereFilter, null, dbTransaction, null);
        }

        public async Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await DeleteByGenericWhereFilterAsync(whereFilter, null, dbTransaction, commandTimeOut);
        }


        public async Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await DeleteByGenericWhereFilterAsync(whereFilter, dbConnection, dbTransaction, null);
        }

        public async Task<bool> DeleteByGenericWhereFilterAsync(GenericWhereFilter whereFilter, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            TEntity entity = new TEntity();

            if (!string.IsNullOrEmpty(whereFilter.Where))
            {
                string deleteQuery = whereFilter.DeleteQueryString;
                try
                {
                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    dbCommand.CommandText = deleteQuery.ToString();
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

                    if (dbConnectionInUse.State != ConnectionState.Open)
                        await dbConnectionInUse.OpenAsync();

                    if (_databaseEngine == EDatabaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    await dbCommand.ExecuteNonQueryAsync();
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
            return true;
        }

    }
}
