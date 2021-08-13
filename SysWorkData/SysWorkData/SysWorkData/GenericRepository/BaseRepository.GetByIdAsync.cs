using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.DbInfo;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;
using System.Data.Common;
using SysWork.Data.Mapping;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> 
    {
        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await GetByIdAsync(id, null, null, null);
        }

        public async Task<TEntity> GetByIdAsync(object id, int commandTimeOut)
        {
            return await GetByIdAsync(id, null, null, commandTimeOut);
        }

        public async Task<TEntity> GetByIdAsync(object id, DbConnection dbConnection)
        {
            return await GetByIdAsync(id, dbConnection, null, null);
        }

        public async Task<TEntity> GetByIdAsync(object id, DbConnection dbConnection, int commandTimeOut)
        {
            return await GetByIdAsync(id, dbConnection, null, commandTimeOut);
        }

        public async Task<TEntity> GetByIdAsync(object id, DbTransaction dbTransaction)
        {
            return await GetByIdAsync(id, null, dbTransaction, null);
        }

        public async Task<TEntity> GetByIdAsync(object id, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await GetByIdAsync(id, null, dbTransaction, commandTimeOut);
        }

        public async Task<TEntity> GetByIdAsync(object id, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await GetByIdAsync(id, dbConnection, dbTransaction, null);
        }

        public async Task<TEntity> GetByIdAsync(object id, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            TEntity entity = new TEntity();

            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            foreach (var pi in _entityProperties)
            {
                var pk = pi.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                if (pk != null && pk.IsIdentity)
                {
                    var columnName = _syntaxProvider.GetSecureColumnName(pk.Name?? pi.Name);

                    string parameterName = "@param_" + pi.Name;
                    clause.Append(string.Format("{0}={1}", columnName, id));

                    DbColumnInfo cdbi = (DbColumnInfo)_columnListWithDbInfo[pi.Name];
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
                        await dbConnectionInUse.OpenAsync();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_databaseEngine == EDatabaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    IDataReader reader = await dbCommand.ExecuteReaderAsync();

                    if (reader.Read())
                        entity = await _mapper.MapSingleAsync<TEntity>(reader, _entityProperties);
                    else
                        entity = default;

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
