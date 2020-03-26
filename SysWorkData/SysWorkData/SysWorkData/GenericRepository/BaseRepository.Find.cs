using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using SysWork.Data.GenericRepository.Attributes;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces.Actions;
using SysWork.Data.GenericRepository.Mapper;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IFind<TEntity>
    {
        public IList<TEntity> Find(IEnumerable<object> ids)
        {
            return Find(ids, null, null, null);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, int commandTimeOut)
        {
            return Find(ids, null, null, commandTimeOut);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection)
        {
            return Find(ids, paramDbConnection, null, null);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return Find(ids, paramDbConnection,null, commandTimeOut );
        }


        public IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction paramDbTransaction)
        {
            return Find(ids, null, paramDbTransaction, null);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return Find(ids, null, paramDbTransaction, commandTimeOut);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return Find(ids, paramDbConnection, paramDbTransaction,null);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            IList<TEntity> entities = new List<TEntity>();
            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            foreach (var pi in ListObjectPropertyInfo)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsPrimary)
                {
                    var columnName = _syntaxProvider.GetSecureColumnName(pk.ColumnName ?? pi.Name);

                    string _ids = string.Empty;
                    foreach (var id in ids)
                    {
                        if (_ids != string.Empty)
                            _ids = _ids + ",";

                        _ids = _ids + id.ToString();
                    }

                    clause.Append(string.Format("{0} IN ({1})", columnName, _ids));
                    break;
                }
            }

            if (clause.ToString() != string.Empty)
            {
                StringBuilder findQuery = new StringBuilder();
                findQuery.Append(string.Format("SELECT {0} FROM {1} WHERE {2}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName), clause));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = findQuery.ToString();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    IDataReader reader = dbCommand.ExecuteReader();
                    entities = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo, _dataBaseEngine);

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
            return entities;
        }
    }
}
