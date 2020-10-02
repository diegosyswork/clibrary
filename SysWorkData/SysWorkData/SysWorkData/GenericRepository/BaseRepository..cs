using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using SysWork.Data.Common.Attributes;
using SysWork.Data.Common.Attributes.Helpers;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.DbInfo;
using SysWork.Data.Common.Dictionaries;
using SysWork.Data.Common.Filters;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;
using SysWork.Data.Common.Mapper;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository.Interfaces;

#pragma warning disable 1587
/// <summary>
/// 
/// </summary>
#pragma warning restore 1587
namespace SysWork.Data.GenericRepository
{

    #region DOCUMENTATION Class
    /// <summary>
    /// Abstract class to be partially implemented. It allows CRUD operations to be performed on database 
    /// entities represented by classes (classes must implement DbTableAttribute and DbColumnAttribute attributes). 
    /// Implementing this class allows: 
    /// Add,
    /// AddRange,
    /// Update,
    /// UpdateRange,
    /// DeleteById,
    /// DeleteAll,
    /// DeleteByLambdaExpressionFilter,
    /// DeleteByGenericWhereFilter,
    /// GetById,
    /// GetByLambdaExpressionFilter,
    /// GetByGenericWhereFilter,
    /// GetAll,
    /// GetListByLambdaExpressionFilter,
    /// GetDataTableByLambdaExpressionFilter,
    /// GetListByGenericWhereFilter,
    /// GetDataTableByGenericWhereFilter,
    /// Find
    /// <seealso cref="SysWork.Data.Common.Attributes"/>
    /// </summary>
    /// <remarks>
    /// This class is multi database engine, see the supported database engines <see cref="Common.ValueObjects.EDatabaseEngine"/>. 
    /// 
    /// All its methods, in case of not specifying a connection, create a new one and at the end they close it.
    /// 
    /// In case a transaction is specified, the active connection of the transaction is used.
    /// 
    /// In case a transaction and a connection are specified, the ones provided will be used.
    /// </remarks>
    #endregion
    public abstract partial class BaseRepository<TEntity>:IRepository<TEntity> where TEntity : class, new()
    {

        private string _connectionString;
        /// <summary>
        /// Gets the active ConnectionString.
        /// </summary>
        public string ConnectionString { get { return _connectionString; } }

        private EDatabaseEngine _databaseEngine;
        /// <summary>
        /// Gets the Database Engine.
        /// </summary>
        /// <seealso cref=" SysWork.Data.Common.ValueObjects.EDatabaseEngine"/>
        public EDatabaseEngine DatabaseEngine { get { return _databaseEngine; } }

        private Hashtable _columnListWithDbInfo = new Hashtable();

        private DbObjectProvider _dbObjectProvider;
        /// <summary>
        /// Gets the data object provider.
        /// </summary>
        /// <value>The DbObjectProvider used in this class.</value>
        protected DbObjectProvider DbObjectProvider { get { return _dbObjectProvider; } }

        private SyntaxProvider _syntaxProvider;
        /// <summary>
        /// Get the name of the database table to represent.
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Get the propertyInfo of the columns.
        /// </summary>
        public IList<PropertyInfo> EntityProperties { get; private set; }

        /// <summary>
        /// Get a list of the columns to perform a INSERT sentence on the represented table, separated by commas. The Identity columns are excluded.
        /// </summary>
        public string ColumnsForInsert { get; private set; }

        /// <summary>
        /// Get a list of the columns to perform a SELECT sentence on the represented table, separated by commas.
        /// </summary>
        public string ColumnsForSelect { get; private set; }

        private MapDataReaderToEntity _mapper;

        private int _defaultCommandTimeout = 30;
        /// <summary>
        /// Gets or sets the default CommandTimeOut.
        /// </summary>
        /// <value>
        /// The default CommandTimeOut.
        /// </value>
        protected int DefaultCommandTimeOut {get { return _defaultCommandTimeout; } set {_defaultCommandTimeout = value;}}

