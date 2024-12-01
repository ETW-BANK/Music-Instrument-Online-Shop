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
      var objFromdb=_context.Products.FirstOrDefault(c=>c.Id == product.Id);    
        if (objFromdb != null)
            {
                objFromdb.Title = product.Title;
                objFromdb.Description = product.Description;
                objFromdb.Price = product.Price;
                objFromdb.CategoryId = product.CategoryId;
                if(product.ImageUrl != null)
                {
                    objFromdb.ImageUrl = product.ImageUrl;  
                }
            }
    }
  }
}