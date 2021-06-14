using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MjrChess.Trainer.Models;

namespace MjrChess.Trainer.Data
{
    /// <summary>
    /// Generic repository service for retrieving domain model entities via Entity Framework (to retrieve data model entities) and Automapper (to map to domain model).
    /// </summary>
    /// <typeparam name="TData">The data model type the repository retrieves.</typeparam>
    /// <typeparam name="T">The domain model type the repository returns.</typeparam>
    public class EFRepository<TData, T> : IRepository<T>
        where T : IEntity
        where TData : Data.Models.EntityBase
    {
        protected PuzzleDbContext Context { get; }

        private IMapper Mapper { get; }

        private ILogger<EFRepository<TData, T>> Logger { get; }

        private DbSet<TData> DbSet => Context.Set<TData>();

        /// <summary>
        /// Gets an Entity Framework query with related entities included. Can be overridden to specify related entities to include for specific types.
        /// </summary>
        protected virtual IQueryable<TData> DbSetWithRelatedEntities => DbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="EFRepository{TData, T}"/> class.
        /// </summary>
        /// <param name="context">Entity Framework Core context.</param>
        /// <param name="mapper">Automapper instance.</param>
        /// <param name="logger">Logger for diagnostics.</param>
        public EFRepository(PuzzleDbContext context, IMapper mapper, ILogger<EFRepository<TData, T>> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maps an item of type T to its corresponding data model and adds it to the DB context.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>The item after being added.</returns>
        public virtual async Task<T> AddAsync(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var dataItem = Mapper.Map<TData>(item);
            var result = await DbSet.AddAsync(dataItem);
            await Context.SaveChangesAsync();
            Logger.LogInformation("Added item {Id} to database {EntityType}", result.Entity.Id, typeof(TData).Name);

            return Mapper.Map<T>(result.Entity);
        }

        /// <summary>
        /// Attemps to delete an item based on its ID.
        /// </summary>
        /// <param name="id">The ID of the item to delete.</param>
        /// <returns>True if the item was deleted, false if it did not exist.</returns>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var result = false;
            var entity = await DbSet.SingleOrDefaultAsync(t => t.Id == id);
            if (entity != null)
            {
                DbSet.Remove(entity);
                await Context.SaveChangesAsync();
                result = true;
            }

            Logger.LogInformation("Deleting entity {Id} from database {EntityType} {Result}", id, typeof(TData).Name, result ? "succeeded" : "failed");

            return result;
        }

        /// <summary>
        /// Gets a query for entities of type T.
        /// </summary>
        /// <returns>An IQueryable of all T in the DB context.</returns>
        public virtual IQueryable<T> Query()
        {
            Logger.LogInformation("Querying items from database {EntityType}", typeof(TData).Name);
            return Mapper.ProjectTo<T>(DbSetWithRelatedEntities);
        }

        /// <summary>
        /// Gets a filtered query for entities of type T.
        /// </summary>
        /// <param name="filter">Filter used to determine whether entities are returned in the IQueryable.</param>
        /// <returns>An IQueryable of all T in the DB context satisfying the filter.</returns>
        public virtual IQueryable<T> Query(Expression<Func<T, bool>>? filter)
        {
            Logger.LogInformation("Querying items from database {EntityType}", typeof(TData).Name);

            var query = DbSetWithRelatedEntities;
            if (filter != null)
            {
                // Use Mapper.MapExpression to convert the filter into a data model filter expression.
                var dataFilter = Mapper.MapExpression<Expression<Func<TData, bool>>>(filter);
                query = query.Where(dataFilter);
            }

            return Mapper.ProjectTo<T>(query);
        }

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity with the specified ID, or null if no such entity exists.</returns>
        public virtual async Task<T?> GetAsync(int id)
        {
            var entity = await DbSetWithRelatedEntities.SingleOrDefaultAsync(t => t.Id == id);
            Logger.LogInformation("Retrieving entity {Id} from database {EntityType} {Result}", id, typeof(TData).Name, entity == null ? "failed" : "succeeded");
            return Mapper.Map<T>(entity);
        }

        /// <summary>
        /// Updates an entity in the DB context.
        /// </summary>
        /// <param name="item">The entity to be updated, with updated properties.</param>
        /// <returns>The updated entity, or null if no entity with the given ID exists.</returns>
        public virtual async Task<T?> UpdateAsync(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            TData? ret = null;
            var entity = await DbSetWithRelatedEntities.SingleOrDefaultAsync(t => t.Id == item.Id);
            if (entity != null)
            {
                Mapper.Map(item, entity);
                var result = DbSet.Update(entity);
                await Context.SaveChangesAsync();
                ret = result.Entity;
            }

            Logger.LogInformation("Updating entity {Id} from database {EntityType} {Result}", item.Id, typeof(T).Name, ret != null ? "succeeded" : "failed");
            return Mapper.Map<T>(ret);
        }
    }
}
