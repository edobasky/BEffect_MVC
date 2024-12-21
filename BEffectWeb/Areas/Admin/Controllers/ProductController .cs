using BEffectWeb.DataAccess.Repository.Interface;
using BEffectWeb.Models;
using BEffectWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BEffectWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SelectListItem> selectLists = new();

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> objCategoryList = await _unitOfWork.Product.GetAll();

            return View(objCategoryList);
        }

        public async Task<IEnumerable<SelectListItem>> GetDropDown()
        {
            IEnumerable<SelectListItem> CategoryList = (await _unitOfWork.Category.GetAll()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

         //   selectLists = CategoryList.ToList();
            return CategoryList.ToList();
        }

        public async Task<IActionResult> Create()
        {
            var catList =  await GetDropDown();

            ProductVM productVM = new ProductVM()
            {
                CategoryList = catList,
                Product = new Product()
            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                obj.CategoryList = await GetDropDown();
                return View(obj);
            }
           

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Product? productFromDb = await _unitOfWork.Product.GetByCondition(u => u.Id == id);
            if (productFromDb == null) return NotFound();

            return View(productFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Product? productFromDb = await _unitOfWork.Product.GetByCondition(u => u.Id == id);
            if (productFromDb == null) return NotFound();

            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            Product? obj = await _unitOfWork.Product.GetByCondition(u => u.Id == id);
            if (obj == null) return NotFound();

            _unitOfWork.Product.Remove(obj);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "pRODUCT deleted successfully";
            return RedirectToAction("Index");


        }
    }
}
