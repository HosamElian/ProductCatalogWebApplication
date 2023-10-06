using Microsoft.EntityFrameworkCore;
using ProductCatalog.DataAccess.Data;
using ProductCatalog.DataAccess.Repository.IRepository;
using ProductCatalog.Models.Entity;
using System.Linq.Expressions;

namespace ProductCatalog.DataAccess.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _db;
		public ProductRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public void Add(Product product)
		{
			_db.Products.Add(product);
		}

		public async Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>>? filter = null, bool isUser = false, int? categoryId = 0)
		{
			IQueryable<Product> query = _db.Products.AsNoTracking();

			if (filter != null) { query = query.Where(filter); }
			if(categoryId != null && categoryId != 0) { query = query.Where(p => p.CategoryId == categoryId);  }
			
			if (isUser)
			{
                query = query.Where(p => p.StartDate.AddDays(p.DurationInDays) >= DateTime.UtcNow);
            }
            return await query.Include(p => p.Category).ToListAsync();
		}

		public async Task<Product> GetAsync(Expression<Func<Product, bool>>? filter = null, bool tracked = true, int? categoryId = 0)
		{
			IQueryable<Product> query = _db.Products;

			if (!tracked) { query = query.AsNoTracking(); }
			if (categoryId != null && categoryId != 0) { query = query.Where(p => p.CategoryId == categoryId); }

			return await query.Include(p => p.Category).SingleOrDefaultAsync(filter);
		}

		public async void Update(Product product)
		{
			var productFromDB = await GetAsync(p => p.Id == product.Id);

			productFromDB.DurationInDays = product.DurationInDays;
			productFromDB.CategoryId = product.CategoryId;
			productFromDB.Name = product.Name;
			productFromDB.Price = product.Price;

			if (product.ImageUrl != null)
			{
				productFromDB.ImageUrl = product.ImageUrl;
			}

		}
		public async Task Remove(Product product)
		{
			var productFromDB = await GetAsync(p => p.Id == product.Id);
			productFromDB.IsDeleted = true;
		}
	}
}
