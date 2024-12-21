using BEffectWeb.DataAccess.Data;
using BEffectWeb.DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEffectWeb.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public ICategoryRepository Category { get; private set; }

       public IProductRepository Product { get; private set; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Category = new CategoryRepositoryImp(_dbContext);
            Product = new ProductRepositoryImp(_dbContext);
        }

       

        public async Task SaveAsync()
        {
           await _dbContext.SaveChangesAsync();
        }
    }
}
