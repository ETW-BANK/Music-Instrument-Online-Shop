using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicShop.Data.Access.Data;
using MusicShop.Models;
using MusicShop.Utility;
using MusicShop.ViewModels;

namespace Music_Instrumet_Online_Shop.Areas.Admin.Controllers
{

    [Area("Admin")]

    [Authorize(Roles =StaticData.RoleAdmin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult RoleManagment(string userId)
        {
            string RoleID = _context.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;
            RoleManagmentVM RoleVm = new RoleManagmentVM()
            {
                ApplicationUser = _context.ApplicationUsers.Include(u => u.Companies).FirstOrDefault(u => u.Id == userId),
                RoleList = _context.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name

                }),
                CompanyList = _context.Companies.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

            };

            RoleVm.ApplicationUser.Role = _context.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

            return View(RoleVm);
        }

        [HttpPost]
        public IActionResult RoleManagment(RoleManagmentVM roleManagmentVM)
        {
            string RoleID = _context.UserRoles.FirstOrDefault(u => u.UserId == roleManagmentVM.ApplicationUser.Id).RoleId;

            string oldRole = _context.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

            if (!(roleManagmentVM.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagmentVM.ApplicationUser.Id);

                if (roleManagmentVM.ApplicationUser.Role == StaticData.RoleCompany)
                {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                }

                if (oldRole == StaticData.RoleCompany)
                {
                    applicationUser.CompanyId = null;
                }

                _context.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }


            return RedirectToAction("Index");
        }




        #region ApiCall
        [HttpGet]
        public IActionResult GetAll()
        {

            List<ApplicationUser> userList = _context.ApplicationUsers.Include(u => u.Companies).ToList();

            var userRoles = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;

                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (user.Companies == null)
                {
                    user.Companies = new Companies() { Name = "" };
                }
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var userFromDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

            if (userFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unloacking User" });
            }
            if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
            {
                userFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _context.SaveChanges();

            return Json(new { success = true, message = "Operation Successfull" });
        }





        #endregion
    }
}
