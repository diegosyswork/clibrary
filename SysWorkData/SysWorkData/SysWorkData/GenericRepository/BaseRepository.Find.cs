using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Mapping;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public IList<TEntity> Find(IEnumerable<object> ids)
        {
            return Find(ids, null, null, null);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, int commandTimeOut)
        {
            return Find(ids, null, null, commandTimeOut);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection)
        {
            return Find(ids, dbConnection, null, null);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection, int commandTimeOut)
        {
            return Find(ids, dbConnection,null, commandTimeOut );
        }


        public IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction dbTransaction)
        {
            return Find(ids, null, dbTransaction, null);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return Find(ids, null, dbTransaction, commandTimeOut);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return Find(ids, dbConnection, dbTransaction,null);
        }

        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> entities = new List<TEntity>();
            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            foreach (var pi in EntityProperties)
            {
                var pk = pi.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                if (pk != null && pk.IsIdentity)
                {
                    var columnName = _syntaxProvider.GetSecureColumnName(pk.Name ?? pi.Name);

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
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        dbConnectionInUse.Open();

                    dbCommand.CommandText = findQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    IDataReader reader = dbCommand.ExecuteReader();
                    entities = _mapper.Map<TEntity>(reader, EntityProperties, _databaseEngine);

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
            return entities;
        }
    }
}
