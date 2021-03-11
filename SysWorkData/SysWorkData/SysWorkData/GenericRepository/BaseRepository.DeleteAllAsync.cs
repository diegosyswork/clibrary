using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity>
    {
        /// <summary>
        /// Deletes all Records.
        /// </summary>
        /// <returns></returns>
        public async Task<long> DeleteAllAsync()
        {
            return await DeleteAllAsync(null, null, null);
        }

        /// <summary>
        /// Deletes all records using a custom commandTimeOut.
        /// </summary>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<long> DeleteAllAsync(int commandTimeOut)
        {
            return await DeleteAllAsync(null, null, commandTimeOut);
        }
        /// <summary>
        /// Deletes all records using an DbConnection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public async Task<long> DeleteAllAsync(DbConnection dbConnection)
        {
            return await DeleteAllAsync(dbConnection, null, null);
        }
        public async Task<long> DeleteAllAsync(DbConnection dbConnection, int commandTimeOut)
        {
            return await DeleteAllAsync(dbConnection, null, commandTimeOut);
        }

        public async Task<long> DeleteAllAsync(DbTransaction dbTransaction)
        {
            return await DeleteAllAsync(null, dbTransaction, null);
        }

        public async Task<long> DeleteAllAsync(DbTransaction dbTransaction, int commandTimeOut)
        {
            return await DeleteAllAsync(null,dbTransaction, commandTimeOut);
        }

        public async Task<long> DeleteAllAsync(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await DeleteAllAsync(dbConnection, dbTransaction, null);
        }

        public async Task<long> DeleteAllAsync(DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandText = string.Format("DELETE FROM {0}", _syntaxProvider.GetSecureTableName(TableName));
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;


                recordsAffected = await dbCommand.ExecuteNonQueryAsync();
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

            return recordsAffected;
        }
    }
}
