using System;
using System.Data;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity>
    {
        /// <summary>
        /// Deletes all Records.
        /// </summary>
        /// <returns></returns>
        public long DeleteAll()
        {
            return DeleteAll(null, null, null);
        }

        /// <summary>
        /// Deletes all records using a custom commandTimeOut.
        /// </summary>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public long DeleteAll(int commandTimeOut)
        {
            return DeleteAll(null, null, commandTimeOut);
        }
        /// <summary>
        /// Deletes all records using an IDbConnection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
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
