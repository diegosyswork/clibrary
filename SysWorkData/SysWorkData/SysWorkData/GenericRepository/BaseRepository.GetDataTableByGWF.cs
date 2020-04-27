using System;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.Common.Filters;
using System.Data.Common;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IGetDataTableByGenericWhereFilter<TEntity>
    {
        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, null, null, null);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeout)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, null, null, commandTimeout);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, dbConnection, null, null);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeout)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, dbConnection, null, commandTimeout);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, null, dbTransaction, null);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeout)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, null, dbTransaction, commandTimeout);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, dbConnection, dbTransaction, null);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? CommandTimeout)
        {
            DataTable result = new DataTable(TableName);

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = (DbConnection)dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandTimeout = CommandTimeout ?? _defaultCommandTimeout;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = (DbTransaction)dbTransaction;

                dbCommand.CommandText = whereFilter.SelectQueryString;

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

                DbDataAdapter dbDataAdapter = _dbObjectProvider.GetDbDataAdapter();
                dbDataAdapter.SelectCommand = dbCommand;
                dbDataAdapter.Fill(result);

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
            return result;
        }
    }
}




