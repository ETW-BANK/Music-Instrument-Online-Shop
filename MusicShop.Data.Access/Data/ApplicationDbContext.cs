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

                new Category { Id = 1, Name = "String Instruments", DisplayOrder = 1, ImageUrl = "/image/string.jpeg" },
                 new Category { Id = 2, Name = "Percussion Instruments", DisplayOrder = 2, ImageUrl = "/image/Per.jpeg" },
                  new Category { Id = 3, Name = "Keyboard Instruments", DisplayOrder = 3 , ImageUrl = "/image/key.jpeg" },
                   new Category { Id = 4, Name = "Wind Instruments", DisplayOrder = 4 , ImageUrl = "/image/win.jpeg" },
                    new Category { Id = 5, Name = "Folk & Ethnic Instruments", DisplayOrder = 5, ImageUrl = "/image/folk.jpeg" },
                   
                    new Category { Id = 6, Name = "Recording & Studio Gear", DisplayOrder = 7, ImageUrl = "/image/rec.jpeg" },
                    new Category { Id = 7, Name = "Pro Audio Equipment", DisplayOrder = 8, ImageUrl = "/image/pro.jpeg" },
                    new Category { Id = 8, Name = "Accessories & Gear", DisplayOrder = 9, ImageUrl = "/image/acc.jpeg" },
                     new Category { Id = 9, Name = "Bundles & Deals", DisplayOrder = 10, ImageUrl = "/image/band.jpeg" }



            );

        }
    }
}