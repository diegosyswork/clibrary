/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using SysWork.Data.NetCore.Common.LambdaSqlBuilder.Adapter;
using SysWork.Data.NetCore.Common.LambdaSqlBuilder.Builder;
using SysWork.Data.NetCore.Common.LambdaSqlBuilder.Resolver;
using SysWork.Data.NetCore.Common.LambdaSqlBuilder.ValueObjects;

namespace SysWork.Data.NetCore.Common.LambdaSqlBuilder
{
    ///TODO: Revisar dodcumentacion y crear ejemplo de los metodos.
    /// <summary>
    /// Base functionality for the SqlLam class that is not related to any specific entity type
    /// </summary>
    public abstract class SqlLamBase
    {
        //internal static ISqlAdapter _defaultAdapter = new SqlServer2012Adapter();
        internal static ISqlAdapter _defaultAdapter = new SqlServer2012Adapter();
        internal SqlQueryBuilder _builder;
        internal LambdaResolver _resolver;
        /// <summary>
        /// Gets the SQL builder.
        /// </summary>
        /// <value>
        /// The SQL builder.
        /// </value>
        public SqlQueryBuilder SqlBuilder { get { return _builder; } }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public string QueryString
        {
            get { return _builder.QueryString; }
        }

        /// <summary>
        /// Gets the query where.
        /// </summary>
        /// <value>
        /// The query where.
        /// </value>
        public string QueryWhere
        {
            get { return _builder.Conditions; }
        }

        /// <summary>
        /// Queries the string page.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns></returns>
        public string QueryStringPage(int pageSize, int? pageNumber = null)
        {
            return _builder.QueryStringPage(pageSize, pageNumber);
        }

        /// <summary>
        /// Gets the query parameters.
        /// </summary>
        /// <value>
        /// The query parameters.
        /// </value>
        public IDictionary<string, object> QueryParameters
        {
            get { return _builder.Parameters; }
        }

        /// <summary>
        /// Gets the split columns.
        /// </summary>
        /// <value>
        /// The split columns.
        /// </value>
        public string[] SplitColumns
        {
            get { return _builder.SplitColumns.ToArray(); }
        }


        /// <summary>
        /// Sets the adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public static void SetAdapter(SqlAdapter adapter)
        {
            _defaultAdapter = GetAdapterInstance(adapter);
        }

        private static ISqlAdapter GetAdapterInstance(SqlAdapter adapter)
        {
            switch (adapter)
            {
                case SqlAdapter.SqlServer2008:
                    return new SqlServer2008Adapter();
                case SqlAdapter.SqlServer2012:
                    return new SqlServer2012Adapter();
                case SqlAdapter.MySql:
                    return new MySQLAdapter();
                case SqlAdapter.SQLite:
                    return new SQLiteAdapter();
                default:
                    throw new ArgumentException("The specified Sql Adapter was not recognized");
            }
        }
    }
}
