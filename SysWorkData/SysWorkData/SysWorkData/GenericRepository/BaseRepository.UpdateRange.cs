using System;
using System.Collections.Generic;
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
    public abstract partial class BaseRepository<TEntity> : IUpdateRange<TEntity>
    {
        public bool UpdateRange(IList<TEntity> entities)
        {
            return UpdateRange(entities, null, null, out long recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, int commandTimeOut)
        {
            return UpdateRange(entities, null, null, out long recordsAffected, commandTimeOut);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection)
        {
            return UpdateRange(entities, paramDbConnection, null, out long recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return UpdateRange(entities, paramDbConnection, null, out long recordsAffected, commandTimeOut);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbTransaction paramDbTransaction)
        {
            return UpdateRange(entities, null, paramDbTransaction, out long recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return UpdateRange(entities, null, paramDbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return UpdateRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return UpdateRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool UpdateRange(IList<TEntity> entities, out string errMessage)
        {
            return UpdateRange(entities, out errMessage, null);
        }

        public bool UpdateRange(IList<TEntity> entities, out string errMessage, int commandTimeOut)
        {
            return UpdateRange(entities, out errMessage, commandTimeOut);
        }

        private bool UpdateRange(IList<TEntity> entities, out string errMessage, int? commandTimeOut)
        {
            bool result = false;
            errMessage = "";

            try
            {
                result = UpdateRange(entities,null,null,out long recordsAffected,commandTimeOut);
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


        public bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            return UpdateRange(entities, paramDbConnection, paramDbTransaction, out recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
            }
            catch (Exception exceptionDb)
            {
                throw new RepositoryException(exceptionDb);
            }

            foreach (TEntity entity in entities)
            {
                StringBuilder updateRangeQuery = new StringBuilder();
                StringBuilder parameterList = new StringBuilder();
                StringBuilder where = new StringBuilder();

                string parameterName;
                dbCommand = dbConnection.CreateCommand();

                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttibute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                    var columnName = _syntaxProvider.GetSecureColumnName(customAttibute.ColumnName ?? i.Name);

                    parameterName = "@param_" + i.Name;

                    if (!customAttibute.IsPrimary)
                    {
                        parameterList.Append(string.Format("{0} = {1},", columnName, parameterName));
                    }
                    else
                    {
                        if (where.ToString() != String.Empty)
                            where.Append(" AND ");

                        where.Append(string.Format("({0} = {1})", columnName, parameterName));
                    }
                    ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                }

                if (parameterList.ToString() != string.Empty)
                {
                    parameterList.Remove(parameterList.Length - 1, 1);
                    updateRangeQuery.AppendLine(string.Format("UPDATE {0} SET {1} WHERE {2}; ", _syntaxProvider.GetSecureTableName(TableName), parameterList, where));
                }

                try
                {
                    dbCommand.CommandText = updateRangeQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected += dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();

                }
                catch (Exception exceptionCommand)
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                    throw new RepositoryException(exceptionCommand, dbCommand);
                }
            }
            if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
            return true;
        }
    }
}
