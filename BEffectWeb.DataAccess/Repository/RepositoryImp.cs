using BEffectWeb.DataAccess.Data;
using BEffectWeb.DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BEffectWeb.DataAccess.Repository
{
    public abstract class RepositoryImp<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _appDb;

        public RepositoryImp(AppDbContext appDb)
        {
            _appDb = appDb;
        }

        public void Add(T entity)
        {
            _appDb.Set<T>().Add(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IQueryable<T> query = _appDb.Set<T>();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = _appDb.Set<T>();
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetByCondition(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _appDb.Set<T>().Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> GetByCondition(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = _appDb.Set<T>().Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> GetById(int id)
        {
           return await _appDb.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _appDb.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _appDb.Set<T>().RemoveRange(entities);
        }
    }
}
