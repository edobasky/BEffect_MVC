using BEffectWeb.Data;
using BEffectWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

        [HttpPost]
        public async Task<IActionResult> Create(Category obj)
        {

            if (ModelState.IsValid)
            {
                _appDb.Categories.Add(obj);
                await _appDb.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Category categoryFromDb = await _appDb.Categories.FindAsync(id);
            if (categoryFromDb == null) return NotFound();

            return View(categoryFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _appDb.Categories.Update(obj);
                await _appDb.SaveChangesAsync();
                  return RedirectToAction("Index");
            }
            return View();

        }
    }
}
