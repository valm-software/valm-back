
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression , bool noTracking = true)
        {
            return noTracking ? _valmContext.Set<T>().AsNoTracking().Where(expression)
                                :_valmContext.Set<T>().Where(expression);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(bool noTracking = true)
        {
            return noTracking ? await _valmContext.Set<T>().AsNoTracking().ToListAsync()
                                : await _valmContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id, bool noTracking = true)
        {
            var entity = await _valmContext.Set<T>().FindAsync(id);

            if (noTracking)
            {
                _valmContext.Entry(entity).State = EntityState.Detached;
            }

            return entity;            
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

        public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize , string search , bool noTracking = true)
        {
            var query = noTracking ? _valmContext.Set<T>().AsNoTracking().AsQueryable()
                                    :_valmContext.Set<T>().AsQueryable();
                                      

            var totalRegistros = await query
                                    .CountAsync();

            var registros = await query
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            return (totalRegistros, registros);
                                   
        }


    }
}
