using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.Mapper;
using SysWork.Data.Common.Attributes;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;
using SysWork.Data.Common.Filters;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;

namespace SysWork.Data.GenericViewManager
{
    /// <summary>
    /// Generic Class to manage views
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract partial class BaseViewManager<TEntity> where TEntity : class, new()
    {
        private string _connectionString;
        /// <summary>
        /// Gets the active ConnectionString.
        /// </summary>
        public string ConnectionString { get { return _connectionString; } private set { _connectionString = value; } }

        private EDataBaseEngine _dataBaseEngine;
        /// <summary>
        /// Gets the Database Engine.
        /// </summary>
        /// <seealso cref=" SysWork.Data.Common.EDataBaseEngine"/>
        public EDataBaseEngine DataBaseEngine { get { return _dataBaseEngine; } private set { _dataBaseEngine = value; } }

        /// <summary>
        /// Gets the data object provider.
        /// </summary>
        /// <value>The DbObjectProvider used in this class.</value>
        protected DbObjectProvider DataObjectProvider { get; private set; }

        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Get the name of the database table to represent.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Get the propertyInfo of the columns.
        /// </summary>
        public IList<PropertyInfo> ListObjectPropertyInfo { get; private set; }

        /// <summary>
        /// Get a list of the columns to perform a SELECT sentence on the represented table, separated by commas.
        /// </summary>
        public string ColumnsForSelect { get; private set; }

        private int _defaultCommandTimeout = 30;
        /// <summary>
        /// Gets or sets the default CommandTimeOut.
        /// </summary>
        /// <value>
        /// The default CommandTimeOut.
        /// </value>
        protected int DefaultCommandTimeOut { get { return _defaultCommandTimeout; } set { _defaultCommandTimeout = value; } }
        
        /// <summary>
        /// Initializes a new instance class. Using MSSqlServer as DataBaseEngine.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public BaseViewManager(string connectionString)
        {
            BaseGenericViewManagerConstructorResolver(connectionString, EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Initializes a new instance class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        public BaseViewManager(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            BaseGenericViewManagerConstructorResolver(connectionString, dataBaseEngine);
        }
        private void BaseGenericViewManagerConstructorResolver(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            _connectionString = connectionString;
            _dataBaseEngine = dataBaseEngine;
            DataObjectProvider = new DbObjectProvider(_dataBaseEngine);
            _syntaxProvider = new SyntaxProvider(_dataBaseEngine);

            TEntity entity = new TEntity();
            ListObjectPropertyInfo = GetPropertyInfoList(entity);

            ViewName = GetViewNameFromEntity(entity.GetType());

            if ((ListObjectPropertyInfo == null) || (ListObjectPropertyInfo.Count == 0))
            {
                throw new Exception(string.Format("The Entity {0}, has not linked attibutes to table: {1}, Use [DbColumn] attribute to link properties to the table.", entity.GetType().Name, ViewName));
            }

            GetDbColumns();
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        /// <returns></returns>
        protected DbConnection GetDbConnection()
        {
            return DataObjectProvider.GetDbConnection(_connectionString);
        }

        /// <summary>
        /// Gets IDbDataParameter instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
        protected IDbDataParameter GetIDbDataParameter()
        {
            return DataObjectProvider.GetIDbDataParameter();
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
        /// Gets IDBConnection instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// A closed IDBConnection instantiated according to the database engine
        /// </returns>
        protected IDbConnection GetIDbConnection()
        {
            return DataObjectProvider.GetIDbConnection(_connectionString);
        }

        /// <summary>
        /// Gets the list by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter)
        {
            return GetListByGenericWhereFilter(whereFilter, null, null);
        }

        /// <summary>
        /// Gets the list by generic generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection)
        {
            return GetListByGenericWhereFilter(whereFilter, paramDbConnection,null);
        }

        /// <summary>
        /// Gets the list by generic generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction)
        {
            return GetListByGenericWhereFilter(whereFilter, null, paramDbTransaction);
        }

        /// <summary>
        /// Gets the list by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        /// <exception cref="RepositoryException"></exception>
        public IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            IList<TEntity> result = new List<TEntity>();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                dbCommand.CommandText = whereFilter.SelectQueryString;

                foreach (var param in whereFilter.Parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value ?? (Object)DBNull.Value;

                    if (whereFilter.ParametersSize.TryGetValue(dbParameter.ParameterName, out int paramSize))
                        if (paramSize != 0)
                            dbParameter.Size = paramSize;

                    if (whereFilter.ParametersDbTye.TryGetValue(dbParameter.ParameterName, out DbType dbType))
                        dbParameter.DbType = dbType;

                    dbCommand.Parameters.Add(dbParameter);
                }

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader();
                result = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo, _dataBaseEngine);

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
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return result;
        }


        private string GetViewNameFromEntity(Type type)
        {
            var DbView = type.GetCustomAttributes(false).OfType<DbViewAttribute>().FirstOrDefault();
            if (DbView != null)
                return DbView.Name ?? type.Name;
            else
                throw new Exception(string.Format("The Entity {0}, has not linked to any view, Use [DbView] attribute to link it to a view.", type.Name));
        }

        private IList<PropertyInfo> GetPropertyInfoList(TEntity entity)
        {
            return entity.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();
        }

        private void GetDbColumns()
        {
            TEntity entity = new TEntity();

            StringBuilder sbColumnsSelect = new StringBuilder();

            using (DbConnection conn = GetDbConnection())
            {
                conn.Open();

                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                    string columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                    sbColumnsSelect.Append(string.Format("{0},", columnName));
                }
            }

            if (sbColumnsSelect.Length > 0)
                sbColumnsSelect.Remove(sbColumnsSelect.Length - 1, 1);

            ColumnsForSelect = sbColumnsSelect.ToString();
        }


        /// <summary>
        /// Gets the generic where filter.
        /// </summary>
        /// <returns></returns>
        public GenericWhereFilter GetGenericWhereFilter()
        {
            GenericWhereFilter whereFilter = new GenericWhereFilter();
            whereFilter.SetColumnsForSelect<TEntity>();
            whereFilter.SetTableOrViewName<TEntity>();

            return whereFilter;
        }

        /// <summary>
        /// Bases the IDatabaseConnection.
        /// </summary>
        /// <returns></returns>
        protected IDbConnection BaseIDbConnection()
        {
            return DataObjectProvider.GetIDbConnection(_connectionString);
        }
        private void SetSqlLamAdapter()
        {
            if (_dataBaseEngine == EDataBaseEngine.MSSqlServer)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SqlServer2012);
            else if (_dataBaseEngine == EDataBaseEngine.OleDb)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SqlServer2012);
            else if (_dataBaseEngine == EDataBaseEngine.MySql)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.MySql);
            else if (_dataBaseEngine == EDataBaseEngine.SqLite)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SQLite);
            else
                throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method");
        }
    }
}
