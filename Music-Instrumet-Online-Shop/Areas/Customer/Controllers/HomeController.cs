using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music_Instrumet_Online_Shop.Models;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.Utility;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace Music_Instrumet_Online_Shop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
           // var claimsIdentity = (ClaimsIdentity)User.Identity;
           // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

           // if (claim != null)
           // {
           //     HttpContext.Session.SetInt32(StaticData.SessionCart,
           //_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
           // }
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll().ToList();
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

        public IActionResult Details(int ProductId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == ProductId, includeProperties: "Category"),
                Count = 1,
                ProductId = ProductId
            };
            return View(cart);

        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
          
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId= userId;

            ShoppingCart cartFromdb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);
          
            if(cartFromdb != null) 
            {
                cartFromdb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromdb);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart); 
            }

            _unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(StaticData.SessionCart,
            _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId==userId).Count());
           
            TempData["success"] = $"{shoppingCart.Count} , Item/Items Added To Shopping Cart Succesfully";

            return RedirectToAction("Index");   
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
