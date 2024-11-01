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
    public class CompanyRepository: Repository<Companies>,ICompanyRepository
    {

      public readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public void Upadate(Companies companies)
        {
            _context.Companies.Update(companies);
        }
    }
}
