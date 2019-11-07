using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Extensions.OleDbCommandExtensions;
using SysWork.Data.Syntax;

namespace SysWork.Data.Common.DbConnectionUtilities
{
    /// <summary>
    /// Class to facilitate Querys.
    /// </summary>
    /// <remarks>
    /// 
    /// This class implements the builder pattern to help execute structured queries. 
    /// Supports ExecuteNonQuery(), ExecuteScalar() and ExecuteReader() and multiple database engines.
    /// Helps to create INSERT and UPDATE Querys.
    /// 
    /// <seealso cref="Common.EDataBaseEngine"/>..
    /// </remarks>
    /// <example>
    /// <code>
    /// 
    ///   var connectionString = "MyConnectrionString";
    ///   
    ///   var recordsAffected = new DbExecute(connectionString)
    ///  .Query("UPDATE Products SET Cost = @pCost WHERE IdProduct = @pIdProduct AND IdCategory = @pIdCategory")
    ///  .AddParameter("@pCost",155.4)
    ///  .AddParameter("@pIdProduct",77978788)
    ///  .AddParameter("@pIdCategory",5)
    ///  .ExecuteNonQuery();
    ///   
    ///   var id = new DbExecute(connectionString)
    ///  .InsertQuery("Products")
    ///  .AddFieldWithValue("IdProduct",77978788)
    ///  .AddFieldWithValue("Description",77978788)
    ///  .AddFieldWithValue("IdCategory",5)
    ///  .AddFieldWithValue("Cost",155.4)
    ///  .ExecuteScalar();
    ///  
    ///   new DbExecute(connectionString)
    ///  .UpdateQuery("Products"," WHERE IdCategory = @pIdCategory AND active = 0")
    ///  .AddFieldWithValue("Cost", 0)
    ///  .AddFieldWithValue("Price", 0)
    ///  .AddParameter("IdCategory",5)
    ///  .ExecuteNonQuery();
    ///  
    ///   var reader = new DbExecute(connectionString)
    ///  .Query("SELECT Products WHERE IdCategory = @pIdCategory")
    ///  .AddParameter("@pIdCategory",5)
    ///  .ExecuteReader();
    ///   while(reader.Read())
    ///   {
    ///       // do something.
    ///   }
    ///   
    ///   var resul = new DbExecute(connectionString)
    ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
    ///  .AddParameter("@pIdCategory",5)
    ///  .ExecuteScalar();
    ///  
    ///   var productCount = DbUtil.ParseToLong(result); 
    /// 
    /// </code>
    /// </example>
    public class DbExecute
    {
        private EDataBaseEngine _dataBaseEngine;
        private string _connectionString;
        private DbConnection _dbConnection = null;
        private DbTransaction _dbTransaction = null;

        private bool _isInsertQuery = false;
        private bool _isUpdateQuery = false;

        private const string _dbExecuteParameterPrefix = "@p_dbex_";

        private string _sqlQuery;
        private IDictionary<string, object> _queryParameters;

        private DbObjectProvider _dataObjectProvider;
        private SyntaxProvider _syntaxProvider;
        /// <summary>
        /// Gets the SQL query.
        /// </summary>
        /// <value>
        /// The SQL query.
        /// </value>
        public string SqlQuery{ get { return _sqlQuery; } private set { }  }

