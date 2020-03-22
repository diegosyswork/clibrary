using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.GenericRepository.Interfaces
{
    /// <summary>
    /// Generic Interface for repositories.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>
    {
        long Add(TEntity entity);
        long Add(TEntity entity, int? commandTimeOut);
        long Add(TEntity entity, IDbConnection paramDbConnection);
        long Add(TEntity entity, IDbConnection paramDbConnection,int? commandTimeOut);
        long Add(TEntity entity, IDbTransaction paramDbTransaction);
        long Add(TEntity entity, IDbTransaction paramDbTransaction,int? commandTimeOut);
        long Add(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        long Add(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction,int? commandTimeOut);
        long Add(TEntity entity, out string errMessage);
        long Add(TEntity entity, out string errMessage,int? commandTimeOut);


        bool AddRange(IList<TEntity> entities);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection);
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, out IEnumerable<object> listIDs);
        bool AddRange(IList<TEntity> entities, out IEnumerable<object> listIds);
        bool AddRange(IList<TEntity> entities, out string errMessage);
        bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> listIds);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out IEnumerable<object> listIds);
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out IEnumerable<object> listIds);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out IEnumerable<object> listIds);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        bool Update(TEntity entity, out string errMessage);
        bool Update(TEntity entity);
        bool Update(TEntity entity, IDbConnection paramDbConnection);
        bool Update(TEntity entity, IDbTransaction paramDbTransaction);
        bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        bool UpdateRange(IList<TEntity> entities, out string errMessage);
        bool UpdateRange(IList<TEntity> entities);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection);
        bool UpdateRange(IList<TEntity> entities, IDbTransaction paramDbTransaction);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        bool DeleteById(long Id, out string errMessage);
        bool DeleteById(long Id);
        bool DeleteById(long Id, IDbConnection paramDbConnection);
        bool DeleteById(long Id, IDbTransaction paramDbTransaction);
        bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        bool DeleteAll(out string errMessage);
        long DeleteAll();
        long DeleteAll(IDbConnection paramDbConnection);
        long DeleteAll(IDbTransaction paramDbTransaction);
        long DeleteAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter);
        long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection);
        long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction);
        long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        TEntity GetById(object id);
        TEntity GetById(object id, IDbConnection paramDbConnection);
        TEntity GetById(object id, IDbTransaction paramDbTransaction);
        TEntity GetById(object id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter);
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection);
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction);
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        IList<TEntity> GetAll();
        IList<TEntity> GetAll(IDbConnection paramDbConnection);
        IList<TEntity> GetAll(IDbTransaction paramDbTransaction);
        IList<TEntity> GetAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter);
        IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection);
        IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction);
        IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        GenericWhereFilter GetGenericWhereFilter();

        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        IList<TEntity> Find(IEnumerable<object> ids);
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection);
        IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction dbTransaction);
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
    }
}
