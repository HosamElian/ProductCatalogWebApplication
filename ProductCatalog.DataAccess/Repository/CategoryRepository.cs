using Microsoft.EntityFrameworkCore;
using ProductCatalog.DataAccess.Data;
using ProductCatalog.DataAccess.Repository.IRepository;
using ProductCatalog.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category> GetAsync(Expression<Func<Category, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Category> query = _db.Categories;

            if (!tracked) { query = query.AsNoTracking(); }

            return await query.SingleOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Category> query = _db.Categories;

            if (filter != null) { query = query.Where(filter); }
            if (!tracked) { query = query.AsNoTracking(); }

            return await query.ToListAsync();
        }
    }
}
