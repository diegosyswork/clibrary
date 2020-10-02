/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using SysWork.Data.Common.LambdaSqlBuilder.Adapter;

namespace SysWork.Data.Common.LambdaSqlBuilder.Builder
{
    /// <summary>
    /// Implements the whole SQL building logic. Continually adds and stores the SQL parts as the requests come. 
    /// When requested to return the QueryString, the parts are combined and returned as a single query string.
    /// The query parameters are stored in a dictionary implemented by an ExpandoObject that can be requested by QueryParameters.
    /// </summary>
    public partial class SqlQueryBuilder
    {
        internal ISqlAdapter Adapter { get; set; }

        private const string PARAMETER_PREFIX = "param_";

        private readonly List<string> _tableNames = new List<string>();
        private readonly List<string> _joinExpressions = new List<string>();
        private readonly List<string> _selectionList = new List<string>();
        private readonly List<string> _conditions = new List<string>();
        private readonly List<string> _sortList = new List<string>();
        private readonly List<string> _groupingList = new List<string>();
        private readonly List<string> _havingConditions = new List<string>();
        private readonly List<string> _splitColumns = new List<string>();
        private int _paramIndex;


        /// <summary>
        /// Gets the table names.
        /// </summary>
        /// <value>
        /// The table names.
        /// </value>
        public List<string> TableNames { get { return _tableNames; } }

        /// <summary>
        /// Gets the join expressions.
        /// </summary>
        /// <value>
        /// The join expressions.
        /// </value>
        public List<string> JoinExpressions { get { return _joinExpressions; } }

        /// <summary>
        /// Gets the selection list.
        /// </summary>
        /// <value>
        /// The selection list.
        /// </value>
        public List<string> SelectionList { get { return _selectionList; } }

        /// <summary>
        /// Gets the where conditions.
        /// </summary>
        /// <value>
        /// The where conditions.
        /// </value>
        public List<string> WhereConditions { get { return _conditions; } }

        /// <summary>
        /// Gets the order by list.
        /// </summary>
        /// <value>
        /// The order by list.
        /// </value>
        public List<string> OrderByList { get { return _sortList; } }

        /// <summary>
        /// Gets the group by list.
        /// </summary>
        /// <value>
        /// The group by list.
        /// </value>
        public List<string> GroupByList { get { return _groupingList; } }

        /// <summary>
        /// Gets the having conditions.
        /// </summary>
        /// <value>
        /// The having conditions.
        /// </value>
        public List<string> HavingConditions { get { return _havingConditions; } }

        /// <summary>
        /// Gets the split columns.
        /// </summary>
        /// <value>
        /// The split columns.
        /// </value>
        public List<string> SplitColumns { get { return _splitColumns; } }
        
        /// <summary>
        /// Gets the index of the current parameter.
        /// </summary>
        /// <value>
        /// The index of the current parameter.
        /// </value>
        public int CurrentParamIndex { get { return _paramIndex; } }

        private string Source
        {
            get
            {
                var joinExpression = string.Join(" ", _joinExpressions);
                return string.Format("{0} {1}", Adapter.Table(_tableNames.First()), joinExpression);
            }
        }

        private string Selection
        {
            get
            {
                if (_selectionList.Count == 0)
                    return string.Format("{0}.*", Adapter.Table(_tableNames.First()));
                else
                    return string.Join(", ", _selectionList);
            }
        }

        /// <summary>
        /// Gets the conditions.
        /// </summary>
        /// <value>
        /// The conditions.
        /// </value>
        public string Conditions
        {
            get
            {
                if (_conditions.Count == 0)
                    return "";
                else
                    return "WHERE " + string.Join("", _conditions);
            }
        }

        private string Order
        {
            get
            {
                if (_sortList.Count == 0)
                    return "";
                else
                    return "ORDER BY " + string.Join(", ", _sortList);
            }
        }

        private string Grouping
        {
            get
            {
                if (_groupingList.Count == 0)
                    return "";
                else
                    return "GROUP BY " + string.Join(", ", _groupingList);
            }
        }

        private string Having
        {
            get
            {
                if (_havingConditions.Count == 0)
                    return "";
                else
                    return "HAVING " + string.Join(" ", _havingConditions);
            }
        }
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IDictionary<string, object> Parameters { get; private set; }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public string QueryString
        {
            get { return Adapter.QueryString(Selection, Source, Conditions, Grouping, Having, Order); }
        }


        /// <summary>
        /// Queries the string page.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Pagination requires the ORDER BY statement to be specified</exception>
        public string QueryStringPage(int pageSize, int? pageNumber = null)
        {
            if (pageNumber.HasValue)
            {
                if (_sortList.Count == 0)
                    throw new Exception("Pagination requires the ORDER BY statement to be specified");

                return Adapter.QueryStringPage(Source, Selection, Conditions, Order, pageSize, pageNumber.Value);
            }
            
            return Adapter.QueryStringPage(Source, Selection, Conditions, Order, pageSize);
        }

        internal SqlQueryBuilder(string tableName, ISqlAdapter adapter)
        {
            _tableNames.Add(tableName);
            Adapter = adapter;
            Parameters = new ExpandoObject();
            _paramIndex = 0;
        }

        private string NextParamId()
        {
            ++_paramIndex;
            return PARAMETER_PREFIX + _paramIndex.ToString(CultureInfo.InvariantCulture);
        }

        private void AddParameter(string key, object value)
        {
            if(!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }
    }
}
