using System;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;
using SysWork.Data.Common.Filters;
using System.Data.Common;

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

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, paramDbConnection, null, null);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, int commandTimeout)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, paramDbConnection, null, commandTimeout);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, null, paramDbTransaction, null);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, int commandTimeout)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, null, paramDbTransaction, commandTimeout);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return GetDataTableByGenericWhereFilter(whereFilter, paramDbConnection, paramDbTransaction, null);
        }

        public DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? CommandTimeout)
        {
            DataTable result = new DataTable(TableName);

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            DbConnection dbConnection = (DbConnection)paramDbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandTimeout = CommandTimeout ?? _defaultCommandTimeout;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = (DbTransaction)paramDbTransaction;

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
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return result;


        }

    }
}




