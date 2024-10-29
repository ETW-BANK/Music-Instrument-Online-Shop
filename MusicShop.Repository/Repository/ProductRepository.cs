using ClothShop.DataAccess.Repository.IRepository;
using MusicShop.Data.Access.Data;
using MusicShop.Models;
using MusicShop.Repository.Rpository;

namespace ClothShop.DataAccess.Repository
{
  public class ProductRepository : Repository<Product>, IProductRepository
  {
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
      _context = context;
    }

    public void Upadate(Product product)
    {
      _context.Products.Update(product);
    }
  }
}