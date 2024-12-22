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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<SelectListItem> selectLists = new();

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> objCategoryList = await _unitOfWork.Product.GetAll(includeProperties:"Category");

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

        public async Task<IActionResult> Upsert(int? id)
        {
            var catList = await GetDropDown();

            ProductVM productVM = new ProductVM()
            {
                CategoryList = catList,
                Product = new Product()
            };

            if (id is null || id == 0)
            {
                return View(productVM);
            }else
            {
                var GetProduct = await _unitOfWork.Product.GetByCondition(u => u.Id == id);
                if (GetProduct is null) return NotFound();
                productVM.Product = GetProduct;

                return View(productVM);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> UpSert(ProductVM obj,IFormFile? file)
        {

            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        // delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));


                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);    
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    obj.Product.ImageUrl = @"\images\product\" + fileName;
                }


                if (obj.Product.Id == 0)
                {

                    _unitOfWork.Product.Add(obj.Product);
                }else
                {

                    _unitOfWork.Product.Update(obj.Product);
                }

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

      /*  public async Task<IActionResult> Edit(int? id)
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

        }*/

      /*  public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Product? productFromDb = await _unitOfWork.Product.GetByCondition(u => u.Id == id);
            if (productFromDb == null) return NotFound();

            return View(productFromDb);
        }*/

       /* [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            Product? obj = await _unitOfWork.Product.GetByCondition(u => u.Id == id);
            if (obj == null) return NotFound();

            _unitOfWork.Product.Remove(obj);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "pRODUCT deleted successfully";
            return RedirectToAction("Index");


        }*/


        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Product> objProductList = await _unitOfWork.Product.GetAll(includeProperties: "Category");

            return Json(new {data = objProductList});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var productToBeDeleted = await _unitOfWork.Product.GetByCondition(u =>u.Id == id);  

            if (productToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }
                      // delete the old image
                  var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));


            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
