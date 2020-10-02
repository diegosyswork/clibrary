using System;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.Common.Interfaces.Actions;
using SysWork.Data.Common.Mapper;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository.Exceptions;

namespace SysWork.Data.GenericViewManager
{
    public abstract partial class BaseViewManager<TEntity> : IGetByGenericWhereFilter<TEntity>
    {
        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter)
        {
            return GetByGenericWhereFilter(whereFilter, null, null, null);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return GetByGenericWhereFilter(whereFilter, null, null, commandTimeOut);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection)
        {
            return GetByGenericWhereFilter(whereFilter, dbConnection, null, null);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetByGenericWhereFilter(whereFilter, dbConnection, null, commandTimeOut);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction)
        {
            return GetByGenericWhereFilter(whereFilter, null, dbTransaction, null);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetByGenericWhereFilter(whereFilter, null, dbTransaction, commandTimeOut);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetByGenericWhereFilter(whereFilter, dbConnection, dbTransaction, null);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity entity = new TEntity();

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

                dbCommand.CommandText = whereFilter.SelectQueryString;
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

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                    entity = _mapper.MapSingle<TEntity>(reader, EntityProperties);
                else
                    entity = default(TEntity);

                reader.Close();
                reader.Dispose();
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
            return entity;
        }
    }
}
