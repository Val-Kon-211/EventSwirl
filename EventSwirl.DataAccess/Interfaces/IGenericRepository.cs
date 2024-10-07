using System.Linq.Expressions;

namespace EventSwirl.DataAccess.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Query();

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        public Task<TEntity> GetById(int id);

        public Task<IQueryable<TEntity>> GetAll();

        public Task Insert(TEntity entity);

        public Task Delete(TEntity entityToDelete);

        public Task Update(TEntity entityToUpdate);
    }
}
