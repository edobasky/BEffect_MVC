using BEffectWeb.Data;
using BEffectWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BEffectWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDb;

        public CategoryController(AppDbContext appDb)
        {
            _appDb = appDb;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> objCategoryList = await _appDb.Categories.ToListAsync();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
