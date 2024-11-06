using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicShop.Data.Access.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Companies> Companies { get; set; } 

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //this is category list
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(

                new Category { Id = 1, Name = "String Instruments", DisplayOrder = 1, ImageUrl = "/image/string.jpg" },
                 new Category { Id = 2, Name = "Percussion Instruments", DisplayOrder = 2, ImageUrl = "/image/Per.jpg" },
                  new Category { Id = 3, Name = "Keyboard Instruments", DisplayOrder = 3 , ImageUrl = "/image/key.jpg" },
                   new Category { Id = 4, Name = "Wind Instruments", DisplayOrder = 4 , ImageUrl = "/image/win.jpg" },
                    new Category { Id = 5, Name = "PA Systems", DisplayOrder = 5, ImageUrl = "/image/pa.jpg" },
                    new Category { Id = 6, Name = "Recording ", DisplayOrder = 7, ImageUrl = "/image/rec.jpg" },
                    new Category { Id = 7, Name = "Electric", DisplayOrder = 8, ImageUrl = "/image/el.jpg" },
                    new Category { Id = 8, Name = "Accessories ", DisplayOrder = 9, ImageUrl = "/image/acc.jpg" },
                     new Category { Id = 9, Name = "DJ Equipments", DisplayOrder = 10, ImageUrl = "/image/dj.jpg" }



            );

        }
    }
}