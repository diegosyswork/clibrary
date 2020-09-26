/* License: http://www.apache.org/licenses/LICENSE-2.0 */
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SysWork.Data.Common.LambdaSqlBuilder.Builder
{
    /// <summary>
    /// Implements the expression buiding for the WHERE statement
    /// </summary>
    partial class SqlQueryBuilder
    {
        /// <summary>
        /// Begins the expression.
        /// </summary>
        public void BeginExpression()
        {
            _conditions.Add("(");
        }


        /// <summary>
        /// Ends the expression.
        /// </summary>
        public void EndExpression()
        {
            _conditions.Add(")");
        }

        /// <summary>
        /// Ands this instance.
        /// </summary>
        public void And()
        {
            if (_conditions.Count > 0)
                _conditions.Add(" AND ");
        }

        /// <summary>
        /// Ors this instance.
        /// </summary>
        public void Or()
        {
            if (_conditions.Count > 0)
                _conditions.Add(" OR ");
        }

        /// <summary>
        /// Nots this instance.
        /// </summary>
        public void Not()
        {
            _conditions.Add(" NOT ");
        }

        /// <summary>
        /// Queries the by field.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="op">The op.</param>
        /// <param name="fieldValue">The field value.</param>
        public void QueryByField(string tableName, string fieldName, string op, object fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} {1} {2}",
                Adapter.Field(tableName, fieldName),
                op,
                Adapter.Parameter(paramId));

            _conditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        /// <summary>
        /// Queries the by field like.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        public void QueryByFieldLike(string tableName, string fieldName, string fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} LIKE {1}",
                Adapter.Field(tableName, fieldName),
                Adapter.Parameter(paramId));

            _conditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        /// <summary>
        /// Queries the by field null.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        public void QueryByFieldNull(string tableName, string fieldName)
        {
            _conditions.Add(string.Format("{0} IS NULL", Adapter.Field(tableName, fieldName)));
        }

        /// <summary>
        /// Queries the by field not null.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        public void QueryByFieldNotNull(string tableName, string fieldName)
        {
            _conditions.Add(string.Format("{0} IS NOT NULL", Adapter.Field(tableName, fieldName)));
        }

        /// <summary>
        /// Queries the by field comparison.
        /// </summary>
        /// <param name="leftTableName">Name of the left table.</param>
        /// <param name="leftFieldName">Name of the left field.</param>
        /// <param name="op">The op.</param>
        /// <param name="rightTableName">Name of the right table.</param>
        /// <param name="rightFieldName">Name of the right field.</param>
        public void QueryByFieldComparison(string leftTableName, string leftFieldName, string op,
            string rightTableName, string rightFieldName)
        {
            string newCondition = string.Format("{0} {1} {2}",
            Adapter.Field(leftTableName, leftFieldName),
            op,
            Adapter.Field(rightTableName, rightFieldName));

            _conditions.Add(newCondition);
        }

        /// <summary>
        /// Queries the by is in.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="sqlQuery">The SQL query.</param>
        public void QueryByIsIn(string tableName, string fieldName, SqlLamBase sqlQuery)
        {
            var innerQuery = sqlQuery.QueryString;            
            foreach (var param in sqlQuery.QueryParameters)
            {
                var innerParamKey = "Inner" + param.Key;
                innerQuery = Regex.Replace(innerQuery, param.Key, innerParamKey);
                AddParameter(innerParamKey, param.Value);
            }

            var newCondition = string.Format("{0} IN ({1})", Adapter.Field(tableName, fieldName), innerQuery);

            _conditions.Add(newCondition);
        }

        /// <summary>
        /// Queries the by is in.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="values">The values.</param>
        public void QueryByIsIn(string tableName, string fieldName, IEnumerable<object> values)
        {
            var paramIds = values.Select(x =>
                                             {
                                                 var paramId = NextParamId();
                                                 AddParameter(paramId, x);
                                                 return Adapter.Parameter(paramId);
                                             });

            var newCondition = string.Format("{0} IN ({1})", Adapter.Field(tableName, fieldName), string.Join(",", paramIds));
            _conditions.Add(newCondition);
        }
    }
}
