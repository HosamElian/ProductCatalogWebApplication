using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductCatalog.DataAccess.Repository.IRepository;
using ProductCatalog.Models.ViewModels;

namespace ProductCatalogWeb.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var productList = await _unitOfWork.Product.GetAllAsync();
            return View(productList);
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductVM productVM = new();

            if (id == null || id == 0) { productVM.Product = new(); }
            else { productVM.Product = await _unitOfWork.Product.GetAsync(p => p.Id == id); }

            productVM.CategoryList = _unitOfWork.Category.GetAllAsync().Result.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(productVM);

        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    if (productVM.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.CreateNew))
                    {
                        file.CopyTo(fileStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if (productVM.Product.Id == 0)
                {
                    var id = _userManager.GetUserId(User);
                    productVM.Product.CreatedBy = id ?? "";
                    _unitOfWork.Product.Add(productVM.Product);

                    TempData["success"] = "Product Created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product Updated successfully";
                }

                _unitOfWork.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            productVM.CategoryList = _unitOfWork.Category.GetAllAsync().Result.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(productVM);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var productfromDb = await _unitOfWork.Product.GetAsync(u => u.Id == id, false);
            return View(productfromDb);

        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productfromDb = await _unitOfWork.Product.GetAsync(u => u.Id == id, false);
            if (productfromDb == null)
            {
                return View(productfromDb);
            }

            await _unitOfWork.Product.Remove(productfromDb);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Product Delete successfully ";
            return RedirectToAction(nameof(Index));

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAllAsync().Result;
            return Json(new { data = productList });
        }
        
        #endregion
    }
}
