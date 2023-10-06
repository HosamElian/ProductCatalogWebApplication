using ProductCatalog.Models.Entity;
using System.Linq.Expressions;

namespace ProductCatalog.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category> GetAsync(Expression<Func<Category, bool>>? filter = null, bool tracked = true);
        Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? filter = null, bool tracked = true);
    }
}
