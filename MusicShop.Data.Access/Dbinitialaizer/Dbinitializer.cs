using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicShop.Data.Access.Data;
using MusicShop.Models;
using MusicShop.Utility;

namespace MusicShop.Data.Access.Dbinitialaizer
{
    public class Dbinitializer : IDbintialaizer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public Dbinitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }
        public void Initialize()
        {
            try
            {
                if (_applicationDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _applicationDbContext.Database.Migrate();
                }

            }
            catch (Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(StaticData.RoleCustomer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticData.RoleCustomer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticData.RoleAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticData.RoleCompany)).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@404notfound.com",
                    Email = "admin@404notfound.com",
                    Name = "Tense Girma",
                    StreetAddress = "Ellinsborgsbacken 22",
                    State = "Spånga",
                    PostalCode = "16364",
                    City = "Stockholm"
                }, "@Admin12345").GetAwaiter().GetResult();

                ApplicationUser user = _applicationDbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@404notfound.com");
                _userManager.AddToRoleAsync(user, StaticData.RoleAdmin).GetAwaiter().GetResult();
            }

            return;
        }

    }

}