using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BEffectWeb.DataAccess.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        // Below is the generic form of writing first or default,inother words passing a filter linq by condition
        Task<T?> GetByCondition(Expression<Func<T,bool>> filter);
        void Add(T entity);
       // void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
