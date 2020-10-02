using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq.Expressions;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam)
        {
            return GetListBySqlLam(sqlLam, null, null, null);
        }

        public IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, int commandTimeOut)
        {
            return GetListBySqlLam(sqlLam, null, null, commandTimeOut);
        }

        public IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbConnection dbConnection)
        {
            return GetListBySqlLam(sqlLam, dbConnection, null, null);
        }

        public IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetListBySqlLam(sqlLam, dbConnection, null, commandTimeOut);
        }

        public IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbTransaction dbTransaction)
        {
            return GetListBySqlLam(sqlLam, null, dbTransaction, null);
        }

        public IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetListBySqlLam(sqlLam, null, dbTransaction, commandTimeOut);
        }

        public IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetListBySqlLam(sqlLam, dbConnection, dbTransaction, null);
        }

        public IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> result = new List<TEntity>();
            SetSqlLamAdapter();

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

                dbCommand.CommandText = sqlLam.QueryString;
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                foreach (var parameters in sqlLam.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_databaseEngine == EDatabaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader();
                result = _mapper.Map<TEntity>(reader, EntityProperties, _databaseEngine);

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
