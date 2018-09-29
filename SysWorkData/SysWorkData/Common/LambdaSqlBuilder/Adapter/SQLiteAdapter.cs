using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common.LambdaSqlBuilder.Adapter
{
    class SQLiteAdapter : SqlAdapterBase, ISqlAdapter
    {
        public string QueryStringPage(string source, string selection, string conditions, string order,
            int pageSize)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} LIMIT {4} ",
                    selection, source, conditions, order, pageSize);
        }

        public string QueryStringPage(string source, string selection, string conditions, string order,
        int pageSize, int pageNumber)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} LIMIT {4} OFFSET {5}",
                                 selection, source, conditions, order, pageSize, pageSize * (pageNumber - 1));
        }

        public string Table(string tableName)
        {
            return string.Format("[{0}]", tableName);
        }

        public string Field(string tableName, string fieldName)
        {
            return string.Format("[{0}].[{1}]", tableName, fieldName);
        }

        public string Parameter(string parameterId)
        {
            return "@" + parameterId;
        }
    }


}
