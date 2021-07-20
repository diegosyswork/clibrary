using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using SysWork.Data.Common.Attributes;
using SysWork.Data.GenericRepository.Exceptions;
using System.Threading.Tasks;
using System.Data.Common;
using SysWork.Data.Mapping;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public async Task<IList<TEntity>> FindAsync(IEnumerable<object> ids)
        {
            return await FindAsync(ids, null, null, null);
        }

        public async Task<IList<TEntity>> FindAsync(IEnumerable<object> ids, int commandTimeOut)
        {
            return await FindAsync(ids, null, null, commandTimeOut);
        }

        public async Task<IList<TEntity>> FindAsync(IEnumerable<object> ids, DbConnection dbConnection)
        {
            return await FindAsync(ids, dbConnection, null, null);
        }

        public async Task<IList<TEntity>> FindAsync(IEnumerable<object> ids, DbConnection dbConnection, int commandTimeOut)
        {
            return await FindAsync(ids, dbConnection,null, commandTimeOut );
        }


        public async Task<IList<TEntity>> FindAsync(IEnumerable<object> ids, DbTransaction dbTransaction)
        {
            return await FindAsync(ids, null, dbTransaction, null);
        }

        public async Task<IList<TEntity>> FindAsync(IEnumerable<object> ids, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await FindAsync(ids, null, dbTransaction, commandTimeOut);
        }

        public async Task<IList<TEntity>> FindAsync(IEnumerable<object> ids, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await FindAsync(ids, dbConnection, dbTransaction,null);
        }

        public async Task<IList<TEntity>> FindAsync(IEnumerable<object> ids, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            IList<TEntity> entities = new List<TEntity>();
            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

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
                        await dbConnectionInUse.OpenAsync();

                    dbCommand.CommandText = findQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    entities = await _mapper.MapAsync<TEntity>(await dbCommand.ExecuteReaderAsync(), EntityProperties, _databaseEngine);

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
