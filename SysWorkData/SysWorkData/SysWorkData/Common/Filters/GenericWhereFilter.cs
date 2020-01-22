using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using SysWork.Data.Common.Syntax;
using SysWork.Data.GenericRepository.Attributes;

namespace SysWork.Data.Common.Filters
{
    /// <summary>
    /// Generic class to help the creation of clauses where. 
    /// </summary>
    /// <remarks>
    /// Generic class to help the creation of clauses where. 
    /// Supports the use of DbParameters or literal values.
    /// If not specify DatabaseEngine, the default is MSSqlServer.
    /// </remarks>
    /// <example>
    /// <![CDATA[
    ///
    /// 
    /// 
    /// ]]>
    /// </example>
    public class GenericWhereFilter
    {
        private const string _filterParameterPrefix = "@p_dbex_";
        private SyntaxProvider _syntaxProvider;
        private EDataBaseEngine _databaseEngine;

        private string _columnsForSelect;
        /// <summary>
        /// Gets the columns for select.
        /// </summary>
        /// <value>
        /// The columns for select.
        /// </value>
        public string ColumnsForSelect { get { return _columnsForSelect; } private set { } }

        private string _tableOrViewName = null;
        /// <summary>
        /// Gets the name of the table or view.
        /// </summary>
        /// <value>
        /// The name of the table or view.
        /// </value>
        public string TableOrViewName { get { return _tableOrViewName; } private set { }}

        private string _where = null;
        /// <summary>
        /// Gets the where clause.
        /// </summary>
        /// <value>
        /// The where.
        /// </value>
        public string Where { get { return _where; } private set { } }

        private string _orderBy = null;
        /// <summary>
        /// Gets the order by clause.
        /// </summary>
        /// <value>
        /// The order by.
        /// </value>
        public string OrderBy { get { return _orderBy; } private set { } }

        /// <summary>
        /// Gets the Select queryString.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public string SelectQueryString { get { return GetSelectQueryString(); } private set { } }

        /// <summary>
        /// Gets the Delete query string.
        /// </summary>
        /// <value>
        /// The delete query string.
        /// </value>
        public string DeleteQueryString { get { return GetDeleteQueryString(); } private set { } }

        /// <summary>
        /// Gets the Update query string.
        /// </summary>
        /// <value>
        /// The update query string.
        /// </value>
        public string UpdateQueryString { get { return GetUpdateQueryString(); } private set { } }

        /// <summary>
        /// Gets the parameters names and values.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IDictionary<string, object> Parameters { get; private set; }

        /// <summary>
        /// Gets the size of the parameters.
        /// </summary>
        /// <value>
        /// The size of the parameters.
        /// </value>
        public IDictionary<string, int> ParametersSize { get; private set; }

