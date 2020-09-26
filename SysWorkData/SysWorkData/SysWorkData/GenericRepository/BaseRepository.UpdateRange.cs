using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Attributes;
using SysWork.Data.Common.DbInfo;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.Common.ValueObjects;

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

        public bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection)
        {
            return UpdateRange(entities, dbConnection, null, out long recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, int commandTimeOut)
        {
            return UpdateRange(entities, dbConnection, null, out long recordsAffected, commandTimeOut);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbTransaction dbTransaction)
        {
            return UpdateRange(entities, null, dbTransaction, out long recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return UpdateRange(entities, null, dbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return UpdateRange(entities, dbConnection, dbTransaction, out long recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return UpdateRange(entities, dbConnection, dbTransaction, out long recordsAffected, commandTimeOut);
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


        public bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            return UpdateRange(entities, dbConnection, dbTransaction, out recordsAffected, null);
        }

        public bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();
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
                dbCommand = dbConnectionInUse.CreateCommand();

                foreach (PropertyInfo i in EntityProperties)
                {
                    var dbColumn = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                    var columnName = _syntaxProvider.GetSecureColumnName(dbColumn.ColumnName ?? i.Name);

                    parameterName = "@param_" + i.Name;

                    if (!dbColumn.IsIdentity)
                        parameterList.Append(string.Format("{0} = {1},", columnName, parameterName));

                    if (dbColumn.IsPrimary)
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

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected += dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();

                }
                catch (Exception exceptionCommand)
                {
                    if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnectionInUse.Close();
                        dbConnectionInUse.Dispose();
                    }
                    throw new RepositoryException(exceptionCommand, dbCommand);
                }
            }
            if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
            {
                dbConnectionInUse.Close();
                dbConnectionInUse.Dispose();
            }
            return true;
        }
    }
}
