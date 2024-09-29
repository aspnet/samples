using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MjrChess.Trainer.Models;

namespace MjrChess.Trainer.Data
{
    /// <summary>
    /// Base interface for retrieving entities from persisted storage.
    /// </summary>
    /// <typeparam name="T">The type of domain model entity to retrieve.</typeparam>
    public interface IRepository<T>
        where T : IEntity
    {
        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity with the specified ID, or null if there is no such entity in the repository.</returns>
        Task<T?> GetAsync(int id);

        /// <summary>
        /// Gets a query for all entities of type T.
        /// </summary>
        /// <returns>An IQueryable for all T in the repository.</returns>
        IQueryable<T> Query();

        /// <summary>
        /// Gets a filtered query for entities of type T.
        /// </summary>
        /// <param name="filter">Filter used to determine whether entities are returned in the IQueryable.</param>
        /// <returns>An IQueryable of all T in the repository satisfying the filter.</returns>
        IQueryable<T> Query(Expression<Func<T, bool>>? filter);

        /// <summary>
        /// Adds an item to the repository.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>The item after being added.</returns>
        Task<T> AddAsync(T item);

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="item">The entity to be updated, with updated properties.</param>
        /// <returns>The updated entity, or null if no entity with the given ID exists in the repository.</returns>
        Task<T?> UpdateAsync(T item);

        /// <summary>
        /// Attemps to delete an item from the repository based on its ID.
        /// </summary>
        /// <param name="id">The ID of the item to delete.</param>
        /// <returns>True if the item was deleted, false if it did not exist in the repository.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
