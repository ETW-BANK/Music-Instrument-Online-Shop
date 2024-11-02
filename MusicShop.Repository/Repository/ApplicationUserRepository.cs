using MusicShop.Data.Access.Data;
using MusicShop.Models;
using MusicShop.Repository.Repository.IRepository;
using MusicShop.Repository.Rpository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicShop.Repository.Repository
{
    public class ApplicationUserRepository:Repository<ApplicationUser>,IApplicationUserRepository
    {

        private readonly ApplicationDbContext _context; 

        public ApplicationUserRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
    }
}
