using BEffectWeb.DataAccess.Data;
using BEffectWeb.DataAccess.Repository.Interface;
using BEffectWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEffectWeb.DataAccess.Repository
{
    public class ProductRepositoryImp : RepositoryImp<Product>, IProductRepository
    {
        private readonly AppDbContext _appDb;

        public ProductRepositoryImp(AppDbContext appDb) : base(appDb)
        {
            _appDb = appDb;
        }

        public void Update(Product product)
        {
           _appDb.Products.Update(product);
        }
    }
}
