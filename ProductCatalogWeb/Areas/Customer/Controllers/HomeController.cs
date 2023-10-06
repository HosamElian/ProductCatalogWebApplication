using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DataAccess.Repository.IRepository;
using ProductCatalog.Models;
using ProductCatalog.Models.Entity;
using ProductCatalog.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace ProductCatalogWeb.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Product.GetAllAsync(isUser:true);
            return View(products);
        }
        [Authorize]
        public IActionResult Privacy()
        {

            return View();
        }
        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ShoppingCart CartObj = new()
            {
                Count = 1,
                ProductId = productId,
                Product = await _unitOfWork.Product.GetAsync(u => u.Id == productId)
            };
            return View(CartObj);
        }
        //Post
		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.IdentityUserId = claim.Value;

            ShoppingCart cartFromDb = await _unitOfWork.ShoppingCart.GetAsync(
                u => u.IdentityUserId == claim.Value && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
            }
            _unitOfWork.SaveChanges();
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAllAsync(u => u.IdentityUserId == claim.Value).Result.Count());

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}