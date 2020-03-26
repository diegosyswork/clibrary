using System;
using System.Collections.Generic;
using System.Data;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;
using SysWork.Data.GenericRepository.Mapper;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IGetAll<TEntity>
    {
        public IList<TEntity> GetAll()
        {
            return GetAll(null, null, null);
        }

        public IList<TEntity> GetAll(int commandTimeOut)
        {
            return GetAll(null, null, commandTimeOut);
        }

        public IList<TEntity> GetAll(IDbConnection paramDbConnection)
        {
            return GetAll(paramDbConnection, null, null);
        }

        public IList<TEntity> GetAll(IDbConnection paramDbConnection, int commandTimeOut)
        {
            return GetAll(paramDbConnection, null, commandTimeOut);
        }

        public IList<TEntity> GetAll(IDbTransaction paramDbTransaction)
        {
            return GetAll(null, paramDbTransaction, null);
        }

        public IList<TEntity> GetAll(IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return GetAll(null, paramDbTransaction, commandTimeOut);
        }

        public IList<TEntity> GetAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return GetAll(paramDbConnection, paramDbTransaction, null);
        }

        public IList<TEntity> GetAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            IList<TEntity> collection = new List<TEntity>();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = string.Format("SELECT {0} FROM {1}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName));
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                IDataReader reader = dbCommand.ExecuteReader();
                collection = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo, _dataBaseEngine);

                reader.Close(); reader.Dispose();
                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception, dbCommand);
            }
            finally
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return collection;

        }
    }
}
