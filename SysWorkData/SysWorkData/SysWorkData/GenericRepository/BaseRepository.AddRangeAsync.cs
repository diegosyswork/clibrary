using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.DbInfo;
using System.Collections.Generic;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;
using System.Data.Common;
using SysWork.Data.Mapping;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity>
    {
        public async Task<bool> AddRangeAsync(IList<TEntity> entities)
        {
            return await AddRangeAsync(entities, null, null, null);
        }

        public async Task<bool> AddRangeAsync(IList<TEntity> entities, int commandTimeOut)
        {
            return await AddRangeAsync(entities, null, null,commandTimeOut);
        }

        public async Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection)
        {
            return await AddRangeAsync(entities, dbConnection, null, null);
        }

        public async Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection, int commandTimeOut)
        {
            return await AddRangeAsync(entities, dbConnection, null, commandTimeOut);
        }

        public async Task<bool> AddRangeAsync(IList<TEntity> entities, DbTransaction dbTransaction)
        {
            return await AddRangeAsync(entities, null, dbTransaction, null);
        }

        public async Task<bool> AddRangeAsync(IList<TEntity> entities, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await AddRangeAsync(entities, null, dbTransaction, commandTimeOut);
        }


        public async Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await AddRangeAsync(entities, dbConnection, dbTransaction, null);
        }

        public async Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await AddRangeAsync(entities, dbConnection, dbTransaction, commandTimeOut);
        }



        public async Task<bool> AddRangeAsync(IList<TEntity> entities, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            var idList = new List<object>();
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
            catch (Exception connectionException)
            {
                throw new RepositoryException(connectionException);
            }

                foreach (TEntity entity in entities)
                {
                    string parameterList = "";
                    dbCommand = dbConnectionInUse.CreateCommand();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    string addRangeQuery = "";
                    foreach (PropertyInfo i in _entityProperties)
                    {
                        var column = i.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                        if (!column.IsIdentity)
                        {
                            string parameterName = "@param_" + i.Name;
                            parameterList += string.Format("{0},", parameterName);

                            DbColumnInfo cdbi = (DbColumnInfo)_columnListWithDbInfo[i.Name];
                            dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                        }
                    }

                    if (parameterList != string.Empty)
                    {
                        parameterList = parameterList.Substring(0, parameterList.Length - 1);
                        addRangeQuery = (string.Format("INSERT INTO {0} ( {1} ) VALUES ( {2} ) {3};", _syntaxProvider.GetSecureTableName(TableName), ColumnsForInsert, parameterList, _syntaxProvider.GetSubQueryGetIdentity()));
                    }

                    try
                    {
                        dbCommand.CommandText = addRangeQuery;
                        if (dbTransaction != null)
                            dbCommand.Transaction = dbTransaction;

                        if (_databaseEngine == EDatabaseEngine.OleDb)
                            ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                        await (dbCommand.ExecuteNonQueryAsync());
                        dbCommand.Dispose();
                    }
                    catch (Exception commandException)
                    {
                        // In case of exception, if the command is open, close it.
                        if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                        {
                            if (dbConnectionInUse.State == ConnectionState.Open)
                                dbConnectionInUse.Close();

                            dbConnectionInUse.Dispose();
                        }

                        throw new RepositoryException(commandException, dbCommand);
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
