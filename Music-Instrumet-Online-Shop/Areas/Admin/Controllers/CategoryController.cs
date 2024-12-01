using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.Utility;
using MusicShop.ViewModels;

namespace Music_Instrumet_Online_Shop.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles =StaticData.RoleAdmin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {

            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Upsert(int? id)
        {
            CategoryVM categoryVM = new()
            {
                Category = new Category()
            };

            if(id==null || id == 0)
            {
                return View(categoryVM);
            }
            else
            {
                categoryVM.Category=_unitOfWork.Category.GetFirstOrDefault(u=>u.Id==id);   
                return View(categoryVM);    
            }

        }

        [HttpPost]
        public IActionResult Upsert(CategoryVM categoryVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string categoryPath = Path.Combine(wwwRootPath, "image", "category");


                    if (!string.IsNullOrEmpty(categoryVM.Category.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, categoryVM.Category.ImageUrl.TrimStart('/').Replace("/", "\\"));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                                    
                    using (var fileStream = new FileStream(Path.Combine(categoryPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                   
                    categoryVM.Category.ImageUrl = $"/image/category/{fileName}";
                }

                if (categoryVM.Category.Id == 0)
                {
                    _unitOfWork.Category.Add(categoryVM.Category);
                }
                else
                {
                    _unitOfWork.Category.Update(categoryVM.Category);
                }

                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }

            return View(categoryVM);
        }




        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? objToDelete = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (objToDelete == null)
            {
                return NotFound();
            }

            return View(objToDelete);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {


            Category? obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)

            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");

        }



    }
}
