/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using SysWork.Data.NetCore.Common.LambdaSqlBuilder.ValueObjects;

namespace SysWork.Data.NetCore.Common.LambdaSqlBuilder.Builder
{
    /// <summary>
    /// Implements the SQL building for JOIN, ORDER BY, SELECT, and GROUP BY
    /// </summary>
    partial class SqlQueryBuilder
    {
        /// <summary>
        /// Joins the specified original table name.
        /// </summary>
        /// <param name="originalTableName">Name of the original table.</param>
        /// <param name="joinTableName">Name of the join table.</param>
        /// <param name="leftField">The left field.</param>
        /// <param name="rightField">The right field.</param>
        public void Join(string originalTableName, string joinTableName, string leftField, string rightField)
        {
            var joinString = string.Format("JOIN {0} ON {1} = {2}",
                                           Adapter.Table(joinTableName), 
                                           Adapter.Field(originalTableName, leftField),
                                           Adapter.Field(joinTableName, rightField));
            _tableNames.Add(joinTableName);
            _joinExpressions.Add(joinString);
            _splitColumns.Add(rightField);
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        public void OrderBy(string tableName, string fieldName, bool desc = false)
        {
            var order = Adapter.Field(tableName, fieldName);
            if (desc)
                order += " DESC";

            _sortList.Add(order);            
        }

        /// <summary>
        /// Selects the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public void Select(string tableName)
        {
            var selectionString = string.Format("{0}.*", Adapter.Table(tableName));
            _selectionList.Add(selectionString);
        }

        /// <summary>
        /// Selects the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        public void Select(string tableName, string fieldName)
        {
            _selectionList.Add(Adapter.Field(tableName, fieldName));
        }

        /// <summary>
        /// Selects the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="selectFunction">The select function.</param>
        public void Select(string tableName, string fieldName, SelectFunction selectFunction)
        {
            var selectionString = string.Format("{0}({1})", selectFunction.ToString(), Adapter.Field(tableName, fieldName));
            _selectionList.Add(selectionString);
        }

        /// <summary>
        /// Groups the by.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        public void GroupBy(string tableName, string fieldName)
        {
            _groupingList.Add(Adapter.Field(tableName, fieldName));
        }
    }
}
