using ProductCatalog.Models.Entity;
using System.Linq.Expressions;

namespace ProductCatalog.DataAccess.Repository.IRepository
{
	public interface IProductRepository
	{
		void Add(Product product);
		Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>>? filter = null, bool isUser = false, int? categoryId = 0);
		Task<Product> GetAsync(Expression<Func<Product, bool>>? filter = null, bool tracked = true, int? categoryId = 0);
		Task Remove(Product product);
		void Update(Product product);

	}
}
