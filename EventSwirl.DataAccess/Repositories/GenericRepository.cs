using EventSwirl.DataAccess.Interfaces;
using EventSwirl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSwirl.DataAccess.Repositories
{
    public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity : DomainObject, new()
    {
        private readonly DataContext _context;
        private DbSet<TEntity> dbSet;

        public GenericRepository(DataContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query()
        {
            return dbSet;
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await dbSet.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IQueryable<TEntity>> GetAll()
        {
            return await Task.Run(() => dbSet).ConfigureAwait(false);
        }

        public async Task Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;

            await dbSet.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            await Task.Run(() => dbSet.Remove(entity)).ConfigureAwait(false);
        }

        public async Task Update(TEntity entity)
        {
            var existingEntity = await GetById(entity.Id).ConfigureAwait(false);

            if (existingEntity != null)
            {
                entity.UpdatedAt = DateTime.Now;
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
        }
    }
}
