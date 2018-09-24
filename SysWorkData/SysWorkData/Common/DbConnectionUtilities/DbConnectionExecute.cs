using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common.ObjectResolver;
using SysWork.Data.Extensions.OleDbCommandExtensions;

namespace SysWork.Data.Common.DbConnectionUtilities
{

    /// <summary>
    /// Date: 20/09/2018
    /// Author: Diego Javier Martinez
    /// Mail: dmartinez@syswork.com.ar
    /// </summary>
    public class DbConnectionExecute
    {
        private EDataBaseEngine _dataBaseEngine;
        private string _connectionString;

        private string _sqlQuery;
        private IDictionary<string, object> _queryParameters;


        public DbConnectionExecute(string connectionString)
        {
            ConstructorResolver(connectionString, EDataBaseEngine.MSSqlServer);
        }
        public DbConnectionExecute(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            ConstructorResolver(connectionString, dataBaseEngine);
        }

        private void ConstructorResolver(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            _connectionString = connectionString;
            _dataBaseEngine = dataBaseEngine;

            _queryParameters = new Dictionary<string, object>();
        }

        public DbConnectionExecute SqlQuery(string sqlQuery)
        {
            _sqlQuery = sqlQuery;
            return this;
        }

        public DbConnectionExecute AddParameters(string name, object value)
        {
            _queryParameters.Add(name, value);
            return this;
        }

        public long ExecuteNonQuery()
        {
            return ExecuteNonQuery(null, null);
        }
        public long ExecuteNonQuery(IDbConnection dbConnection)
        {
            return ExecuteNonQuery(dbConnection, null);
        }
        public long ExecuteNonQuery(IDbTransaction dbTransaction)
        {
            return ExecuteNonQuery(null, dbTransaction);
        }
        public long ExecuteNonQuery(IDbConnection paramConnection, IDbTransaction dbTransaction)
        {
            bool closeConnection = ((paramConnection == null) && (dbTransaction == null));

            if (paramConnection == null && dbTransaction != null)
                paramConnection = dbTransaction.Connection;

            IDbConnection dbConnection;
            if (paramConnection == null)
            {
                dbConnection = DataObjectResolver.GetIDbConnection(_dataBaseEngine);
                dbConnection.ConnectionString = _connectionString;
            }
            else
            {
                dbConnection = paramConnection;
            }

            IDbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dbCommand.CommandText = _sqlQuery;
                foreach (var param in _queryParameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                    dbCommand.Parameters.Add(dbParameter);
                }

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                long recordsAffected = dbCommand.ExecuteNonQuery();

                return recordsAffected;
            }
            catch (Exception exception)
            {
                throw exception;
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

        public object ExecuteScalar()
        {
            return ExecuteScalar(null, null);
        }
        public object ExecuteScalar(IDbConnection dbConnection)
        {
            return ExecuteScalar(dbConnection, null);
        }
        public object ExecuteScalar(IDbTransaction dbTransaction)
        {
            return ExecuteScalar(null, dbTransaction);
        }
        public object ExecuteScalar(IDbConnection paramConnection, IDbTransaction dbTransaction)
        {
            bool closeConnection = ((paramConnection == null) && (dbTransaction == null));

            if (paramConnection == null && dbTransaction != null)
                paramConnection = dbTransaction.Connection;

            IDbConnection dbConnection;
            if (paramConnection == null)
            {
                dbConnection = DataObjectResolver.GetIDbConnection(_dataBaseEngine);
                dbConnection.ConnectionString = _connectionString;
            }
            else
            {
                dbConnection = paramConnection;
            }

            IDbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dbCommand.CommandText = _sqlQuery;
                foreach (var param in _queryParameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                    dbCommand.Parameters.Add(dbParameter);
                }

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                return dbCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                throw exception;
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

        public IDataReader ExecuteReader()
        {
            return ExecuteReader(null, null);
        }
        public IDataReader ExecuteReader(IDbConnection dbConnection)
        {
            return ExecuteReader(dbConnection, null);
        }
        public IDataReader ExecuteReader(IDbTransaction dbTransaction)
        {
            return ExecuteReader(null, dbTransaction);
        }
        public IDataReader ExecuteReader(IDbConnection paramConnection, IDbTransaction dbTransaction)
        {
            bool closeConnection = ((paramConnection == null) && (dbTransaction == null));

            if (paramConnection == null && dbTransaction != null)
                paramConnection = dbTransaction.Connection;

            IDbConnection dbConnection;
            if (paramConnection == null)
            {
                dbConnection = DataObjectResolver.GetIDbConnection(_dataBaseEngine);
                dbConnection.ConnectionString = _connectionString;
            }
            else
            {
                dbConnection = paramConnection;
            }

            IDbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dbCommand.CommandText = _sqlQuery;
                foreach (var param in _queryParameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                    dbCommand.Parameters.Add(dbParameter);
                }

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                return dbCommand.ExecuteReader();
            }
            catch (Exception exception)
            {
                throw exception;
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
    }
}
