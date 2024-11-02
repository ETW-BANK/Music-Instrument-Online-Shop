using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.Utility;
using MusicShop.ViewModels;
using System.Security.Claims;

namespace Music_Instrumet_Online_Shop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }


        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
               
            };

            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                double price=GetTotalPrice(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (price * cart.Count);
            }

          
            return View(ShoppingCartVM);
        }
        public IActionResult Plus(int cartId)
        {
            var chartfromdb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            chartfromdb.Count += 1;
            _unitOfWork.ShoppingCart.Update(chartfromdb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var chartfromdb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId, tracked: true);

            if (chartfromdb.Count <= 1)
            {
                HttpContext.Session.SetInt32(StaticData.SessionCart, _unitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == chartfromdb.ApplicationUserId).Count() - 1);

                _unitOfWork.ShoppingCart.Remove(chartfromdb);

            }
            else
            {
                chartfromdb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(chartfromdb);
            }


            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var chartfromdb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId, tracked: true);
            HttpContext.Session.SetInt32(StaticData.SessionCart, _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == chartfromdb.ApplicationUserId).Count() - 1);

            _unitOfWork.ShoppingCart.Remove(chartfromdb);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary () 
        
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()

            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id == userId);  

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Product.Price = (double)GetTotalPrice(cart);
                ShoppingCartVM.OrderHeader.OrderTotal = (cart.Product.Price * cart.Count);
            }
            return View(ShoppingCartVM);

        }

        private double GetTotalPrice(ShoppingCart shoppingCart)
        {



            return shoppingCart.Product.Price;


        }
    }
}
