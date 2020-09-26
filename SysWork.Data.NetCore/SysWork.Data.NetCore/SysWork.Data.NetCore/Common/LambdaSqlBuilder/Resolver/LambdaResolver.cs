/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SysWork.Data.NetCore.Common.LambdaSqlBuilder.Builder;
using SysWork.Data.NetCore.Common.Attributes;

namespace SysWork.Data.NetCore.Common.LambdaSqlBuilder.Resolver
{
    partial class LambdaResolver
    {
        private Dictionary<ExpressionType, string> _operationDictionary = new Dictionary<ExpressionType, string>()
                                                                              {
                                                                                  { ExpressionType.Equal, "="},
                                                                                  { ExpressionType.NotEqual, "!="},
                                                                                  { ExpressionType.GreaterThan, ">"},
                                                                                  { ExpressionType.LessThan, "<"},
                                                                                  { ExpressionType.GreaterThanOrEqual, ">="},
                                                                                  { ExpressionType.LessThanOrEqual, "<="}
                                                                              };

        private SqlQueryBuilder _builder { get; set; }

        public LambdaResolver(SqlQueryBuilder builder)
        {
            _builder = builder;
        }

        #region helpers
        public static string GetColumnName<T>(Expression<Func<T, object>> selector)
        {
            return GetColumnName(GetMemberExpression(selector.Body));
        }

        public static string GetColumnName(Expression expression)
        {
            var member = GetMemberExpression(expression);
            var column = member.Member.GetCustomAttributes(false).OfType<DbColumnAttribute>().FirstOrDefault();
            var columnName = column.ColumnName ?? member.Member.Name;

            return columnName;
        }

        public static string GetTableOrViewName<T>()
        {
            return GetTableOrViewName(typeof(T));
        }

        /// <summary>
        /// Gets the name of the table or view. If DbTable and DbView are not present, returs the name of class (Type Name).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// </returns>
        public static string GetTableOrViewName(Type type)
        {
            var table = type.GetCustomAttributes(false).OfType<DbTableAttribute>().FirstOrDefault();
            var view = type.GetCustomAttributes(false).OfType<DbViewAttribute>().FirstOrDefault();

            if (table != null)
                return table.Name;
            else if (view != null)
                return view.Name;
            else
                return type.Name;
        }

        private static string GetTableName(MemberExpression expression)
        {
            return GetTableOrViewName(expression.Member.DeclaringType);
        }

        private static BinaryExpression GetBinaryExpression(Expression expression)
        {
            if (expression is BinaryExpression)
                return expression as BinaryExpression;

            throw new ArgumentException("Binary expression expected");
        }

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return expression as MemberExpression;
                case ExpressionType.Convert:
                    return GetMemberExpression((expression as UnaryExpression).Operand);
            }


            throw new ArgumentException("Member expression expected");
        }
        
        #endregion
    }
}
