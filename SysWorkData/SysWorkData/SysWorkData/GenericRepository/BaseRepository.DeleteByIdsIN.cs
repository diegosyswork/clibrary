using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.Mapping;
using System.Collections.Generic;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity>
    {
        public bool DeleteByIdsIN(IEnumerable<object> ids)
        {
            return DeleteByIdsIN(ids, null, null, out _, null);
        }
        public bool DeleteByIdsIN(IEnumerable<object> ids, int commandTimeOut)
        {
            return DeleteByIdsIN(ids, null, null, out _, commandTimeOut);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, IDbConnection dbConnection)
        {
            return DeleteByIdsIN(ids, dbConnection, null, out _, null);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, IDbConnection dbConnection, int commandTimeOut)
        {
            return DeleteByIdsIN(ids, dbConnection, null, out _, commandTimeOut);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, IDbTransaction dbTransaction)
        {
            return DeleteByIdsIN(ids, null, dbTransaction, out _, null);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return DeleteByIdsIN(ids, null, dbTransaction, out _, commandTimeOut);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return DeleteByIdsIN(ids, dbConnection, dbTransaction, out _, null);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return DeleteByIdsIN(ids, dbConnection, dbTransaction, out _, commandTimeOut);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, out string errMessage)
        {
            return DeleteByIdsIN(ids, out errMessage, null);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, out string errMessage, int commandTimeOut)
        {
            return DeleteByIdsIN(ids, out errMessage, commandTimeOut);
        }
        private bool DeleteByIdsIN(IEnumerable<object> ids, out string errMessage, int? commandTimeOut)
        {
            if (ids is null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            errMessage = "";


            bool result;
            try
            {
                result = DeleteByIdsIN(ids, null, null, out long recordsAffected, commandTimeOut);
            }
            catch (RepositoryException genericRepositoryException)
            {
                errMessage = genericRepositoryException.OriginalException.Message;
                result = false;
            }
            catch (Exception exception)
            {
                errMessage = exception.Message;
                result = false;
            }
            return result;
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            return DeleteByIdsIN(ids, dbConnection, dbTransaction, out recordsAffected, null);
        }

        public bool DeleteByIdsIN(IEnumerable<object> ids, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut)
        {
            recordsAffected = 0;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

            StringBuilder where = new StringBuilder();

            foreach (var pi in _entityProperties)
            {
                var idCol = pi.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;

                if (idCol != null && idCol.IsIdentity)
                {
                    var columnName = _syntaxProvider.GetSecureColumnName(idCol.Name ?? pi.Name);

                    string _ids = string.Empty;
                    foreach (var id in ids)
                    {
                        if (_ids != string.Empty)
                            _ids += ",";

                        _ids += id.ToString();
                    }

                    where.Append(string.Format("{0} IN ({1})", columnName, _ids));
                    break;
                }
            }

            if (where.ToString() != string.Empty)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(string.Format("DELETE FROM {0} WHERE {1}", _syntaxProvider.GetSecureTableName(TableName), where));

                try
                {
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        dbConnectionInUse.Open();

                    dbCommand.CommandText = deleteQuery.ToString();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_databaseEngine == EDatabaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected = dbCommand.ExecuteNonQuery();
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
