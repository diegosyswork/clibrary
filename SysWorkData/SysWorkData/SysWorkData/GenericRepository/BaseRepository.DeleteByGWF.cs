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

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection)
        {
            return DeleteByGenericWhereFilter(whereFilter, dbConnection, null, out long recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, dbConnection, null, out long recordsAffected, commandTimeOut);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, out long recordsAffected)
        {
            return DeleteByGenericWhereFilter(whereFilter, dbConnection, null, out recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, out long recordsAffected, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, dbConnection, null, out recordsAffected, commandTimeOut);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, dbTransaction, out long recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, dbTransaction, out long recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, out long recordsAffected)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, dbTransaction, out recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut)
        {
            return DeleteByGenericWhereFilter(whereFilter, null, dbTransaction, out recordsAffected, commandTimeOut);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            return DeleteByGenericWhereFilter(whereFilter, dbConnection, dbTransaction, out recordsAffected, null);
        }

        public bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            TEntity entity = new TEntity();

            if (!string.IsNullOrEmpty(whereFilter.Where))
            {
                string deleteQuery = whereFilter.DeleteQueryString;
                try
                {
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        dbConnectionInUse.Open();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    dbCommand.CommandText = deleteQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

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
                    if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnectionInUse.Close();
                        dbConnectionInUse.Dispose();
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
