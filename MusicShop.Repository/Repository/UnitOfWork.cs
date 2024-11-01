using ClothShop.DataAccess.Repository;
using ClothShop.DataAccess.Repository.IRepository;
using MusicShop.Data.Access.Data;
using MusicShop.Repository.IRepository;
using MusicShop.Repository.Repository;
using MusicShop.Repository.Repository.IRepository;

namespace MusicShop.Repository.Rpository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Product = new ProductRepository(_context);
            Company = new CompanyRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
