using System;
using System.Collections.Generic;
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
        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities)
        {
            return await UpdateRangeAsync(entities, null, null,  null);
        }

        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities, int commandTimeOut)
        {
            return await UpdateRangeAsync(entities, null, null,  commandTimeOut);
        }

        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection)
        {
            return await UpdateRangeAsync(entities, dbConnection, null,  null);
        }

        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection, int commandTimeOut)
        {
            return await UpdateRangeAsync(entities, dbConnection, null, commandTimeOut);
        }

        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbTransaction dbTransaction)
        {
            return await UpdateRangeAsync(entities, null, dbTransaction, null);
        }

        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await UpdateRangeAsync(entities, null, dbTransaction,  commandTimeOut);
        }

        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await UpdateRangeAsync(entities, dbConnection, dbTransaction, null);
        }

        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await UpdateRangeAsync(entities, dbConnection, dbTransaction, commandTimeOut);
        }

        public async Task<bool> UpdateRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    await dbConnectionInUse.OpenAsync();
            }
            catch (Exception exceptionDb)
            {
                throw new RepositoryException(exceptionDb);
            }

            foreach (TEntity entity in entities)
            {
                StringBuilder updateRangeQuery = new StringBuilder();
                StringBuilder parameterList = new StringBuilder();
                StringBuilder where = new StringBuilder();

                string parameterName;
                dbCommand = dbConnectionInUse.CreateCommand();

                foreach (PropertyInfo i in _entityProperties)
                {
                    var column = i.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                    var columnName = _syntaxProvider.GetSecureColumnName(column.Name ?? i.Name);

                    parameterName = "@param_" + i.Name;

                    if (!column.IsIdentity)
                        parameterList.Append(string.Format("{0} = {1},", columnName, parameterName));

                    if (column.IsPrimaryKey)
                    {
                        if (where.ToString() != String.Empty)
                            where.Append(" AND ");

                        where.Append(string.Format("({0} = {1})", columnName, parameterName));
                    }


                    DbColumnInfo cdbi = (DbColumnInfo)_columnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                }

                if (parameterList.ToString() != string.Empty)
                {
                    parameterList.Remove(parameterList.Length - 1, 1);
                    updateRangeQuery.AppendLine(string.Format("UPDATE {0} SET {1} WHERE {2}; ", _syntaxProvider.GetSecureTableName(TableName), parameterList, where));
                }

                try
                {
                    dbCommand.CommandText = updateRangeQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_databaseEngine == EDatabaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected += await dbCommand.ExecuteNonQueryAsync();
                    dbCommand.Dispose();

                }
                catch (Exception exceptionCommand)
                {
                    if ((closeConnection) && (dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open))
                    {
                        dbConnectionInUse.Close();
                        dbConnectionInUse.Dispose();
                    }
                    throw new RepositoryException(exceptionCommand, dbCommand);
                }
            }
            if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
            {
                dbConnectionInUse.Close();
                dbConnectionInUse.Dispose();
            }
            return true;
        }
    }
}
