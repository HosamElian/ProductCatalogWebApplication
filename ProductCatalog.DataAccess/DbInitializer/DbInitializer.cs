using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.DataAccess.Data;
using ProductCatalog.DataAccess.DbInitializer;
using ProductCatalog.Utility;

namespace BulkyBook.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db; 

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            // Migrations
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {


            }
            // Create Roles if Not Created
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();
            }
            // Create Admin User
            _userManager.CreateAsync(new IdentityUser
            {
                UserName = "Admin@gmail.com",
                Email = "Admin@gmail.com",
                PhoneNumber = "01004014619",
                EmailConfirmed = true,
            }, "Admin123*").GetAwaiter().GetResult();

            IdentityUser user = _db.Users.FirstOrDefault(u => u.Email == "Admin@gmail.com");
            if (user != null)
            {
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
