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
    public abstract partial class BaseRepository<TEntity> : IUpdate<TEntity>
    {
        public bool Update(TEntity entity)
        {
            return Update(entity, null, null, out long recordsAffected, null);
        }

        public bool Update(TEntity entity, int commandTimeOut)
        {
            return Update(entity, null, null, out long recordsAffected, commandTimeOut);
        }

        public bool Update(TEntity entity, IDbConnection paramDbConnection)
        {
            return Update(entity, paramDbConnection, null, out long recordsAffected, null);
        }

        public bool Update(TEntity entity, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return Update(entity, paramDbConnection, null, out long recordsAffected, commandTimeOut);
        }


        public bool Update(TEntity entity, IDbTransaction paramDbTransaction)
        {
            return Update(entity, null, paramDbTransaction, out long recordsAffected, null);
        }

        public bool Update(TEntity entity, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return Update(entity, null, paramDbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return Update(entity, paramDbConnection, paramDbTransaction, out long recordsAffected, null);
        }

        public bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return Update(entity, paramDbConnection, paramDbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            return Update(entity, paramDbConnection, paramDbTransaction, out recordsAffected,null);
        }

        public bool Update(TEntity entity, out string errMessage)
        {
            return Update(entity, out errMessage, null);
        }

        public bool Update(TEntity entity, out string errMessage, int commandTimeOut)
        {
            return Update(entity, out errMessage, commandTimeOut);
        }

        private bool Update(TEntity entity, out string errMessage, int? commandTimeOut)
        {
            errMessage = "";
            bool result = false;

            try
            {
                result = Update(entity,null,null,out long recordsAffected,commandTimeOut);
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


        public bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            StringBuilder parameterList = new StringBuilder();
            StringBuilder where = new StringBuilder();

            bool hasPrimary = false;

            string parameterName;
            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                parameterName = "@param_" + i.Name;
                var columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                if (!customAttribute.IsPrimary)
                {
                    parameterList.Append(string.Format("{0} = {1},", columnName, parameterName));
                }
                else
                {
                    hasPrimary = true;

                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("({0} = {1})", columnName, parameterName));
                }

                ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];
                dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
            }

            if (!hasPrimary)
            {
                throw new ArgumentException("There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True");
            }

            if (parameterList.ToString() != string.Empty)
            {
                parameterList.Remove(parameterList.Length - 1, 1);
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append(string.Format("UPDATE  {0} SET {1} WHERE {2}", _syntaxProvider.GetSecureTableName(TableName), parameterList, where));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = updateQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

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
