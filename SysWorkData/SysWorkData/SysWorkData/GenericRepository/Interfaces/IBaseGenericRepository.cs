using System.Collections.Generic;

namespace SysWork.Data.GenericRepostory.Interfaces
{
    /// <summary>
    /// Generic Interface for repositories.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseGenericRepository<T>
    {
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        long Add(T entity);

        /// <summary>
        /// Adds a range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        bool AddRange(IList<T> entities);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// Updates a range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        bool UpdateRange(IList<T> entities);

        /// <summary>
        /// Deletes and entity by the identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        bool DeleteById(long id);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T GetById(object id);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// Finds the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<T> Find(IEnumerable<object> ids);
    }
}
