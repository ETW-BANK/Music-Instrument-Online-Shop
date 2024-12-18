using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MusicShop.Data.Access.Data;
using MusicShop.Data.Access.Dbinitialaizer;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.Repository.Rpository;
using MusicShop.Utility;
using Stripe;
using System.Text.Json.Serialization;

namespace Music_Instrumet_Online_Shop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionstring = builder.Configuration.GetConnectionString("MusicShopDb");
            builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionstring));

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = $"/Identity/Account/Login";
                option.LogoutPath = $"/Identity/Account/Logout";
                option.AccessDeniedPath = $"/Identity/Account/AccessDenied";


            });
            //Session Service 

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => {

                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IDbintialaizer, Dbinitializer>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            app.MapRazorPages();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            SeedDatabase();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();

            void SeedDatabase()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbintialaizer>();

                    dbInitializer.Initialize();
                }
            }

        }
    }
}