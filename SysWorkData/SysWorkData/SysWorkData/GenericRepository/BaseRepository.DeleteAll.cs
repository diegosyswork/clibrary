using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;

namespace SysWork.Data.GenericRepository
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
        public long DeleteAll(IDbConnection paramDbConnection)
        {
            return DeleteAll(paramDbConnection, null, null);
        }
        public long DeleteAll(IDbConnection paramDbConnection, int commandTimeOut)
        {
            return DeleteAll(paramDbConnection, null, commandTimeOut);
        }

        public long DeleteAll(IDbTransaction paramDbTransaction)
        {
            return DeleteAll(null, paramDbTransaction, null);
        }

        public long DeleteAll(IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return DeleteAll(null,paramDbTransaction, commandTimeOut);
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

        public long DeleteAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return DeleteAll(paramDbConnection, paramDbTransaction, null);
        }

        public long DeleteAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            long recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = string.Format("DELETE FROM {0}", _syntaxProvider.GetSecureTableName(TableName));
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;


                recordsAffected = dbCommand.ExecuteNonQuery();
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
            return recordsAffected;
        }

    }
}
