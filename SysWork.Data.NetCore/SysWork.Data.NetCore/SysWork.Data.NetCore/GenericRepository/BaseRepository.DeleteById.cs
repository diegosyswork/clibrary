using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.NetCore.Common;
using SysWork.Data.NetCore.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.NetCore.Common.Attributes;
using SysWork.Data.NetCore.Common.DbInfo;
using SysWork.Data.NetCore.GenericRepository.Exceptions;
using SysWork.Data.NetCore.Common.Interfaces.Actions;
using SysWork.Data.NetCore.Common.ValueObjects;

namespace SysWork.Data.NetCore.GenericRepository
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

        public bool DeleteById(long Id, IDbConnection dbConnection)
        {
            return DeleteById(Id, dbConnection, null, out long recordsAffected, null);
        }

        public bool DeleteById(long Id, IDbConnection dbConnection, int commandTimeOut)
        {
            return DeleteById(Id, dbConnection, null, out long recordsAffected, commandTimeOut);
        }

        public bool DeleteById(long Id, IDbTransaction dbTransaction)
        {
            return DeleteById(Id, null, dbTransaction, out long recordsAffected, null);
        }

        public bool DeleteById(long Id, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return DeleteById(Id, null, dbTransaction, out long recordsAffected, commandTimeOut);
        }

        public bool DeleteById(long Id, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return DeleteById(Id, dbConnection, dbTransaction, out long recordsAffected, null);
        }

        public bool DeleteById(long Id, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return DeleteById(Id, dbConnection, dbTransaction, out long recordsAffected, commandTimeOut);
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

        public bool DeleteById(long Id, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            return DeleteById(Id, dbConnection, dbTransaction, out recordsAffected, null);
        }

        public bool DeleteById(long Id, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            TEntity entity = new TEntity();
            StringBuilder where = new StringBuilder();

            string parameterName;
            foreach (PropertyInfo i in EntityProperties)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                var columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                parameterName = "@param_" + i.Name;

                if (customAttribute.IsIdentity)
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
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        dbConnectionInUse.Open();

                    dbCommand.CommandText = deleteQuery.ToString();

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
