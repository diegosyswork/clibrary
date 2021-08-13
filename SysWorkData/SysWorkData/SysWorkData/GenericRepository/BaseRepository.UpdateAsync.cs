using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Attributes;
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
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await UpdateAsync(entity, null, null,  null);
        }

        public async Task<bool> UpdateAsync(TEntity entity, int commandTimeOut)
        {
            return await UpdateAsync(entity, null, null, commandTimeOut);
        }

        public async Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection)
        {
            return await UpdateAsync(entity, dbConnection, null, null);
        }

        public async Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection, int commandTimeOut)
        {
            return await UpdateAsync(entity, dbConnection, null, commandTimeOut);
        }


        public async Task<bool> UpdateAsync(TEntity entity, DbTransaction dbTransaction)
        {
            return await UpdateAsync(entity, null, dbTransaction, null);
        }

        public async Task<bool> UpdateAsync(TEntity entity, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await UpdateAsync(entity, null, dbTransaction, commandTimeOut);
        }

        public async Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await UpdateAsync(entity, dbConnection, dbTransaction,  null);
        }

        public async Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await UpdateAsync(entity, dbConnection, dbTransaction, commandTimeOut);
        }

        public async Task<bool> UpdateAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            StringBuilder parameterList = new StringBuilder();
            StringBuilder where = new StringBuilder();

            bool hasPrimary = false;

            string parameterName;
            foreach (PropertyInfo i in _entityProperties)
            {
                var column = i.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                parameterName = "@param_" + i.Name;
                var columnName = _syntaxProvider.GetSecureColumnName(column.Name ?? i.Name);

                if (!column.IsIdentity)
                    parameterList.Append(string.Format("{0} = {1},", columnName, parameterName));

                if (column.IsPrimaryKey)
                {
                    hasPrimary = true;

                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("({0} = {1})", columnName, parameterName));
                }

                DbColumnInfo cdbi = (DbColumnInfo)_columnListWithDbInfo[i.Name];
                dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
            }

            if (!hasPrimary)
            {
                throw new ArgumentException("There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True");
            }

            if (parameterList.ToString() != string.Empty)
            {
                parameterList.Remove(parameterList.Length - 1, 1);
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append(string.Format("UPDATE  {0} SET {1} WHERE {2}", _syntaxProvider.GetSecureTableName(TableName), parameterList, where));

                try
                {
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        await dbConnectionInUse.OpenAsync();

                    dbCommand.CommandText = updateQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_databaseEngine == EDatabaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected = await dbCommand.ExecuteNonQueryAsync();
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
