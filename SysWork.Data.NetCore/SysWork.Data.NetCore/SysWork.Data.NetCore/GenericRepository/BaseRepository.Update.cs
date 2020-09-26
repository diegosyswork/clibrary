using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.NetCore.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.NetCore.Common.Attributes;
using SysWork.Data.NetCore.Common.DbInfo;
using SysWork.Data.NetCore.GenericRepository.Exceptions;
using SysWork.Data.NetCore.Common.Interfaces.Actions;
using SysWork.Data.NetCore.Common.ValueObjects;

namespace SysWork.Data.NetCore.GenericRepository
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

        public bool Update(TEntity entity, IDbConnection dbConnection)
        {
            return Update(entity, dbConnection, null, out long recordsAffected, null);
        }

        public bool Update(TEntity entity, IDbConnection dbConnection, int commandTimeOut)
        {
            return Update(entity, dbConnection, null, out long recordsAffected, commandTimeOut);
        }


        public bool Update(TEntity entity, IDbTransaction dbTransaction)
        {
            return Update(entity, null, dbTransaction, out long recordsAffected, null);
        }

        public bool Update(TEntity entity, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return Update(entity, null, dbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return Update(entity, dbConnection, dbTransaction, out long recordsAffected, null);
        }

        public bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return Update(entity, dbConnection, dbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            return Update(entity, dbConnection, dbTransaction, out recordsAffected,null);
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


        public bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            StringBuilder parameterList = new StringBuilder();
            StringBuilder where = new StringBuilder();

            bool hasPrimary = false;

            string parameterName;
            foreach (PropertyInfo i in EntityProperties)
            {
                var dbColumn = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                parameterName = "@param_" + i.Name;
                var columnName = _syntaxProvider.GetSecureColumnName(dbColumn.ColumnName ?? i.Name);

                if (!dbColumn.IsIdentity)
                    parameterList.Append(string.Format("{0} = {1},", columnName, parameterName));

                if (dbColumn.IsPrimary)
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
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        dbConnectionInUse.Open();

                    dbCommand.CommandText = updateQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

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
                    if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnectionInUse.Close();
                        dbConnectionInUse.Dispose();
                    }
                }
            }
            return true;
        }
    }
}
