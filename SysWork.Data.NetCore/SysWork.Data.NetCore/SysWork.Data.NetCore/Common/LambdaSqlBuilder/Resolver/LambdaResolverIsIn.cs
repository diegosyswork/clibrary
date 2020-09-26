/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SysWork.Data.NetCore.Common.LambdaSqlBuilder.Resolver
{
    partial class LambdaResolver
    {
        public void QueryByIsIn<T>(Expression<Func<T, object>> expression, SqlLamBase sqlQuery)
        {
            var fieldName = GetColumnName(expression);
            _builder.QueryByIsIn(GetTableOrViewName<T>(), fieldName, sqlQuery);
        }

        public void QueryByIsIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            var fieldName = GetColumnName(expression);
            _builder.QueryByIsIn(GetTableOrViewName<T>(), fieldName, values);
        }

        public void QueryByNotIn<T>(Expression<Func<T, object>> expression, SqlLamBase sqlQuery)
        {
            var fieldName = GetColumnName(expression);
            _builder.Not();
            _builder.QueryByIsIn(GetTableOrViewName<T>(), fieldName, sqlQuery);
        }

        public void QueryByNotIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            var fieldName = GetColumnName(expression);
            _builder.Not();
            _builder.QueryByIsIn(GetTableOrViewName<T>(), fieldName, values);
        }
    }
}
