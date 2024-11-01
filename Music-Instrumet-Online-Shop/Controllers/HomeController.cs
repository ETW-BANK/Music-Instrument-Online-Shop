using Microsoft.AspNetCore.Mvc;
using Music_Instrumet_Online_Shop.Models;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Music_Instrumet_Online_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories=_unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Instruments(int categoryId)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == categoryId, includeProperties: "Products");
            if (category == null) return NotFound();

            
            if (category.Products == null || !category.Products.Any())
            {
                category.Products = _unitOfWork.Product.GetAll()
                    .Where(i => i.CategoryId == categoryId)
                    .ToList();
            }

            return View(category.Products); 
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
