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
    public class CategoryRepositoryImp : RepositoryImp<Category>, ICategoryRepository
    {
        private readonly AppDbContext _appDb;

        public CategoryRepositoryImp(AppDbContext appDb) : base(appDb)
        {
             _appDb = appDb;
        }

        public void Update(Category category)
        {
            _appDb.Categories.Update(category);
        }
    }
}
