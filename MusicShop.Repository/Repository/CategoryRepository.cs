using MusicShop.Data.Access.Data;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicShop.Repository.Rpository
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		private readonly ApplicationDbContext _context;

		public CategoryRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}


		public void Update(Category category)
		{
			var objFromDb = _context.Categories.FirstOrDefault(c => c.Id == category.Id);

			if (objFromDb != null)
			{
				objFromDb.Name = category.Name;
				objFromDb.DisplayOrder = category.DisplayOrder;
				if(category.ImageUrl != null)
				{
					objFromDb.ImageUrl = category.ImageUrl;
				}
			}
	
		}
	}
}
