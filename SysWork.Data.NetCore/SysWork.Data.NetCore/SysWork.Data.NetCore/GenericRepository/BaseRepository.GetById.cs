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
using SysWork.Data.NetCore.Common.Mapper;
using SysWork.Data.NetCore.Common.ValueObjects;

namespace SysWork.Data.NetCore.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IGetById<TEntity>
    {
        public TEntity GetById(object id)
        {
            return GetById(id, null, null, null);
        }

        public TEntity GetById(object id, int commandTimeOut)
        {
            return GetById(id, null, null, commandTimeOut);
        }


        public TEntity GetById(object id, IDbConnection dbConnection)
        {
            return GetById(id, dbConnection, null, null);
        }

        public TEntity GetById(object id, IDbConnection dbConnection, int commandTimeOut)
        {
            return GetById(id, dbConnection, null, commandTimeOut);
        }

        public TEntity GetById(object id, IDbTransaction dbTransaction)
        {
            return GetById(id, null, dbTransaction, null);
        }

        public TEntity GetById(object id, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return GetById(id, null, dbTransaction, commandTimeOut);
        }

        public TEntity GetById(object id, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return GetById(id, dbConnection, dbTransaction, null);
        }

        public TEntity GetById(object id, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity entity = new TEntity();

            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            foreach (var pi in EntityProperties)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsIdentity)
                {
                    var columnName = _syntaxProvider.GetSecureColumnName(pk.ColumnName ?? pi.Name);

                    string parameterName = "@param_" + pi.Name;
                    clause.Append(string.Format("{0}={1}", columnName, id));

                    ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[pi.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, id, cdbi.MaxLenght));

                    break;
                }
            }

            entity = null;
            if (clause.ToString() != string.Empty)
            {
                StringBuilder getQuery = new StringBuilder();
                getQuery.Append(string.Format("SELECT {0} FROM {1} WHERE {2}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName), clause));

                dbCommand.CommandText = getQuery.ToString();
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                try
                {
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        dbConnectionInUse.Open();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    IDataReader reader = dbCommand.ExecuteReader();

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
            }
            return entity;
        }

    }
}
