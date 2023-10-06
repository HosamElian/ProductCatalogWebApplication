using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductCatalog.Models.Entity;
using System.Linq.Expressions;

namespace ProductCatalog.DataAccess.Repository.IRepository
{
	public interface IShoppingCartRepository 
	{
		bool Add(ShoppingCart shoppingCart);
        Task<ShoppingCart> GetAsync(Expression<Func<ShoppingCart, bool>> filter, bool  tracked = true);
		Task<IEnumerable<ShoppingCart>> GetAllAsync(Expression<Func<ShoppingCart, bool>> filter, bool IncludeConfirmed = false);
        Task Remove(ShoppingCart shoppingCart);
        bool IncrementCount(ShoppingCart shoppingCart, int Count);
        void DecrementCount(ShoppingCart shoppingCart, int Count);
        void Confirm(string id);
    }
}
