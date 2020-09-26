/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SysWork.Data.Common.LambdaSqlBuilder.Builder;
using SysWork.Data.Common.LambdaSqlBuilder.Resolver;
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;

namespace SysWork.Data.Common.LambdaSqlBuilder
{
    ///TODO: Revisar documentacion, y crear ejemplos de los metodos.

    /// <summary>
    /// The single most important LambdaSqlBuilder class. Encapsulates the whole SQL building and lambda expression resolving logic. 
    /// Serves as a proxy to the underlying SQL builder and the lambda expression resolver. It should be used to continually build the SQL query
    /// and then request the QueryString as well as the QueryParameters at the end.
    /// </summary>
    /// <typeparam name="T">Entity type required for lambda expressions as well as for proper resolution of the table name and the column names</typeparam>
    public class SqlLam<T> : SqlLamBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLam{T}"/> class.
        /// </summary>
        public SqlLam()
        {
            _builder = new SqlQueryBuilder(LambdaResolver.GetTableOrViewName<T>(), _defaultAdapter);
            _resolver = new LambdaResolver(_builder);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLam{T}"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public SqlLam(Expression<Func<T, bool>> expression) : this()
        {
            Where(expression);
        }

        internal SqlLam(SqlQueryBuilder builder, LambdaResolver resolver)
        {
            _builder = builder;
            _resolver = resolver;
        }

        /// <summary>
        /// Wheres the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> Where(Expression<Func<T, bool>> expression)
        {
            return And(expression);
        }

        /// <summary>
        /// Ands the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> And(Expression<Func<T, bool>> expression)
        {
            _builder.And();
            _resolver.ResolveQuery(expression);
            return this;
        }

        /// <summary>
        /// Ors the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> Or(Expression<Func<T, bool>> expression)
        {
            _builder.Or();
            _resolver.ResolveQuery(expression);
            return this;
        }

        /// <summary>
        /// Wheres the is in.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns></returns>
        public SqlLam<T> WhereIsIn(Expression<Func<T, object>> expression, SqlLamBase sqlQuery)
        {
            _builder.And();
            _resolver.QueryByIsIn(expression, sqlQuery);
            return this;
        }

        /// <summary>
        /// Wheres the is in.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public SqlLam<T> WhereIsIn(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            _builder.And();
            _resolver.QueryByIsIn(expression, values);
            return this;
        }

        /// <summary>
        /// Wheres the not in.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns></returns>
        public SqlLam<T> WhereNotIn(Expression<Func<T, object>> expression, SqlLamBase sqlQuery)
        {
            _builder.And();
            _resolver.QueryByNotIn(expression, sqlQuery);
            return this;
        }

        /// <summary>
        /// Wheres the not in.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public SqlLam<T> WhereNotIn(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            _builder.And();
            _resolver.QueryByNotIn(expression, values);
            return this;
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> OrderBy(Expression<Func<T, object>> expression)
        {
            _resolver.OrderBy(expression);
            return this;
        }

        /// <summary>
        /// Orders the by descending.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            _resolver.OrderBy(expression, true);
            return this;
        }

        /// <summary>
        /// Selects the specified expressions.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns></returns>
        public SqlLam<T> Select(params Expression<Func<T, object>>[] expressions)
        {
            foreach (var expression in expressions)
                _resolver.Select(expression);
            return this;
        }

        /// <summary>
        /// Selects the count.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> SelectCount(Expression<Func<T, object>> expression)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.COUNT);
            return this;
        }

        /// <summary>
        /// Selects the distinct.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> SelectDistinct(Expression<Func<T, object>> expression)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.DISTINCT);
            return this;
        }

        /// <summary>
        /// Selects the sum.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> SelectSum(Expression<Func<T, object>> expression)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.SUM);
            return this;
        }

        /// <summary>
        /// Selects the maximum.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> SelectMax(Expression<Func<T, object>> expression)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.MAX);
            return this;
        }

        /// <summary>
        /// Selects the minimum.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> SelectMin(Expression<Func<T, object>> expression)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.MIN);
            return this;
        }

        /// <summary>
        /// Selects the average.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> SelectAverage(Expression<Func<T, object>> expression)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.AVG);
            return this;
        }

        /// <summary>
        /// Joins the specified join query.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="joinQuery">The join query.</param>
        /// <param name="primaryKeySelector">The primary key selector.</param>
        /// <param name="foreignKeySelector">The foreign key selector.</param>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public SqlLam<TResult> Join<T2, TKey, TResult>(SqlLam<T2> joinQuery,  
            Expression<Func<T, TKey>> primaryKeySelector, 
            Expression<Func<T, TKey>> foreignKeySelector,
            Func<T, T2, TResult> selection)
        {
            var query = new SqlLam<TResult>(_builder, _resolver);
            _resolver.Join<T, T2, TKey>(primaryKeySelector, foreignKeySelector);
            return query;
        }

        /// <summary>
        /// Joins the specified expression.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T2> Join<T2>(Expression<Func<T, T2, bool>> expression)
        {
            var joinQuery = new SqlLam<T2>(_builder, _resolver);
            _resolver.Join(expression);
            return joinQuery;
        }

        /// <summary>
        /// Groups the by.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public SqlLam<T> GroupBy(Expression<Func<T, object>> expression)
        {
            _resolver.GroupBy(expression);
            return this;
        }
    }
}
