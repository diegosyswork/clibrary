using System;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Filters;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;
using SysWork.Data.GenericRepository.Mapper;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IGetByGenericWhereFilter<TEntity>
    {
        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter)
        {
            return GetByGenericWhereFilter(whereFilter, null, null, null);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return GetByGenericWhereFilter(whereFilter, null, null, commandTimeOut);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection)
        {
            return GetByGenericWhereFilter(whereFilter, paramDbConnection, null, null);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return GetByGenericWhereFilter(whereFilter, paramDbConnection, null, commandTimeOut);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction)
        {
            return GetByGenericWhereFilter(whereFilter, null, paramDbTransaction, null);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return GetByGenericWhereFilter(whereFilter, null, paramDbTransaction, commandTimeOut);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return GetByGenericWhereFilter(whereFilter, paramDbConnection, paramDbTransaction, null);
        }

        public TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            TEntity entity = new TEntity();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

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

                IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                    entity = new MapDataReaderToEntity().MapSingle<TEntity>(reader, ListObjectPropertyInfo);
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
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return entity;
        }
    }
}
