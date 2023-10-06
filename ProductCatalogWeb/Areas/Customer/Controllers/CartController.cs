using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DataAccess.Repository.IRepository;
using ProductCatalog.Models.ViewModels;
using ProductCatalog.Utility;
using System.Security.Claims;

namespace ProductCatalogWeb.Areas.Customer.Controllers
{
	[Area(nameof(Customer))]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;

		public ShoppingCartVM? ShoppingCartVM { get; set; }
		public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
		{
			_unitOfWork = unitOfWork;
			_emailSender = emailSender;
		}
		public async Task<IActionResult> Index()
		{
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var claims = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
			var cartItems = await _unitOfWork.ShoppingCart.GetAllAsync(u => u.IdentityUserId == claims.Value);
			var totalPrice = cartItems.Sum(x => x.Product.Price);
			var shoppingCartVM = new ShoppingCartVM()
			{
				TotalPrice = totalPrice,
				ListCart = cartItems.ToList()
			};

            return View(shoppingCartVM);
	}


		public async Task<IActionResult> plus(int? cartId)
		{
			var cart = await _unitOfWork.ShoppingCart.GetAsync(u => u.Id == cartId);
			var success = _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
			if (!success)
			{
				TempData["error"] = "No more Available";

			}
			_unitOfWork.SaveChanges();

			return Redirect(nameof(Index));
		}
		public async Task<IActionResult> minus(int? cartId)
		{
			var cart = await _unitOfWork.ShoppingCart.GetAsync(u => u.Id == cartId);
			if (cart.Count <= 1)
			{
				await _unitOfWork.ShoppingCart.Remove(cart);
			}
			else
			{
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
			}
			_unitOfWork.SaveChanges();
			var count = _unitOfWork.ShoppingCart.GetAllAsync(u => u.IdentityUserId == cart.IdentityUserId).Result.ToList().Count;
			HttpContext.Session.SetInt32(SD.SessionCart, count);
			return Redirect(nameof(Index));
		}
		public async Task<IActionResult> remove(int? cartId)
		{
			var cart = await _unitOfWork.ShoppingCart.GetAsync(u => u.Id == cartId);
			await _unitOfWork.ShoppingCart.Remove(cart);
			_unitOfWork.SaveChanges();
			var count = _unitOfWork.ShoppingCart.GetAllAsync(u => u.IdentityUserId == cart.IdentityUserId).Result.ToList().Count;
			HttpContext.Session.SetInt32(SD.SessionCart, count);
			return Redirect(nameof(Index));
		}

        public IActionResult Confirm()
        {
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var claims = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
			var userId = claims.Value;
			_unitOfWork.ShoppingCart.Confirm(userId);
			_unitOfWork.SaveChanges();
			var count = _unitOfWork.ShoppingCart.GetAllAsync(u => u.IdentityUserId == userId).Result.ToList().Count;
			HttpContext.Session.SetInt32(SD.SessionCart, count);
			return View();
        }
    }
}