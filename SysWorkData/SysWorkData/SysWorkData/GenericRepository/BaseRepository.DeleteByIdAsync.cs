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
        public async Task<bool> DeleteByIdAsync(long Id)
        {
            return await DeleteByIdAsync(Id, null, null, null);
        }
        public async Task<bool> DeleteByIdAsync(long Id, int commandTimeOut)
        {
            return await DeleteByIdAsync(Id, null, null, commandTimeOut);
        }

        public async Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection)
        {
            return await DeleteByIdAsync(Id, dbConnection, null, null);
        }

        public async Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, int commandTimeOut)
        {
            return await DeleteByIdAsync(Id, dbConnection, null, commandTimeOut);
        }

        public async Task<bool> DeleteByIdAsync(long Id, DbTransaction dbTransaction)
        {
            return await DeleteByIdAsync(Id, null, dbTransaction, null);
        }

        public async Task<bool> DeleteByIdAsync(long Id, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await DeleteByIdAsync(Id, null, dbTransaction, commandTimeOut);
        }

        public async Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await DeleteByIdAsync(Id, dbConnection, dbTransaction, null);
        }

        public async Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await DeleteByIdAsync(Id, dbConnection, dbTransaction, commandTimeOut);
        }

        public async Task<bool> DeleteByIdAsync(long Id, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            TEntity entity = new TEntity();
            StringBuilder where = new StringBuilder();

            string parameterName;
            foreach (PropertyInfo i in _entityProperties)
            {
                var customAttribute = i.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                var columnName = _syntaxProvider.GetSecureColumnName(customAttribute.Name ?? i.Name);

                parameterName = "@param_" + i.Name;

                if (customAttribute.IsIdentity)
                {
                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("({0} = {1})", columnName, parameterName));

                    DbColumnInfo cdbi = (DbColumnInfo)_columnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, Id, cdbi.MaxLenght));

                }
            }

            if (where.ToString() != string.Empty)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(string.Format("DELETE FROM {0} WHERE {1}", _syntaxProvider.GetSecureTableName(TableName), where));

                try
                {
                    dbCommand.CommandText = deleteQuery.ToString();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_databaseEngine == EDatabaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    if (dbConnectionInUse.State != ConnectionState.Open)
                        await dbConnectionInUse.OpenAsync();

                    await dbCommand.ExecuteNonQueryAsync();

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
            return true;
        }
    }
}
