/* License: http://www.apache.org/licenses/LICENSE-2.0 */
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;

namespace SysWork.Data.Common.LambdaSqlBuilder.Adapter
{
    /// <summary>
    /// SQL adapter provides db specific functionality related to db specific SQL syntax
    /// </summary>
    interface ISqlAdapter
    {
        string QueryString(string selection, string source, string conditions, 
            string order, string grouping, string having);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize, int pageNumber);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize);

        string Table(string tableName);
        string Field(string tableName, string fieldName);
        string Parameter(string parameterId);
    }
}