        /// <summary>
        /// Initializes a new instance class. Using MSSqlServer as DatabaseEngine.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public BaseRepository(string connectionString)
        {
            BaseRepositoryConstructorResolver(connectionString, EDatabaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Initializes a new instance class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="databaseEngine">The data base engine.</param>
        public BaseRepository(string connectionString, EDatabaseEngine databaseEngine)
        {
            BaseRepositoryConstructorResolver(connectionString, databaseEngine);
        }

        private void BaseRepositoryConstructorResolver(string connectionString, EDatabaseEngine databaseEngine)
        {
            _connectionString = connectionString;
            _databaseEngine = databaseEngine;
            _syntaxProvider = new SyntaxProvider(_databaseEngine);
            _dbObjectProvider = new DbObjectProvider(_databaseEngine);

            _mapper = new MapDataReaderToEntity();
            _mapper.UseTypeCache = false;

            TEntity entity = new TEntity();
            EntityProperties = DbColumnHelper.GetProperties(entity);

            TableName = GetTableNameFromEntity(entity.GetType());

            if ((EntityProperties == null) || (EntityProperties.Count == 0))
                throw new Exception(string.Format("The Entity {0}, has not linked attibutes to table: {1}, Use [DbColumn] attribute to link properties to the table.", entity.GetType().Name, TableName));

            GetDbColumnsAndAtributes();
        }

        private string GetTableNameFromEntity(Type type)
        {
            var DbTable = type.GetCustomAttributes(false).OfType<DbTableAttribute>().FirstOrDefault();
            if (DbTable != null)
                return DbTable.Name ?? type.Name;
            else
                throw new Exception(string.Format("The Entity {0}, has not linked to any table, Use [DbTable] attribute to link it to a table.", type.Name));

        }
        
        /// <summary>
        /// Gets a new instance of the GenericFilterQuery with the ColumnsForSelect and TableName setted.
        /// </summary>
        /// <returns></returns>
        public GenericWhereFilter GetGenericWhereFilter()
        {
            GenericWhereFilter whereFilter = new GenericWhereFilter(_databaseEngine);
            whereFilter.SetColumnsForSelect<TEntity>();
            whereFilter.SetTableOrViewName<TEntity>();

            return whereFilter;
        }

        /// <summary>
        /// Gets IDBConnection instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// A closed IDBConnection instantiated according to the database engine
        /// </returns>
        protected IDbConnection BaseIDbConnection()
        {
            return _dbObjectProvider.GetIDbConnection(_connectionString);
        }

        /// <summary>
        /// Gets DBConnection instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// A closed DBConnection instantiated according to the database engine.
        /// </returns>
        protected DbConnection BaseDbConnection()
        {
            return _dbObjectProvider.GetDbConnection(_connectionString);
        }

        /// <summary>
        /// A new instance of DbExecute instantiated according to the database engine
        /// </summary>
        /// <returns>
        /// A new instance of DbExecute instantiated according to the database engine.
        /// </returns>
        protected DbExecutor BaseDbExecutor()
        {
            return new DbExecutor(_connectionString, _databaseEngine);
        }
        /// <summary>
        /// A new instance of DbExecute instantiated according to the database engine. Using a DbConnection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        protected DbExecutor BaseDbExecutor(DbConnection dbConnection)
        {
            return new DbExecutor(dbConnection);
        }

        /// <summary>
        /// A new instance of DbExecute instantiated according to the database engine. Using a DbTransaction.
        /// </summary>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        protected DbExecutor BaseDbExecutor(DbTransaction dbTransaction)
        {
            return new DbExecutor(dbTransaction);
        }

        /// <summary>
        /// Return new instance of SqlLam.
        /// </summary>
        /// <returns></returns>
        protected SqlLam<TEntity> BaseSqlLam()
        {
            SetSqlLamAdapter();
            return new SqlLam<TEntity>();
        }

        /// <summary>
        /// Creates an query parameters list.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, object> CreateQueryParametersList()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            return parameters;
        }

        private void GetDbColumnsAndAtributes()
        {
            TEntity entity = new TEntity();

            StringBuilder sbColumnsInsert = new StringBuilder();
            StringBuilder sbColumnsSelect = new StringBuilder();

            using (DbConnection conn = BaseDbConnection())
            {
                conn.Open();
                foreach (PropertyInfo i in EntityProperties)
                {
                    var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                    string columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                    if (!customAttribute.IsIdentity)
                        sbColumnsInsert.Append(string.Format("{0},", columnName));

                    sbColumnsSelect.Append(string.Format("{0},", columnName));

                    string schemaColumnName = _syntaxProvider.RemoveStartersAndEndersColumnName(customAttribute.ColumnName ?? i.Name);
                    _columnListWithDbInfo.Add(i.Name, GetColumnDbInfo(schemaColumnName, conn));
                }
                conn.Close();
            }

            if (sbColumnsInsert.Length > 0)
                sbColumnsInsert.Remove(sbColumnsInsert.Length - 1, 1);

            if (sbColumnsSelect.Length > 0)
                sbColumnsSelect.Remove(sbColumnsSelect.Length - 1, 1);

            ColumnsForInsert = sbColumnsInsert.ToString();
            ColumnsForSelect = sbColumnsSelect.ToString();
        }

        private DbColumnInfo GetColumnDbInfo(string columnName, DbConnection dbConnection)
        {
            DbColumnInfo columnData = new DbColumnInfo();

            DataTable columnProperty;
            if (_databaseEngine == EDatabaseEngine.OleDb || _databaseEngine == EDatabaseEngine.MySql)
                columnProperty = dbConnection.GetSchema("Columns", new[] { null, null, TableName, columnName });
            else
                columnProperty = dbConnection.GetSchema("Columns", new[] { dbConnection.Database, null, TableName, columnName });

            DataRow dataRow;
            try
            {
                dataRow = columnProperty.Rows[0];
            }
            catch (Exception)
            {
                throw new IndexOutOfRangeException(string.Format("The column {0} in the table {1} not exists",columnName,TableName));
            }

            columnData.MaxLenght = null;
            if (Int32.TryParse(dataRow["CHARACTER_MAXIMUM_LENGTH"].ToString(), out Int32 maxLength))
                columnData.MaxLenght = maxLength;

            var dataType = dataRow["DATA_TYPE"];

            if (_databaseEngine == EDatabaseEngine.OleDb)
            {
                if (DbTypeDictionary.DbColumnTypeToDbTypeEnum.TryGetValue(((OleDbType)dataType).ToString(), out DbType dBTypeValue))
                    columnData.DbType = dBTypeValue;
                else
                    throw new ArgumentOutOfRangeException("The datatype is not recognited. See the DbColumnTypeToDbTypeEnum Dictionary");
            }
            else
            {
                if (DbTypeDictionary.DbColumnTypeToDbTypeEnum.TryGetValue(dataType.ToString(), out DbType dBTypeValue))
                    columnData.DbType = dBTypeValue;
                else
                    throw new ArgumentOutOfRangeException("The datatype is not recognited. See the DbColumnTypeToDbTypeEnum Dictionary");
            }

            return columnData;
        }

        /// <summary>
        /// Gets IDbDataParameter instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
        protected IDbDataParameter GetIDbDataParameter()
        {
            return _dbObjectProvider.GetIDbDataParameter();
        }

        /// <summary>
        /// Creates an IDbDataParameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
        protected IDbDataParameter CreateIDbDataParameter(string parameterName, object value, Int32? size = null)
        {
            IDbDataParameter dataParameter = GetIDbDataParameter();

            dataParameter.ParameterName = parameterName;
            dataParameter.Value = value ?? (Object)DBNull.Value;

            if (size != null)
                dataParameter.Size = (int)size;

            return dataParameter;
        }

        /// <summary>
        /// Creates an IDbDataParameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="DbType">Database Tyoe.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
        protected IDbDataParameter CreateIDbDataParameter(string parameterName, DbType DbType, object value, Int32? size = null)
        {
            IDbDataParameter dataParameter = GetIDbDataParameter();

            dataParameter.ParameterName = parameterName;
            dataParameter.DbType = DbType;
            dataParameter.Value = value ?? (Object)DBNull.Value;

            if (size != null && size != 0)
                dataParameter.Size = (int)size;

            return dataParameter;
        }

        private void SetSqlLamAdapter()
        {
            if (_databaseEngine == EDatabaseEngine.MSSqlServer)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SqlServer2012);
            else if (_databaseEngine == EDatabaseEngine.OleDb)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SqlServer2012);
            else if (_databaseEngine == EDatabaseEngine.MySql)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.MySql);
            else if (_databaseEngine == EDatabaseEngine.SqLite)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SQLite);
            else
                throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method");
        }

    }
}
