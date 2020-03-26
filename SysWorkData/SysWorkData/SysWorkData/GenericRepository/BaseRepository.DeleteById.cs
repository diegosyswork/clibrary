using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Attributes;
using SysWork.Data.GenericRepository.DbInfo;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IDeleteById<TEntity>
    {
        public bool DeleteById(long Id)
        {
            return DeleteById(Id, null, null, out long recordsAffected, null);
        }
        public bool DeleteById(long Id, int commandTimeOut)
        {
            return DeleteById(Id, null, null, out long recordsAffected, commandTimeOut);
        }

        public bool DeleteById(long Id, IDbConnection paramDbConnection)
        {
            return DeleteById(Id, paramDbConnection, null, out long recordsAffected, null);
        }

        public bool DeleteById(long Id, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return DeleteById(Id, paramDbConnection, null, out long recordsAffected, commandTimeOut);
        }

        public bool DeleteById(long Id, IDbTransaction paramDbTransaction)
        {
            return DeleteById(Id, null, paramDbTransaction, out long recordsAffected, null);
        }

        public bool DeleteById(long Id, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return DeleteById(Id, null, paramDbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return DeleteById(Id, paramDbConnection, paramDbTransaction, out long recordsAffected, null);
        }

        public bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return DeleteById(Id, paramDbConnection, paramDbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool DeleteById(long Id, out string errMessage)
        {
            return DeleteById(Id, out errMessage, null);
        }

        public bool DeleteById(long Id, out string errMessage, int commandTimeOut)
        {
            return DeleteById(Id, out errMessage, commandTimeOut);
        }
        private bool DeleteById(long Id, out string errMessage, int? commandTimeOut)
        {
            bool result = false;
            errMessage = "";

            try
            {
                result = DeleteById(Id,null,null,out long recordsAffected, commandTimeOut);
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

        public bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            return DeleteById(Id, paramDbConnection, paramDbTransaction, out recordsAffected, null);
        }

        public bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            TEntity entity = new TEntity();
            StringBuilder where = new StringBuilder();

            string parameterName;
            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                var columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                parameterName = "@param_" + i.Name;

                if (customAttribute.IsPrimary)
                {
                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("({0} = {1})", columnName, parameterName));

                    ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, Id, cdbi.MaxLenght));

                }
            }

            if (where.ToString() != string.Empty)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(string.Format("DELETE FROM {0} WHERE {1}", _syntaxProvider.GetSecureTableName(TableName), where));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = deleteQuery.ToString();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

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
            }
            return true;
        }
    }
}
