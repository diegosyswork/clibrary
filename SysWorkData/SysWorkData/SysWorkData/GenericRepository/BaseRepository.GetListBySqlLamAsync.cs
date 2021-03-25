using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;
using System.Data.Common;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public async Task<IList<TEntity>> GetListBySqlLamAsync(SqlLamBase sqlLam)
        {
            return await GetListBySqlLamAsync(sqlLam, null, null, null);
        }

        public async Task<IList<TEntity>> GetListBySqlLamAsync(SqlLamBase sqlLam, int commandTimeOut)
        {
            return await GetListBySqlLamAsync(sqlLam, null, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListBySqlLamAsync(SqlLamBase sqlLam, DbConnection dbConnection)
        {
            return await GetListBySqlLamAsync(sqlLam, dbConnection, null, null);
        }

        public async Task<IList<TEntity>> GetListBySqlLamAsync(SqlLamBase sqlLam, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetListBySqlLamAsync(sqlLam, dbConnection, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListBySqlLamAsync(SqlLamBase sqlLam, DbTransaction dbTransaction)
        {
            return await GetListBySqlLamAsync(sqlLam, null, dbTransaction, null);
        }

        public async Task<IList<TEntity>> GetListBySqlLamAsync(SqlLamBase sqlLam, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetListBySqlLamAsync(sqlLam, null, dbTransaction, commandTimeOut);
        }

        public async Task<IList<TEntity>> GetListBySqlLamAsync(SqlLamBase sqlLam, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetListBySqlLamAsync(sqlLam, dbConnection, dbTransaction, null);
        }

        public async Task<IList<TEntity>> GetListBySqlLamAsync(SqlLamBase sqlLam, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();
            SetSqlLamAdapter();

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

                dbCommand.CommandText = sqlLam.QueryString;
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                foreach (var parameters in sqlLam.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = await dbCommand.ExecuteReaderAsync();
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