        /// <summary>
        /// Initializes a new instance of the class. The databaseEngine is MSSqlServer.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DbExecute(string connectionString)
        {
            ConstructorResolver(null,null, connectionString, EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecute"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        public DbExecute(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            ConstructorResolver(null,null, connectionString, dataBaseEngine);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecute"/> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        public DbExecute(DbConnection dbConnection)
        {
            ConstructorResolver(dbConnection,null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecute"/> class.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        public DbExecute(DbTransaction dbTransaction)
        {
            ConstructorResolver(null, dbTransaction);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecute" /> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        public DbExecute(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            ConstructorResolver(dbConnection, null);
        }

        private void ConstructorResolver(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            EDataBaseEngine paramEDataBaseEngine;
            DbConnection paramConnection = dbConnection;

            if (paramConnection == null)
                if (dbTransaction != null)
                    paramConnection = dbTransaction.Connection;

            paramEDataBaseEngine = StaticDbObjectProvider.GetDataBaseEngineFromDbConnection(paramConnection);

            ConstructorResolver(paramConnection, dbTransaction, null, paramEDataBaseEngine);
        }
        private void ConstructorResolver(DbConnection dbConnection,DbTransaction dbTransaction, string connectionString, EDataBaseEngine dataBaseEngine)
        {
            _dbConnection = dbConnection;
            _connectionString = connectionString;
            _dbTransaction = dbTransaction;
            _dataBaseEngine = dataBaseEngine;
            _dataObjectProvider = new DbObjectProvider(dataBaseEngine);
            _syntaxProvider = new SyntaxProvider(dataBaseEngine);
            _queryParameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Set the SQL Query.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecute Query(string commandText)
        {
            _sqlQuery = commandText;
            return this;
        }

        /// <summary>
        /// Create a Empty INSERT QUERY with the Template: INSERT INTO (FIELDS) VALUES (PARAMETERS).
        /// Use <see cref="AddFieldWithValue"/>, to create the fields and parametrized values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <example>
        /// <code>
        ///    var id = new DbExecute(connectionString)
        ///   .InsertQuery("Products")
        ///   .AddFieldWithValue("Cost",155.4)
        ///   .AddFieldWithValue("IdProduct",77978788)
        ///   .AddFieldWithValue("IdCategory",5)
        ///   .ExecuteScalar();
        /// </code>
        /// </example>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecute InsertQuery(string tableName)
        {
            _isInsertQuery = true;

            var commandText = string.Format("INSERT INTO {0} ( /LIST_OF_FIELDS_NAMES/ ) VALUES ( /LIST_OF_PARAMETERS_NAMES/ )", tableName);
            _sqlQuery = commandText;
            return this;
        }

        /// <summary>
        /// Create a Empty UPDATE QUERY with the Template: UPDATE TABLE SET .... WHERE.
        /// Use <see cref="AddFieldWithValue"/>, to create the fields and parametrized values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <example>
        /// <code>
        ///   new DbExecute(connectionString)
        ///  .UpdateQuery("Products"," WHERE IdCategory = @pIdCategory AND active = 0")
        ///  .AddFieldWithValue("Cost", 0)
        ///  .AddFieldWithValue("Price", 0)
        ///  .AddParameter("IdCategory",5)
        ///  .ExecuteNonQuery();
        /// </code>
        /// </example>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecute UpdateQuery(string tableName)
        {
            return UpdateQuery(tableName, null);
        }

        /// <summary>
        /// Create a Empty UPDATE QUERY with the Template: UPDATE TABLE SET .... WHERE.
        /// Use <see cref="AddFieldWithValue"/>, to create the fields and parametrized values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <example>
        /// <code>
        ///   new DbExecute(connectionString)
        ///  .UpdateQuery("Products"," WHERE IdCategory = @pIdCategory AND active = 0")
        ///  .AddFieldWithValue("Cost", 0)
        ///  .AddFieldWithValue("Price", 0)
        ///  .AddParameter("IdCategory",5)
        ///  .ExecuteNonQuery();
        /// </code> 
        /// </example>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecute UpdateQuery(string tableName,string where)
        {
            _isUpdateQuery = true;

            if ((!string.IsNullOrEmpty(where)) && (!string.IsNullOrWhiteSpace(where)))
            {
                where = where.Trim();
                if (!where.StartsWith("WHERE"))
                    where = " WHERE " + where;
            }
            else
            {
                where = "";
            }

            var commandText = string.Format("UPDATE {0} SET /LIST_OF_FIELDS_AND_VALUES/ {1}", tableName,where);

            _sqlQuery = commandText;
            return this;
        }

        /// <summary>
        /// Adds and parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        ///   new DbExecute(connectionString)
        ///  .UpdateQuery("Products"," SET cost = 0 WHERE IdCategory = @pIdCategory AND active = 0")
        ///  .AddParameter("IdCategory",5)
        ///  .ExecuteNonQuery();
        ///  
        ///   var result = new DbExecute(connectionString)
        ///  .Query("INSERT INTO products (ProductCode, Description) VALUES (@pProductCode,@pDescription)")
        ///  .AddParameter("@pProductCode","779778745581")
        ///  .AddParameter("@pDescription","MANAOS UVA")
        ///  .ExecuteScalar();
        /// </code> 
        /// </example>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecute AddParameter(string name, object value)
        {
            if (!_queryParameters.ContainsKey(name))
                _queryParameters.Add(name, value);

            return this;
        }

        /// <summary>
        /// Adds multiples parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecute AddParameters(IDictionary<string, object> parameters)
        {
            foreach (var key in parameters.Keys)
                _queryParameters.Add(key, parameters[key]);

            return this;
        }

        /// <summary>
        /// Add an Field with the value, automatically, creates a parameter with the same name and "@p_dbex_" prefix.
        /// Use this only for InsertQuery and UpdateQuery.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <example>
        /// <code>
        ///   var id = new DbExecute(connectionString)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description",77978788)
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar();
        /// </code> 
        /// </example>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecute AddFieldWithValue(string fieldName, object value)
        {
            string parameterName = _dbExecuteParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!_queryParameters.ContainsKey(parameterName))
                _queryParameters.Add(parameterName, value);

            return this;
        }


        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteNonQuery() method.
        /// </summary>
        /// <example>
        ///   <code>
        ///   
        ///   var recordsAffected = new DbExecute(connectionString)
        ///  .Query("UPDATE Products SET Cost = @pCost WHERE IdProduct = @pIdProduct AND IdCategory = @pIdCategory")
        ///  .AddParameter("@pCost",155.4)
        ///  .AddParameter("@pIdProduct",77978788)
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteNonQuery();
        ///  
        /// 
        ///   //Using an external connection
        ///   var extConnectionString = "MyConnectrionString";
        ///   var extIDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   extIDbConnection.Open();
        ///   
        ///   var recordsAffected = new DbExecute(extIDbConnection)
        ///  .Query("UPDATE Products SET Cost = @pCost WHERE IdProduct = @pIdProduct AND IdCategory = @pIdCategory")
        ///  .AddParameter("@pCost",155.4)
        ///  .AddParameter("@pIdProduct",77978788)
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteNonQuery();
        ///  
        ///   // Using an external transaction.-
        ///   var extConnectionString = "MyConnectrionString";
        ///   var extDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   extDbConnection .Open();
        ///   
        ///   var dbTransaction = extDbConnection.BeginTransaction();
        ///   
        ///   var recordsAffected = new DbExecute(dbTransaction)
        ///  .Query("UPDATE Products SET Cost = @pCost WHERE IdProduct = @pIdProduct AND IdCategory = @pIdCategory")
        ///  .AddParameter("@pCost",155.4)
        ///  .AddParameter("@pIdProduct",77978788)
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteNonQuery();
        ///  
        ///  IDbTransaction.Commit();
        ///  
        ///  </code>
        /// </example>
        /// 
        /// <returns>
        /// An long with the records affecteds by the query.
        /// </returns>
        public long ExecuteNonQuery()
        {
            return ExecuteNonQuery(_dbConnection , _dbTransaction);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="paramConnection">The parameter connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// 
        /// <returns>
        /// An long with the recordsAffecteds.
        /// </returns>
        private long ExecuteNonQuery(IDbConnection paramConnection, IDbTransaction dbTransaction)
        {
            if (_isInsertQuery) NormalizeInsertQuery();
            if (_isUpdateQuery) NormalizeUpdateQuery();

            bool closeConnection = ((paramConnection == null) && (dbTransaction == null));

            if (paramConnection == null && dbTransaction != null)
                paramConnection = dbTransaction.Connection;

            IDbConnection dbConnection;
            if (paramConnection == null)
            {
                dbConnection = _dataObjectProvider.GetIDbConnection();
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

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

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

        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteScalar() method.
        /// </summary>
        /// <example>
        /// <code>
        /// 
        ///   // Using ConnectionString.
        ///   var id = new DbExecute(connectionString)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar();
        ///   
        ///   var resul = new DbExecute(connectionString)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar();
        ///  
        ///   // Using an External DbConnection.
        ///   var extDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   extDbConnection.Open();
        /// 
        ///   var id = new DbExecute(extDbConnection)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar();
        ///   
        ///   var resul = new DbExecute(extDbConnection)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar();
        ///   
        ///   // Using an external dbTransaction.
        ///   var extDbTransaction = extDbConnection.BeginTransaction();
        ///   
        ///   var id = new DbExecute(extDbTransaction)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar();
        ///   
        ///   
        ///   var resul = new DbExecute(extDbTransaction)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar();
        /// 
        ///   extDbTransaction.Commit();
        /// 
        /// </code>
        /// </example>
        /// 
        /// <returns>
        /// An object that must then be converted to obtain the query result value.
        /// </returns>
        public object ExecuteScalar()
        {
            return ExecuteScalar(_dbConnection, _dbTransaction);
        }

        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteScalar() method and use the IDbConnection and the IDbTransaction provided.
        /// </summary>
        /// <param name="paramConnection">The parameter connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <example>
        /// <code>
        /// 
        ///   var connectionString = "MyConnectrionString";
        ///   var IDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   IDbConnection.Open();
        ///   var IDbTransaction = IDbConnection.BeginTransaction();
        ///   
        ///   var id = new DbExecute(connectionString)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar(IDbConnection, IDbTransaction);
        ///   
        ///   var resul = new DbExecute(connectionString)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar(IDbConnection, IDbTransaction);
        /// </code> 
        /// </example>
        /// <returns>
        /// An object that must then be converted to obtain the query result value.
        /// </returns>
        private object ExecuteScalar(IDbConnection paramConnection, IDbTransaction dbTransaction)
        {
            if (_isInsertQuery)
                NormalizeInsertQuery();

            if (_isUpdateQuery)
                NormalizeUpdateQuery();

            bool closeConnection = ((paramConnection == null) && (dbTransaction == null));

            if (paramConnection == null && dbTransaction != null)
                paramConnection = dbTransaction.Connection;

            IDbConnection dbConnection;
            if (paramConnection == null)
            {
                dbConnection = _dataObjectProvider.GetIDbConnection();
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

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;
                
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

        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteReader() method.
        /// </summary>
        /// <example>
        /// <code>
        /// 
        ///   var connectionString = "MyConnectionString";
        ///   var reader = new DbExecute(connectionString)
        ///  .Query("SELECT Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteReader(IDbConnection);
        ///  
        ///   while(reader.Read())
        ///   {
        ///       // do something.
        ///   }
        ///   
        ///   //Using an external DbConnection
        ///   var extDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   extDbConnection.Open();
        ///   
        ///   var reader = new DbExecute(extDbConnection)
        ///  .Query("SELECT Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteReader();
        ///  
        ///   while(reader.Read())
        ///   {
        ///       // do something.
        ///   }
        ///   
        ///   // Using an External transaction.
        ///   var extTransaction = extDbConnection.BeginTransaction();
        ///   var reader = new DbExecute(extTransaction)
        ///  .Query("SELECT Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteReader(IDbTransaction);
        ///  
        ///   while(reader.Read())
        ///   {
        ///       // do something.
        ///   }
        ///   
        ///   extTransaction.Commit();
        ///   
        /// </code>
        /// </example>
        /// <returns>
        /// An IDataReader
        /// </returns>
        public IDataReader ExecuteReader()
        {
            return ExecuteReader(_dbConnection , _dbTransaction);
        }

        private IDataReader ExecuteReader(IDbConnection paramConnection, IDbTransaction dbTransaction)
        {
            bool closeConnection = ((paramConnection == null) && (dbTransaction == null));

            if (paramConnection == null && dbTransaction != null)
                paramConnection = dbTransaction.Connection;

            IDbConnection dbConnection;
            if (paramConnection == null)
            {
                dbConnection = _dataObjectProvider.GetIDbConnection();
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

                if (dbTransaction != null)
                    dbCommand.Transaction = dbTransaction;

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

                return dbCommand.ExecuteReader(closeConnection ? CommandBehavior.CloseConnection : CommandBehavior.Default);
            }
            catch (Exception exception)
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }

                throw exception;
            }
        }

        private string FirstLetterCapital(string str)
        {
            return Char.ToUpper(str[0]) + str.Remove(0, 1);
        }

        private void NormalizeUpdateQuery()
        {
            var list = _queryParameters.Keys.ToList();
            list.Sort();

            var listFieldsAndParameters = new List<string>();

            foreach (var key in list)
            {
                if (key.StartsWith(_dbExecuteParameterPrefix))
                    listFieldsAndParameters.Add(_syntaxProvider.GetSecureColumnName(_syntaxProvider.SecureParameterNameToField(key.Replace(_dbExecuteParameterPrefix, ""))) + " = " + key);
            }

            _sqlQuery = _sqlQuery.Replace("/LIST_OF_FIELDS_AND_VALUES/", string.Join(",", listFieldsAndParameters));
        }

        private void NormalizeInsertQuery()
        {
            var list = _queryParameters.Keys.ToList();
            list.Sort();

            var listFields = new List<string>();
            var listParametersNames = new List<string>();

            foreach (var key in list)
            {
                if (key.StartsWith(_dbExecuteParameterPrefix))
                {
                    
                    listFields.Add(_syntaxProvider.GetSecureColumnName(_syntaxProvider.SecureParameterNameToField(key.Replace(_dbExecuteParameterPrefix, ""))));
                    listParametersNames.Add(key);
                }
            }

            _sqlQuery = _sqlQuery.Replace("/LIST_OF_FIELDS_NAMES/", string.Join(",", listFields));
            _sqlQuery = _sqlQuery.Replace("/LIST_OF_PARAMETERS_NAMES/", string.Join(",", listParametersNames));
        }
    }
}
