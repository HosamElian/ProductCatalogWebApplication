using ProductCatalog.DataAccess.Data;
using ProductCatalog.DataAccess.Repository.IRepository;
using ProductCatalog.Models.Entity;

namespace ProductCatalog.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(db);
            Product = new ProductRepository(db);
			ShoppingCart = new ShoppingCartRepository(db);

		}

        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }

        public bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }
    }

}
