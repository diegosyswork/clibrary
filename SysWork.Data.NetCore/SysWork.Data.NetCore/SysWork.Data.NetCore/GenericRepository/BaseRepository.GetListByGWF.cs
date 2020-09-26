using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using SysWork.Data.NetCore.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.NetCore.Common.Filters;
using SysWork.Data.NetCore.GenericRepository.Exceptions;
using SysWork.Data.NetCore.Common.Interfaces.Actions;
using SysWork.Data.NetCore.Common.Mapper;
using SysWork.Data.NetCore.Common.ValueObjects;

namespace SysWork.Data.NetCore.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IGetListByGenericWhereFilter<TEntity>
   {
        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter)
        {
            return GetListByGenericWhereFilter(whereFilter, null, null, null);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut)
        {
            return GetListByGenericWhereFilter(whereFilter, null, null, commandTimeOut);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection)
        {
            return GetListByGenericWhereFilter(whereFilter, dbConnection, null, null);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetListByGenericWhereFilter(whereFilter, dbConnection, null, commandTimeOut);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction)
        {
            return GetListByGenericWhereFilter(whereFilter, null, dbTransaction, null);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetListByGenericWhereFilter(whereFilter, null, dbTransaction, commandTimeOut);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetListByGenericWhereFilter(whereFilter, dbConnection, dbTransaction, null);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();

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

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                var reader = dbCommand.ExecuteReader();
                result = _mapper.Map<TEntity>(reader, EntityProperties, _dataBaseEngine);

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
            return result;
        }
    }
}
