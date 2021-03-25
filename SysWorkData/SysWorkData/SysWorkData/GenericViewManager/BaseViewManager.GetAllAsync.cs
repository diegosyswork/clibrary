using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericViewManager
{
    public abstract partial class BaseViewManager<TEntity> : IGetAllAsync<TEntity>
    {
        /// <summary>
        /// Gets all records of the table.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await GetAllAsync(null, null, null);
        }

        /// <summary>
        /// Gets all records of the table using a custom dbCommand timeout.
        /// </summary>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAllAsync(int commandTimeOut)
        {
            return await GetAllAsync(null, null, commandTimeOut);
        }

        /// <summary>
        /// Gets all records of the table using an DbConnection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAllAsync(DbConnection dbConnection)
        {
            return await GetAllAsync(dbConnection, null, null);
        }

        /// <summary>
        /// Gets all records of the table using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAllAsync(DbConnection dbConnection, int commandTimeOut)
        {
            return await GetAllAsync(dbConnection, null, commandTimeOut);
        }

        /// <summary>
        /// Gets all records of the table using an DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAllAsync(DbTransaction dbTransaction)
        {
            return await GetAllAsync(null, dbTransaction, null);
        }

        /// <summary>
        /// Gets all records of the table using an DbTransaction and a cusmon.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out</param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAllAsync(DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetAllAsync(null, dbTransaction, commandTimeOut);
        }

        /// <summary>
        /// Gets all records of the table using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAllAsync(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetAllAsync(dbConnection, dbTransaction, null);
        }

        /// <summary>
        /// Gets all records of the table using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAllAsync(DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> collection = new List<TEntity>();

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? GetDbConnection();

            DbCommand dbCommand = dbConnectionInUse.CreateCommand();
            dbCommand.CommandText = string.Format("SELECT {0} FROM {1}", ColumnsForSelect, _syntaxProvider.GetSecureViewName(ViewName));
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                DbDataReader reader = await dbCommand.ExecuteReaderAsync();
                collection = await _mapper.MapAsync<TEntity>(reader, EntityProperties, _databaseEngine);

                reader.Close(); reader.Dispose();
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

            return collection;
        }






    }
}
