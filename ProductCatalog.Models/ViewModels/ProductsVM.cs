using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductCatalog.Models.Entity;

namespace ProductCatalog.Models.ViewModels
{
    public class ProducstVM
    {
        public IEnumerable<Product> Products { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        
    }
}
