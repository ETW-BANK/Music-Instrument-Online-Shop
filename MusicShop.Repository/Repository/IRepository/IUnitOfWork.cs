using ClothShop.DataAccess.Repository.IRepository;
using MusicShop.Repository.Repository.IRepository;

namespace MusicShop.Repository.IRepository
{
  public interface IUnitOfWork
  {
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    ICompanyRepository Company { get; }

        void Save();
  }
}
