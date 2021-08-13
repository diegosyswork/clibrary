using System;
using System.Collections.Generic;
using System.Data;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public IList<TEntity> GetAll()
        {
            return GetAll(null, null, null);
        }

        public IList<TEntity> GetAll(int commandTimeOut)
        {
            return GetAll(null, null, commandTimeOut);
        }

        public IList<TEntity> GetAll(IDbConnection dbConnection)
        {
            return GetAll(dbConnection, null, null);
        }

        public IList<TEntity> GetAll(IDbConnection dbConnection, int commandTimeOut)
        {
            return GetAll(dbConnection, null, commandTimeOut);
        }

        public IList<TEntity> GetAll(IDbTransaction dbTransaction)
        {
            return GetAll(null, dbTransaction, null);
        }

        public IList<TEntity> GetAll(IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetAll(null, dbTransaction, commandTimeOut);
        }

        public IList<TEntity> GetAll(IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetAll(dbConnection, dbTransaction, null);
        }

        public IList<TEntity> GetAll(IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandText = string.Format("SELECT {0} FROM {1}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName));
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                IDataReader reader = dbCommand.ExecuteReader();
                result = _mapper.Map<TEntity>(reader, _entityProperties, _databaseEngine);

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

            return result;
        }
    }
}
