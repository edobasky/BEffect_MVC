using BEffectWeb.DataAccess.Data;
using BEffectWeb.DataAccess.Repository.Interface;
using BEffectWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BEffectWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objCategoryList = await _unitOfWork.Category.GetAll();
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
                _unitOfWork.Category.Add(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Category? categoryFromDb = await _unitOfWork.Category.GetByCondition(u => u.Id == id);
            if (categoryFromDb == null) return NotFound();

            return View(categoryFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Category? categoryFromDb = await _unitOfWork.Category.GetByCondition(u => u.Id == id);
            if (categoryFromDb == null) return NotFound();

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePPOST(int? id)
        {
            Category? obj = await _unitOfWork.Category.GetByCondition(u => u.Id == id);
            if (obj == null) return NotFound();

            _unitOfWork.Category.Remove(obj);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");


        }
    }
}
