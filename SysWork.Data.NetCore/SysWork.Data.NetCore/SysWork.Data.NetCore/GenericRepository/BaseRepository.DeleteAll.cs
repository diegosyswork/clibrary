using System;
using System.Data;
using SysWork.Data.NetCore.GenericRepository.Exceptions;
using SysWork.Data.NetCore.Common.Interfaces.Actions;

namespace SysWork.Data.NetCore.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IDeleteAll<TEntity>
    {
        public long DeleteAll()
        {
            return DeleteAll(null, null, null);
        }
        public long DeleteAll(int commandTimeOut)
        {
            return DeleteAll(null, null, commandTimeOut);
        }
        public long DeleteAll(IDbConnection dbConnection)
        {
            return DeleteAll(dbConnection, null, null);
        }
        public long DeleteAll(IDbConnection dbConnection, int commandTimeOut)
        {
            return DeleteAll(dbConnection, null, commandTimeOut);
        }

        public long DeleteAll(IDbTransaction dbTransaction)
        {
            return DeleteAll(null, dbTransaction, null);
        }

        public long DeleteAll(IDbTransaction dbTransaction, int commandTimeOut)
        {
            return DeleteAll(null,dbTransaction, commandTimeOut);
        }

        public bool DeleteAll(out string errMessage)
        {
            return DeleteAll(out errMessage, null);
        }

        public bool DeleteAll(out string errMessage, int commandTimeOut)
        {
            return DeleteAll(out errMessage, commandTimeOut);
        }

        private bool DeleteAll(out string errMessage, int? commandTimeOut)
        {
            bool result = false;
            errMessage = "";

            try
            {
                DeleteAll(null,null,commandTimeOut);
                result = true;
            }
            catch (RepositoryException genericRepositoryException)
            {
                errMessage = genericRepositoryException.OriginalException.Message;
                result = false;
            }
            catch (Exception exception)
            {
                errMessage = exception.Message;
                result = false;
            }

            return result;
        }

        public long DeleteAll(IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return DeleteAll(dbConnection, dbTransaction, null);
        }

        public long DeleteAll(IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            long recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandText = string.Format("DELETE FROM {0}", _syntaxProvider.GetSecureTableName(TableName));
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;


                recordsAffected = dbCommand.ExecuteNonQuery();
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
