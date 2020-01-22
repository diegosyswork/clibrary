using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Syntax;
using SysWork.Data.GenericRepository.Mapper;

namespace SysWork.Data.Common.Utilities
{
    #region DOCUMENTATION Class
    /// <summary>
    /// Class to facilitate Querys.
    /// 
    /// This class implements the builder pattern to help execute structured queries. 
    /// Supports ExecuteNonQuery(), ExecuteScalar() and ExecuteReader() and multiple database engines.
    /// Helps to create INSERT and UPDATE Querys.
    /// 
    /// </summary>
    /// <remarks>
    /// <seealso cref="Common.EDataBaseEngine"/>..
    /// </remarks>
    /// <example>
    /// <code>
    /// 
    ///   var connectionString = "MyConnectrionString";
    ///   
    ///   var recordsAffected = new DbExecutor(connectionString)
    ///  .Query("UPDATE Products SET Cost = @pCost WHERE IdProduct = @pIdProduct AND IdCategory = @pIdCategory")
    ///  .AddParameter("@pCost",155.4)
    ///  .AddParameter("@pIdProduct",77978788)
    ///  .AddParameter("@pIdCategory",5)
    ///  .ExecuteNonQuery();
    ///   
    ///   var id = new DbExecutor(connectionString)
    ///  .InsertQuery("Products")
    ///  .AddFieldWithValue("IdProduct",77978788)
    ///  .AddFieldWithValue("Description",77978788)
    ///  .AddFieldWithValue("IdCategory",5)
    ///  .AddFieldWithValue("Cost",155.4)
    ///  .ExecuteScalar();
    ///  
    ///   new DbExecutor(connectionString)
    ///  .UpdateQuery("Products"," WHERE IdCategory = @pIdCategory AND active = 0")
    ///  .AddFieldWithValue("Cost", 0)
    ///  .AddFieldWithValue("Price", 0)
    ///  .AddParameter("IdCategory",5)
    ///  .ExecuteNonQuery();
    ///  
    ///   var reader = new DbExecutor(connectionString)
    ///  .Query("SELECT Products WHERE IdCategory = @pIdCategory")
    ///  .AddParameter("@pIdCategory",5)
    ///  .ExecuteReader();
    ///   while(reader.Read())
    ///   {
    ///       // do something.
    ///   }
    ///   
    ///   var resul = new DbExecutor(connectionString)
    ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
    ///  .AddParameter("@pIdCategory",5)
    ///  .ExecuteScalar();
    ///  
    ///   var productCount = DbUtil.ParseToLong(result); 
    /// 
    /// </code>
    /// </example>
    #endregion
    public class DbExecutor
    {
        private EDataBaseEngine _dataBaseEngine;
        private string _connectionString;
        private DbConnection _dbConnection = null;
        private DbTransaction _dbTransaction = null;

        private bool _isInsertQuery = false;
        private bool _isUpdateQuery = false;

        private const string _DbExecutorParameterPrefix = "@p_dbex_";

        private string _sqlQuery;
        private IDictionary<string, object> _queryParameters;
        private IDictionary<string, int> _queryParameterSize;
        private IDictionary<string, DbType> _queryParameterDbTye;

