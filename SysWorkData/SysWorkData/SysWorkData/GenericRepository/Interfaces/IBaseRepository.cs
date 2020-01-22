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
    public interface IBaseRepository<TEntity>
    {

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        long Add(TEntity entity, out string errMessage);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        long Add(TEntity entity);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection paramDbConnection);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        long Add(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out string errMessage);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <param name="listIds">The list ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> listIds);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="listIds">The list ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, out IEnumerable<object> listIds);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="listIds">The list ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out IEnumerable<object> listIds);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="listIds">The list ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out IEnumerable<object> listIds);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="listIds">The list ids.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out IEnumerable<object> listIds);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        /// <summary>
        /// Adds an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="listIDs">The list i ds.</param>
        /// <returns></returns>
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, out IEnumerable<object> listIDs);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool Update(TEntity entity, out string errMessage);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        bool Update(TEntity entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection paramDbConnection);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        /// <summary>
        /// Updates an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, out string errMessage);

        /// <summary>
        /// Updates an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities);

        /// <summary>
        /// Updates an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection);

        /// <summary>
        /// Updates an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Updates an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Updates an range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool DeleteById(long Id, out string errMessage);

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        bool DeleteById(long Id);

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        bool DeleteById(long Id, IDbConnection paramDbConnection);

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool DeleteById(long Id, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool DeleteAll(out string errMessage);

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <returns></returns>
        long DeleteAll();

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        long DeleteAll(IDbConnection paramDbConnection);

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        long DeleteAll(IDbTransaction paramDbTransaction);

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        long DeleteAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Deletes the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter);

        /// <summary>
        /// Deletes the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection);

        /// <summary>
        /// Deletes the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Deletes the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Deletes the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter);

        /// <summary>
        /// Deletes the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, out long recordsAffected);

        /// <summary>
        /// Deletes the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);

        /// <summary>
        /// Deletes the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, out long recordsAffected);

        /// <summary>
        /// Deletes the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Deletes the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, out long recordsAffected);

        /// <summary>
        /// Deletes the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbConnection paramDbConnection);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        TEntity GetById(object id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter);

        /// <summary>
        /// Gets the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection);

        /// <summary>
        /// Gets the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter);

        /// <summary>
        /// Gets the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);

        /// <summary>
        /// Gets the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(IDbConnection paramDbConnection);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the list by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter);

        /// <summary>
        /// Gets the list by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection);

        /// <summary>
        /// Gets the list by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the list by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the generic where filter.
        /// </summary>
        /// <returns></returns>
        GenericWhereFilter GetGenericWhereFilter();

        /// <summary>
        /// Gets the list by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <returns></returns>
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter);

        /// <summary>
        /// Gets the list by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);

        /// <summary>
        /// Gets the list by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Gets the list by generic where filter.
        /// </summary>
        /// <param name="whereFilter">The where filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);

        /// <summary>
        /// Finds the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids);

        /// <summary>
        /// Finds the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection);

        /// <summary>
        /// Finds the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction dbTransaction);

        /// <summary>
        /// Finds the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
    }
}
