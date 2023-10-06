using Microsoft.EntityFrameworkCore;
using ProductCatalog.DataAccess.Data;
using ProductCatalog.Models.Entity;
using ProductCatalog.Models.ViewModels;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductCatalog.DataAccess.Repository.IRepository
{
    public class ShoppingCartRepository : IShoppingCartRepository
	{
		private readonly ApplicationDbContext _db;

		public ShoppingCartRepository(ApplicationDbContext db) 
		{
			_db = db;
		}

		public bool Add(ShoppingCart shoppingCart)
		{
			var success = false;
			var product = _db.Products.SingleOrDefault(p => p.Id == shoppingCart.ProductId);
			if (product != null)
			{
                if (product.Quantity - shoppingCart.Count >= 0)
                {
                    _db.ShoppingCarts.Add(shoppingCart);
                    success = true;
                }
            }
			return success;
		}

		public async Task<IEnumerable<ShoppingCart>> GetAllAsync(Expression<Func<ShoppingCart, bool>> filter, bool IncludeConfirmed = false)
		{
			IQueryable<ShoppingCart> query = _db.ShoppingCarts.AsNoTracking();

			if (IncludeConfirmed) { query = query.Where(s => !s.IsConfirmed); }
			if (filter != null) { query = query.Where(filter); }
			
			return await query.Include(s=> s.Product).ToListAsync();
		}

		public async Task<ShoppingCart> GetAsync(Expression<Func<ShoppingCart, bool>> filter, bool tracked = true)
		{
			IQueryable<ShoppingCart> query = _db.ShoppingCarts;

			if (!tracked) { query = query.AsNoTracking(); }

			return await query.Include(s => s.Product).SingleOrDefaultAsync(filter);
		}
        public async Task Remove(ShoppingCart shoppingCart)
        {
            var shoppingCartFromDB = await GetAsync(s => s.Id == shoppingCart.Id);
            shoppingCartFromDB.IsDeleted = true;
        }

        public bool IncrementCount(ShoppingCart shoppingCart, int Count)
		{
            var success = false;
			if (shoppingCart.Product.Quantity <= 0)
			{
				shoppingCart.IsDeleted = true;
				return success;
			}

			if (shoppingCart.Product.Quantity - shoppingCart.Count >= 0)
            {
				shoppingCart.Product.Quantity -= shoppingCart.Count;
                shoppingCart.Count += Count;
                success = true;
            }
            return success;
        }
		public void DecrementCount(ShoppingCart shoppingCart, int Count)
		{
            shoppingCart.Count -= Count;

		}
        
        public  void Confirm(string id)
        {
			var shoppingCartsFromDB =  _db.ShoppingCarts.Where(s => s.IdentityUserId == id).Include(p=>p.Product).ToList();
			if(shoppingCartsFromDB.Count() > 0)
			{
				shoppingCartsFromDB.ForEach(s =>
				{
                    s.IsConfirmed = true;
					
                });
			}

        }
    }
}