        /// <summary>
        /// Gets the parameters database tye.
        /// </summary>
        /// <value>
        /// The parameters database tye.
        /// </value>
        public IDictionary<string, DbType> ParametersDbTye { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericWhereFilter"/> class.
        /// The DatabaseEngine is MSSqlServer
        /// </summary>
        public GenericWhereFilter()
        {
            GenericFilterQueryConstructorResolver(EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericWhereFilter"/> class.
        /// </summary>
        /// <param name="databaseEngine">The database engine.</param>
        public GenericWhereFilter(EDataBaseEngine databaseEngine)
        {
            GenericFilterQueryConstructorResolver(databaseEngine);
        }

        private void GenericFilterQueryConstructorResolver(EDataBaseEngine databaseEngine)
        {
            _databaseEngine = databaseEngine;
            _syntaxProvider = new SyntaxProvider(_databaseEngine);

            Parameters = new Dictionary<string, object>();
            ParametersSize = new Dictionary<string, int>();
            ParametersDbTye = new Dictionary<string, DbType>();
        }

        /// <summary>
        /// Sets the name of the table or view manually.
        /// </summary>
        /// <param name="tableOrViewName">Name of the table or view.</param>
        public void SetTableOrViewName(string tableOrViewName)
        {
            _tableOrViewName = tableOrViewName;
        }

        /// <summary>
        /// Sets the name of the table or view using an entity.
        /// If the entity have a DbTable or DbView attributes uses this, 
        /// other case use the name of class.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        public void SetTableOrViewName<TEntity>() where TEntity : class, new()
        {
            TEntity t = new TEntity();
            bool isDbTable = false;

            var table = t.GetType().GetCustomAttributes(false).OfType<DbTableAttribute>().FirstOrDefault();
            if (table != null)
            {
                isDbTable = true;
                _tableOrViewName = table.Name;
            }
            else
            {
                _tableOrViewName = t.GetType().Name;
            }

            if (!isDbTable)
            {
                var view = t.GetType().GetCustomAttributes(false).OfType<DbViewAttribute>().FirstOrDefault();
                if (view!= null)
                {
                    _tableOrViewName = view.Name;
                }
                else
                {
                    _tableOrViewName = t.GetType().Name;
                }
            }

            _tableOrViewName = _syntaxProvider.GetSecureTableName(_tableOrViewName);
        }

        /// <summary>
        /// Sets the columns for select manually, separated by (,) comma.
        /// </summary>
        /// <param name="columnsForSelect">The columns for select.</param>
        public void SetColumnsForSelect(string columnsForSelect)
        {
            string[] fields = columnsForSelect.Split(',');
            for (int i = 0; i < fields.Length; i++)
                fields[i] = _syntaxProvider.GetSecureColumnName(fields[i]);


            _columnsForSelect = string.Join(",", fields);
        }

        /// <summary>
        /// Sets the columns for select using an entity.
        /// The properties of entity must be have a DbColumn attribute
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        public void SetColumnsForSelect<TEntity>() where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            StringBuilder sbColumnsSelect = new StringBuilder();

            foreach (PropertyInfo i in entity.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList())
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                string columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                sbColumnsSelect.Append(string.Format("{0},", columnName));
            }

            if (sbColumnsSelect.Length > 0)
                sbColumnsSelect.Remove(sbColumnsSelect.Length - 1, 1);

            _columnsForSelect = sbColumnsSelect.ToString();
        }

        /// <summary>
        /// Sets the where clause. Use literal values or parameters.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public GenericWhereFilter SetWhere(string where)
        {
            string w = "WHERE";
            if (!string.IsNullOrEmpty(where))
            {
                _where = where.Trim();

                if (_where.StartsWith(w, System.StringComparison.InvariantCultureIgnoreCase))
                    _where = _where.Substring(w.Length);
            }

            return this;
        }

        /// <summary>
        /// Sets the order by clause.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public GenericWhereFilter SetOrderBy(string orderBy)
        {
            string w = "ORDER BY";
            if (!string.IsNullOrEmpty(orderBy))
            {
                _orderBy = orderBy.Trim();

                if (_orderBy.StartsWith(w, System.StringComparison.InvariantCultureIgnoreCase))
                    _orderBy = _orderBy.Substring(w.Length);
            }

            return this;
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public GenericWhereFilter AddParameter(string name, object value)
        {
            if (!Parameters.ContainsKey(name))
                Parameters.Add(name, value);

            if (!ParametersSize.ContainsKey(name))
                ParametersSize.Add(name, 0);

            return this;
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public GenericWhereFilter AddParameter(string name, object value, int size)
        {
            if (!Parameters.ContainsKey(name))
                Parameters.Add(name, value);

            if (!ParametersSize.ContainsKey(name))
                ParametersSize.Add(name, size);

            return this;
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <returns></returns>
        public GenericWhereFilter AddParameter(string name, object value, DbType dbType)
        {
            if (!Parameters.ContainsKey(name))
                Parameters.Add(name, value);

            if (!ParametersDbTye.ContainsKey(name))
                ParametersDbTye.Add(name, dbType);

            return this;
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public GenericWhereFilter AddParameter(string name, object value, DbType dbType, int size)
        {
            if (!Parameters.ContainsKey(name))
                Parameters.Add(name, value);

            if (!ParametersSize.ContainsKey(name))
                ParametersSize.Add(name, size);

            if (!ParametersDbTye.ContainsKey(name))
                ParametersDbTye.Add(name, dbType);

            return this;
        }

        /// <summary>
        /// Adds multiples parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public GenericWhereFilter AddParameters(IDictionary<string, object> parameters)
        {
            foreach (var key in parameters.Keys)
                Parameters.Add(key, parameters[key]);

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
        ///     var id = new DbExecutor(connectionString)
        ///                 .InsertQuery("Products")
        ///                 .AddFieldWithValue("IdProduct",77978788)
        ///                 .AddFieldWithValue("Description",77978788)
        ///                 .AddFieldWithValue("IdCategory",5)
        ///                 .AddFieldWithValue("Cost",155.4)
        ///             .ExecuteScalar();
        /// </code> 
        /// </example>
        /// <returns>
        /// The same instance. Use the builder pattern
        /// </returns>
        public GenericWhereFilter AddFieldWithValue(string fieldName, object value)
        {
            string parameterName = _filterParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!Parameters.ContainsKey(parameterName))
                Parameters.Add(parameterName, value);

            if (!ParametersSize.ContainsKey(parameterName))
                ParametersSize.Add(parameterName, 0);

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
        public GenericWhereFilter AddFieldWithValue(string fieldName, object value, int size)
        {
            string parameterName = _filterParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!Parameters.ContainsKey(parameterName))
                Parameters.Add(parameterName, value);

            if (!ParametersSize.ContainsKey(parameterName))
                ParametersSize.Add(parameterName, size);

            return this;
        }

        /// <summary>
        /// Adds the field with value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <returns></returns>
        public GenericWhereFilter AddFieldWithValue(string fieldName, object value, DbType dbType)
        {
            string parameterName = _filterParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!Parameters.ContainsKey(parameterName))
                Parameters.Add(parameterName, value);

            if (!ParametersDbTye.ContainsKey(parameterName))
                ParametersDbTye.Add(parameterName, dbType);

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
        public GenericWhereFilter AddFieldWithValue(string fieldName, object value, DbType dbType, int size)
        {
            string parameterName = _filterParameterPrefix + _syntaxProvider.SecureNameForParameter(fieldName);

            if (!Parameters.ContainsKey(parameterName))
                Parameters.Add(parameterName, value);

            if (!ParametersSize.ContainsKey(parameterName))
                ParametersSize.Add(parameterName, size);

            if (!ParametersDbTye.ContainsKey(parameterName))
                ParametersDbTye.Add(parameterName, dbType);

            return this;
        }

        private string GetSelectQueryString()
        {
            var queryTemplate = "SELECT {0} FROM {1} {2} {3}";

            string paramWhere = "";
            paramWhere = string.IsNullOrEmpty(_where) ? "" : _where.Trim();
            if (paramWhere != "")
                paramWhere = " WHERE " + paramWhere;

            string paramOrderBy = "";
            paramOrderBy = string.IsNullOrEmpty(_orderBy) ? "" : _orderBy.Trim();
            if (paramOrderBy != "")
                paramOrderBy = " ORDER BY " + paramOrderBy;

            return string.Format(queryTemplate, _columnsForSelect, _tableOrViewName, paramWhere, paramOrderBy);
        }

        private string GetDeleteQueryString()
        {
            var queryTemplate = "DELETE FROM {0} {1}";

            string paramWhere = "";
            paramWhere = string.IsNullOrEmpty(_where) ? "" : _where.Trim();
            if (paramWhere != "")
                paramWhere = " WHERE " + paramWhere;

            return string.Format(queryTemplate, _tableOrViewName, paramWhere);
        }

        private string GetUpdateQueryString()
        {
            var queryTemplate = "UPDATE {0} SET {1} {2}";
            var list = Parameters.Keys.ToList();
            list.Sort();

            var listFieldsAndParametersValues = new List<string>();
            foreach (var key in list)
            {
                if (key.StartsWith(_filterParameterPrefix))
                    listFieldsAndParametersValues.Add(_syntaxProvider.GetSecureColumnName(_syntaxProvider.SecureParameterNameToField(key.Replace(_filterParameterPrefix, ""))) + " = " + key);
            }

            var fieldsAndValues = string.Join(",", listFieldsAndParametersValues);

            string paramWhere = "";
            paramWhere = string.IsNullOrEmpty(_where) ? "" : _where.Trim();
            if (paramWhere != "")
                paramWhere = " WHERE " + paramWhere;

            return string.Format(queryTemplate, _tableOrViewName, fieldsAndValues, paramWhere);
        }
    }
}