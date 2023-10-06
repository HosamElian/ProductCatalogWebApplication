using ProductCatalog.Models.Entity;

namespace ProductCatalog.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public double TotalPrice { get; set; }
        public IEnumerable<ShoppingCart> ListCart { get; set; }
    }
}
