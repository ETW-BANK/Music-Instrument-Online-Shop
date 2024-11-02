using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.Utility;

namespace Music_Instrumet_Online_Shop.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles =StaticData.RoleAdmin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {

                ModelState.AddModelError("name", "The Display Order Can't Match The Name");

            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Created successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? objToEdit = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (objToEdit == null)
            {
                return NotFound();
            }

            return View(objToEdit);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {


            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated successfully";
                return RedirectToAction("Index");
            }

            return View();
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
