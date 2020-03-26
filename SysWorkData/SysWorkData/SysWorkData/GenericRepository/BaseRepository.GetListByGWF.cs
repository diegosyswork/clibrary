using System;
using System.Collections.Generic;
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

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection)
        {
            return GetListByGenericWhereFilter(whereFilter, paramDbConnection, null, null);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return GetListByGenericWhereFilter(whereFilter, paramDbConnection, null, commandTimeOut);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction)
        {
            return GetListByGenericWhereFilter(whereFilter, null, paramDbTransaction, null);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return GetListByGenericWhereFilter(whereFilter, null, paramDbTransaction, commandTimeOut);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return GetListByGenericWhereFilter(whereFilter, paramDbConnection, paramDbTransaction, null);
        }

        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

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


                IDataReader reader = dbCommand.ExecuteReader();
                result = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo, _dataBaseEngine);

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
            return result;
        }
    }
}
