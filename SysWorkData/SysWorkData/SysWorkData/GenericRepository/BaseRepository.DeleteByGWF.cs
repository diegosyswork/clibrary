using System;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IDeleteByGenericWhereFilter<TEntity>
    {
        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, null, out long recordsAffected, null);
        }
        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, null, out long recordsAffected, commandTimeOut);
        }
        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, out long recordsAffected)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, null, out recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, out long recordsAffected, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, null, out recordsAffected, commandTimeOut);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection)
        {
            return DeleteByGenericWhereFilter(whereFilter, paramDbConnection, null, out long recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, paramDbConnection, null, out long recordsAffected, commandTimeOut);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, out long recordsAffected)
        {
            return DeleteByGenericWhereFilter(whereFilter, paramDbConnection, null, out recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, out long recordsAffected, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, paramDbConnection, null, out recordsAffected, commandTimeOut);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, paramDbTransaction, out long recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, paramDbTransaction, out long recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, paramDbTransaction, out recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, out long recordsAffected, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, paramDbTransaction, out recordsAffected, commandTimeOut);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            return DeleteByGenericWhereFilter(whereFilter, paramDbConnection, paramDbTransaction, out recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            TEntity entity = new TEntity();

            if (!string.IsNullOrEmpty(whereFilter.Where))
            {
                string deleteQuery = whereFilter.DeleteQueryString;
                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    dbCommand.CommandText = deleteQuery.ToString();

                    foreach (var param in whereFilter.Parameters)
                    {
                        var dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = param.Key;
                        dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                        if (whereFilter.ParametersSize.TryGetValue(dbParameter.ParameterName, out int paramSize))
                            if (paramSize != 0)
                                dbParameter.Size = paramSize;

                        if (whereFilter.ParametersDbTye.TryGetValue(dbParameter.ParameterName, out DbType dbType))
                            dbParameter.DbType = dbType;

                        dbCommand.Parameters.Add(dbParameter);
                    }

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
            else
            {
                throw new ArgumentNullException("Where clause is not set");
            }
            return true;

        }

    }
}
