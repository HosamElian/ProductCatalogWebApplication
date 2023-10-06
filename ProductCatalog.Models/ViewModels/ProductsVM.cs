using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductCatalog.Models.Entity;

namespace ProductCatalog.Models.ViewModels
{
    public class ProductsVM
    {
        public int CategoryId { get; set; }
        public IEnumerable<Product> Products { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        
    }
}
