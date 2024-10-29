using MusicShop.Models;
using MusicShop.Repository.IRepository;

namespace ClothShop.DataAccess.Repository.IRepository
{
  public interface IProductRepository : IRepository<Product>
  {
    void Upadate(Product product);
  }
}