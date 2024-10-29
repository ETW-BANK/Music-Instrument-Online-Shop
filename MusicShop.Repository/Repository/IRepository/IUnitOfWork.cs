using ClothShop.DataAccess.Repository.IRepository;

namespace MusicShop.Repository.IRepository
{
  public interface IUnitOfWork
  {
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }

    void Save();
  }
}