        private DbObjectProvider _dataObjectProvider;
        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Gets or sets the default command time out.
        /// </summary>
        /// <value>
        /// The default command time out.
        /// </value>
        public int DefaultCommandTimeOut { get; set; } = 30;

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
        public DbExecutor(string connectionString)
        {
            ConstructorResolver(null,null, connectionString, EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecutor"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        public DbExecutor(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            ConstructorResolver(null,null, connectionString, dataBaseEngine);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecutor"/> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        public DbExecutor(DbConnection dbConnection)
        {
            ConstructorResolver(dbConnection,null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecutor"/> class.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        public DbExecutor(DbTransaction dbTransaction)
        {
            ConstructorResolver(null, dbTransaction);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecutor" /> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        public DbExecutor(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            ConstructorResolver(dbConnection, null);
        }

        private void ConstructorResolver(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            EDataBaseEngine paramEDataBaseEngine;
            DbConnection paramConnection = dbConnection;

            if ((paramConnection == null) && (dbTransaction != null))
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
            _queryParameterSize = new Dictionary<string, int>();
            _queryParameterDbTye = new Dictionary<string, DbType>();
        }

        /// <summary>
        /// Set the SQL Query.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecutor Query(string commandText)
        {
            _sqlQuery = commandText;
            return this;
        }

        /// <summary>
        /// Create a Empty INSERT QUERY with the Template: INSERT INTO (FIELDS) VALUES (PARAMETERS).
        /// Use <see cref="AddFieldWithValue(string, object)"/>, to create the fields and parametrized values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <example>
        /// <code>
        ///    var id = new DbExecutor(connectionString)
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
        public DbExecutor InsertQuery(string tableName)
        {
            _isInsertQuery = true;

            var commandText = string.Format("INSERT INTO {0} ( /LIST_OF_FIELDS_NAMES/ ) VALUES ( /LIST_OF_PARAMETERS_NAMES/ ) {1}", tableName, _syntaxProvider.GetSubQueryGetIdentity());
            _sqlQuery = commandText;
            return this;
        }

        /// <summary>
        /// Create a Empty UPDATE QUERY with the Template: UPDATE TABLE SET .... WHERE.
        /// Use <see cref="AddFieldWithValue(string, object)"/>, to create the fields and parametrized values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <example>
        /// <code>
        ///   new DbExecutor(connectionString)
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
        public DbExecutor UpdateQuery(string tableName)
        {
            return UpdateQuery(tableName, null);
        }

        /// <summary>
        /// Create a Empty UPDATE QUERY with the Template: UPDATE TABLE SET .... WHERE.
        /// Use <see cref="AddFieldWithValue(string, object)"/>, to create the fields and parametrized values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <example>
        /// <code>
        ///   new DbExecutor(connectionString)
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
        public DbExecutor UpdateQuery(string tableName,string where)
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
        ///   new DbExecutor(connectionString)
        ///  .UpdateQuery("Products"," SET cost = 0 WHERE IdCategory = @pIdCategory AND active = 0")
        ///  .AddParameter("IdCategory",5)
        ///  .ExecuteNonQuery();
        ///  
        ///   var result = new DbExecutor(connectionString)
        ///  .Query("INSERT INTO products (ProductCode, Description) VALUES (@pProductCode,@pDescription)")
        ///  .AddParameter("@pProductCode","779778745581")
        ///  .AddParameter("@pDescription","MANAOS UVA")
        ///  .ExecuteScalar();
        /// </code> 
        /// </example>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecutor AddParameter(string name, object value)
        {
            if (!_queryParameters.ContainsKey(name))
                _queryParameters.Add(name, value);

            if (!_queryParameterSize.ContainsKey(name))
                _queryParameterSize.Add(name, 0);

            return this;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public DbExecutor AddParameter(string name, object value, int size)
        {
            if (!_queryParameters.ContainsKey(name))
                _queryParameters.Add(name, value);

            if (!_queryParameterSize.ContainsKey(name))
                _queryParameterSize.Add(name, size);

            return this;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <returns></returns>
        public DbExecutor AddParameter(string name, object value, DbType dbType)
        {
            if (!_queryParameters.ContainsKey(name))
                _queryParameters.Add(name, value);


            if (!_queryParameterDbTye.ContainsKey(name))
                _queryParameterDbTye.Add(name, dbType);

            return this;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public DbExecutor AddParameter(string name, object value, DbType dbType, int size)
        {
            if (!_queryParameters.ContainsKey(name))
                _queryParameters.Add(name, value);

            if (!_queryParameterSize.ContainsKey(name))
                _queryParameterSize.Add(name, size);

            if (!_queryParameterDbTye.ContainsKey(name))
                _queryParameterDbTye.Add(name, dbType);

            return this;
        }

        /// <summary>
        /// Adds multiples parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecutor AddParameters(IDictionary<string, object> parameters)
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
        ///   var id = new DbExecutor(connectionString)
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
        public DbExecutor AddFieldWithValue(string fieldName, object value)
        {
            string parameterName = _DbExecutorParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!_queryParameters.ContainsKey(parameterName))
                _queryParameters.Add(parameterName, value);

            if (!_queryParameterSize.ContainsKey(parameterName))
                _queryParameterSize.Add(parameterName, 0);

            return this;
        }

        /// <summary>
        /// Adds the field with value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public DbExecutor AddFieldWithValue(string fieldName, object value, int size)
        {
            string parameterName = _DbExecutorParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!_queryParameters.ContainsKey(parameterName))
                _queryParameters.Add(parameterName, value);

            if (!_queryParameterSize.ContainsKey(parameterName))
                _queryParameterSize.Add(parameterName, size);

            return this;
        }

        /// <summary>
        /// Adds the field with value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <returns></returns>
        public DbExecutor AddFieldWithValue(string fieldName, object value, DbType dbType)
        {
            string parameterName = _DbExecutorParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!_queryParameters.ContainsKey(parameterName))
                _queryParameters.Add(parameterName, value);

            if (!_queryParameterDbTye.ContainsKey(parameterName))
                _queryParameterDbTye.Add(parameterName, dbType);

            return this;
        }

        /// <summary>
        /// Adds the field with value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public DbExecutor AddFieldWithValue(string fieldName, object value, DbType dbType, int size)
        {
            string parameterName = _DbExecutorParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!_queryParameters.ContainsKey(parameterName))
                _queryParameters.Add(parameterName, value);

            if (!_queryParameterSize.ContainsKey(parameterName))
                _queryParameterSize.Add(parameterName, size);

            if (!_queryParameterDbTye.ContainsKey(parameterName))
                _queryParameterDbTye.Add(parameterName, dbType);

            return this;
        }

        #region DOCUMENTATION ExecuteNonQuery()
        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteNonQuery() method.
        /// </summary>
        /// <example>
        ///   <code>
        ///   
        ///   var recordsAffected = new DbExecutor(connectionString)
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
        ///   var recordsAffected = new DbExecutor(extIDbConnection)
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
        ///   var recordsAffected = new DbExecutor(dbTransaction)
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
        #endregion 
        public long ExecuteNonQuery()
        {
            return ExecuteNonQuery(_dbConnection, _dbTransaction);
        }

        #region DOCUMENTATION ExecuteNonQuery(int dbCommandTimeOut)
        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteNonQuery() method.
        /// </summary>
        /// <param name="dbCommandTimeOut">Timeout for this execution.</param>
        /// <example>
        ///   <code>
        ///   
        ///   var recordsAffected = new DbExecutor(connectionString)
        ///  .Query("UPDATE Products SET Cost = @pCost WHERE IdProduct = @pIdProduct AND IdCategory = @pIdCategory")
        ///  .AddParameter("@pCost",155.4)
        ///  .AddParameter("@pIdProduct",77978788)
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteNonQuery(10);
        ///  
        /// 
        ///   //Using an external connection
        ///   var extConnectionString = "MyConnectrionString";
        ///   var extIDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   extIDbConnection.Open();
        ///   
        ///   var recordsAffected = new DbExecutor(extIDbConnection)
        ///  .Query("UPDATE Products SET Cost = @pCost WHERE IdProduct = @pIdProduct AND IdCategory = @pIdCategory")
        ///  .AddParameter("@pCost",155.4)
        ///  .AddParameter("@pIdProduct",77978788)
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteNonQuery(10);
        ///  
        ///   // Using an external transaction.-
        ///   var extConnectionString = "MyConnectrionString";
        ///   var extDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   extDbConnection .Open();
        ///   
        ///   var dbTransaction = extDbConnection.BeginTransaction();
        ///   
        ///   var recordsAffected = new DbExecutor(dbTransaction)
        ///  .Query("UPDATE Products SET Cost = @pCost WHERE IdProduct = @pIdProduct AND IdCategory = @pIdCategory")
        ///  .AddParameter("@pCost",155.4)
        ///  .AddParameter("@pIdProduct",77978788)
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteNonQuery(10);
        ///  
        ///  IDbTransaction.Commit();
        ///  
        ///  </code>
        /// </example>
        /// 
        /// <returns>
        /// An long with the records affecteds by the query.
        /// </returns>
        #endregion 
        public long ExecuteNonQuery(int dbCommandTimeOut)
        {
            return ExecuteNonQuery(_dbConnection, _dbTransaction, dbCommandTimeOut);
        }

        #region DOCUMENTATION private ExecuteNonQuery(IDbConnection paramConnection, IDbTransaction dbTransaction, int dbCommandTimeOut = -1)
        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteNonQuery() method.
        /// </summary>
        /// <param name="paramConnection">The parameter connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="dbCommandTimeOut">CommandTimeOut form this execution</param>
        /// <returns>
        /// An long with the recordsAffecteds.
        /// </returns>
        #endregion
        private long ExecuteNonQuery(IDbConnection paramConnection, IDbTransaction dbTransaction, int dbCommandTimeOut = -1)
        {
            if (_isInsertQuery) NormalizeInsertQuery();
            if (_isUpdateQuery) NormalizeUpdateQuery();

            if (dbCommandTimeOut == -1) dbCommandTimeOut = DefaultCommandTimeOut;

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
                dbCommand.CommandTimeout = dbCommandTimeOut;

                foreach (var param in _queryParameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                    if (_queryParameterSize.TryGetValue(dbParameter.ParameterName, out int paramSize))
                        if (paramSize != 0)
                            dbParameter.Size = paramSize;

                    if (_queryParameterDbTye.TryGetValue(dbParameter.ParameterName, out DbType dbType))
                        dbParameter.DbType = dbType;

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

        #region DOCUMENTATION ExecuteScalar()
        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteScalar() method.
        /// </summary>
        /// <example>
        /// <code>
        /// 
        ///   // Using ConnectionString.
        ///   var id = new DbExecutor(connectionString)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar();
        ///   
        ///   var resul = new DbExecutor(connectionString)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar();
        ///  
        ///   // Using an External DbConnection.
        ///   var extDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   extDbConnection.Open();
        /// 
        ///   var id = new DbExecutor(extDbConnection)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar();
        ///   
        ///   var resul = new DbExecutor(extDbConnection)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar();
        ///   
        ///   // Using an external dbTransaction.
        ///   var extDbTransaction = extDbConnection.BeginTransaction();
        ///   
        ///   var id = new DbExecutor(extDbTransaction)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar();
        ///   
        ///   
        ///   var resul = new DbExecutor(extDbTransaction)
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
        #endregion
        public object ExecuteScalar()
        {
            return ExecuteScalar(_dbConnection, _dbTransaction);
        }


        #region DOCUMENTATION ExecuteScalar(int dbCommandTimeOut)
        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteScalar() method.
        /// </summary>
        /// <param name="dbCommandTimeOut">CommandTimeOut form this execution</param>
        /// <example>
        /// <code>
        /// 
        ///   // Using ConnectionString.
        ///   var id = new DbExecutor(connectionString)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar(5);
        ///   
        ///   var resul = new DbExecutor(connectionString)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar(5);
        ///  
        ///   // Using an External DbConnection.
        ///   var extDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   extDbConnection.Open();
        /// 
        ///   var id = new DbExecutor(extDbConnection)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar(5);
        ///   
        ///   var resul = new DbExecutor(extDbConnection)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar(5);
        ///   
        ///   // Using an external dbTransaction.
        ///   var extDbTransaction = extDbConnection.BeginTransaction();
        ///   
        ///   var id = new DbExecutor(extDbTransaction)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar();
        ///   
        ///   var resul = new DbExecutor(extDbTransaction)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar(5);
        /// 
        ///   extDbTransaction.Commit();
        /// 
        /// </code>
        /// </example>
        /// 
        /// <returns>
        /// An object that must then be converted to obtain the query result value.
        /// </returns>
        #endregion 
        public object ExecuteScalar(int dbCommandTimeOut)
        {
            return ExecuteScalar(_dbConnection, _dbTransaction, dbCommandTimeOut);
        }
        #region DOCUMENTATION private ExecuteScalarIDbConnection paramConnection, IDbTransaction dbTransaction, int dbCommandTimeOut = -1)
        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteScalar() method and use the IDbConnection and the IDbTransaction provided.
        /// </summary>
        /// <param name="paramConnection">The parameter connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="dbCommandTimeOut">CommandTimeOut for this execution</param>
        /// <example>
        /// <code>
        /// 
        ///   var connectionString = "MyConnectrionString";
        ///   var IDbConnection = DataManager.GetInstance().GetDbConnection();
        ///   IDbConnection.Open();
        ///   var IDbTransaction = IDbConnection.BeginTransaction();
        ///   
        ///   var id = new DbExecutor(connectionString)
        ///  .InsertQuery("Products")
        ///  .AddFieldWithValue("IdProduct",77978788)
        ///  .AddFieldWithValue("Description","MANAOS UVA")
        ///  .AddFieldWithValue("IdCategory",5)
        ///  .AddFieldWithValue("Cost",155.4)
        ///  .ExecuteScalar(IDbConnection, IDbTransaction);
        ///   
        ///   var resul = new DbExecutor(connectionString)
        ///  .Query("SELECT COUNT(*) as qty FROM Products WHERE IdCategory = @pIdCategory")
        ///  .AddParameter("@pIdCategory",5)
        ///  .ExecuteScalar(IDbConnection, IDbTransaction);
        /// </code> 
        /// </example>
        /// <returns>
        /// An object that must then be converted to obtain the query result value.
        /// </returns>
        #endregion
        private object ExecuteScalar(IDbConnection paramConnection, IDbTransaction dbTransaction, int dbCommandTimeOut = -1)
        {
            if (_isInsertQuery) NormalizeInsertQuery();
            if (_isUpdateQuery) NormalizeUpdateQuery();

            if (dbCommandTimeOut == -1) dbCommandTimeOut = DefaultCommandTimeOut;

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
                dbCommand.CommandTimeout = dbCommandTimeOut;

                foreach (var param in _queryParameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                    if (_queryParameterSize.TryGetValue(dbParameter.ParameterName, out int paramSize))
                        if (paramSize != 0)
                            dbParameter.Size = paramSize;

                    if (_queryParameterDbTye.TryGetValue(dbParameter.ParameterName, out DbType dbType))
                        dbParameter.DbType = dbType;

                    dbCommand.Parameters.Add(dbParameter);
                }

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                {
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();
                    if (_isInsertQuery)
                    {
                        dbCommand.ExecuteNonQuery();
                        dbCommand.CommandText = "Select @@Identity";
                        dbCommand.Parameters.Clear();
                    }
                }

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
        #region DOCUMENTATION ExecuteReader()
        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteReader() method.
        /// </summary>
        /// <example>
        /// <code>
        /// 
        ///   var connectionString = "MyConnectionString";
        ///   var reader = new DbExecutor(connectionString)
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
        ///   var reader = new DbExecutor(extDbConnection)
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
        ///   var reader = new DbExecutor(extTransaction)
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
        #endregion
        public IDataReader ExecuteReader()
        {
            return ExecuteReader(_dbConnection, _dbTransaction);
        }

        #region DOCUMENTATION ExecuteReader(int dbCommandTextTimeOut)
        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteReader(int dbCommandTextTimeOut) method.
        /// </summary>
        /// <param name="dbCommandTimeOut">CommandTimeOut for this execution.</param>
        /// <example>
        /// <code>
        /// 
        ///   var connectionString = "MyConnectionString";
        ///   var reader = new DbExecutor(connectionString)
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
        ///   var reader = new DbExecutor(extDbConnection)
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
        ///   var reader = new DbExecutor(extTransaction)
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
        #endregion
        public IDataReader ExecuteReader(int dbCommandTimeOut)
        {
            return ExecuteReader(_dbConnection, _dbTransaction, dbCommandTimeOut);
        }

        /// <summary>
        /// Run an IDbCommand with the SQLQuery using the ExecuteReader(int dbCommandTextTimeOut) method.
        /// </summary>
        /// <param name="paramConnection">The parameter connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="dbCommandTimeout">The DbCommand timeout for this execution.</param>
        /// <returns></returns>
        private IDataReader ExecuteReader(IDbConnection paramConnection, IDbTransaction dbTransaction, int dbCommandTimeout = -1)
        {
            bool closeConnection = ((paramConnection == null) && (dbTransaction == null));
            if (dbCommandTimeout == -1) dbCommandTimeout = DefaultCommandTimeOut;

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
                dbCommand.CommandTimeout = dbCommandTimeout;

                foreach (var param in _queryParameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                    if (_queryParameterSize.TryGetValue(dbParameter.ParameterName, out int paramSize))
                        if (paramSize != 0)
                            dbParameter.Size = paramSize;

                    if (_queryParameterDbTye.TryGetValue(dbParameter.ParameterName, out DbType dbType))
                        dbParameter.DbType = dbType;


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

        /// <summary>
        /// Gets a Typed List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetTypedList<T>()where T:class, new()
        {
            return new MapDataReaderToEntity().Map<T>(ExecuteReader(_dbConnection, _dbTransaction)).ToList();
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
                if (key.StartsWith(_DbExecutorParameterPrefix))
                    listFieldsAndParameters.Add(_syntaxProvider.GetSecureColumnName(_syntaxProvider.SecureParameterNameToField(key.Replace(_DbExecutorParameterPrefix, ""))) + " = " + key);
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
                if (key.StartsWith(_DbExecutorParameterPrefix))
                {
                    
                    listFields.Add(_syntaxProvider.GetSecureColumnName(_syntaxProvider.SecureParameterNameToField(key.Replace(_DbExecutorParameterPrefix, ""))));
                    listParametersNames.Add(key);
                }
            }

            _sqlQuery = _sqlQuery.Replace("/LIST_OF_FIELDS_NAMES/", string.Join(",", listFields));
            _sqlQuery = _sqlQuery.Replace("/LIST_OF_PARAMETERS_NAMES/", string.Join(",", listParametersNames));
        }
    }
}
