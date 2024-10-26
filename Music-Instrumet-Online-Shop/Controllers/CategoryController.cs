using Microsoft.AspNetCore.Mvc;

namespace Music_Instrumet_Online_Shop.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
