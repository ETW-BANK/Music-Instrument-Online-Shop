using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.Utility;
using MusicShop.ViewModels;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace Music_Instrumet_Online_Shop.Areas.Admin.Controllers
{

    [Area("admin")]
    [Authorize]
    public class OrderController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVm OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId,includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };

            return View(OrderVM);
        }


        [HttpPost]
        [Authorize(Roles = StaticData.RoleAdmin)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Successfully.";


            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
        }


        [HttpPost]
        [Authorize(Roles = StaticData.RoleAdmin)]

        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, StaticData.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "Order is Processing";

            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }
        [HttpPost]
        [Authorize(Roles = StaticData.RoleAdmin)]

        public IActionResult ShipOrder()
        {

            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = StaticData.StatusShipped;
            orderHeader.OrderDate = DateTime.Now;

            if (orderHeader.paymentStatus == StaticData.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();


            TempData["success"] = "Order is Shipped Succesfully";

            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }


        [HttpPost]
        [Authorize(Roles = StaticData.RoleAdmin)]

        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            if (orderHeader.paymentStatus == StaticData.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, StaticData.StatusCancelled, StaticData.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, StaticData.StatusCancelled);
            }

            _unitOfWork.Save();
            TempData["success"] = "order cancelled Sucessfully";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }
        [ActionName("Details")]
        [HttpPost]
        public IActionResult Details_PAY_NOW()
        {
            OrderVM.OrderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");

            OrderVM.OrderDetail = _unitOfWork.OrderDetail
                .GetAll(u => u.OrderHeaderId == OrderVM.OrderHeader.Id, includeProperties: "Product");

           
            if (OrderVM.OrderDetail == null || !OrderVM.OrderDetail.Any())
            {
                ModelState.AddModelError("", "No order details found.");
                return View(OrderVM); 
            }

            // Stripe logic
            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in OrderVM.OrderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), 
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }

            // Ensure LineItems is not empty before proceeding
            if (!options.LineItems.Any())
            {
                ModelState.AddModelError("", "No line items to process for payment.");
                return View(OrderVM); // Return to the view with an error
            }

            var service = new SessionService();
            Session session = service.Create(options);

            // Update the OrderHeader with Stripe payment details
            _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            // Redirect to the Stripe URL
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }


        public ActionResult PaymentConfirmation(int orderHeaderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderHeaderId);
            if (orderHeader.paymentStatus == StaticData.PaymentStatusDelayedPayment)
            {
                //company oredr
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderId, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, StaticData.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }


            return View(orderHeaderId);
        }



        #region API CALLS

        [HttpGet]


        public IActionResult GetALL(string status)
        {

            IEnumerable<OrderHeader> objOrderHeaders;


            if (User.IsInRole(StaticData.RoleAdmin))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

            }
            else
            {

                var claimsIdentity=(ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objOrderHeaders=_unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId == userId,includeProperties:"ApplicationUser");
            }
            switch (status)
            {
                case "pending":
                  objOrderHeaders=objOrderHeaders.Where(u=>u.paymentStatus==StaticData.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.paymentStatus == StaticData.StatusInProcess);
                    break;
                  
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.paymentStatus == StaticData.StatusShipped);
                    break;
                 
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.paymentStatus == StaticData.StatusApproved);
                    break;
                
                default:
                   
                    break;

            }

         
            return Json(new { data = objOrderHeaders });
        }

        #endregion

    }
}
