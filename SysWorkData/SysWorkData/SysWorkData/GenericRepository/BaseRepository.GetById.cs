using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Attributes;
using SysWork.Data.GenericRepository.DbInfo;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;
using SysWork.Data.GenericRepository.Mapper;

namespace SysWork.Data.GenericRepository
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


        public TEntity GetById(object id, IDbConnection paramDbConnection)
        {
            return GetById(id, paramDbConnection, null, null);
        }

        public TEntity GetById(object id, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return GetById(id, paramDbConnection, null, commandTimeOut);
        }

        public TEntity GetById(object id, IDbTransaction paramDbTransaction)
        {
            return GetById(id, null, paramDbTransaction, null);
        }

        public TEntity GetById(object id, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return GetById(id, null, paramDbTransaction, commandTimeOut);
        }

        public TEntity GetById(object id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return GetById(id, paramDbConnection, paramDbTransaction, null);
        }

        public TEntity GetById(object id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            TEntity entity = new TEntity();

            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            foreach (var pi in ListObjectPropertyInfo)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsPrimary)
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
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    IDataReader reader = dbCommand.ExecuteReader();

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
            }
            return entity;
        }

    }
}
