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
			_context.Categories.Update(category);
		}
	}
}
