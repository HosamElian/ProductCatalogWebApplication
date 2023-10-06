using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DataAccess.Repository.IRepository;
using ProductCatalog.Utility;
using System.Security.Claims;

namespace ProductCatalogWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    var shoppingCartList = await _unitOfWork.ShoppingCart.GetAllAsync(u => u.IdentityUserId == claims.Value);
                    var count = shoppingCartList.Count();
                    HttpContext.Session.SetInt32(SD.SessionCart, count);

                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            HttpContext.Session.Clear();
            return View(0);
        }
    }
}
