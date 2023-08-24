
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ValmContext _valmContext;

        public GenericRepository(ValmContext valmContext)
        {
            _valmContext = valmContext;
        }
        public virtual void Add(T entity)
        {
            _valmContext.Set<T>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _valmContext.Set<T>().AddRange(entities);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _valmContext.Set<T>().Where(expression);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _valmContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _valmContext.Set<T>().FindAsync(id);
        }

        public virtual void Remove(T entity)
        {
            _valmContext.Set<T>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _valmContext.Set<T>().RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            _valmContext.Set<T>()
                .Update(entity);
        }

    }
}
