using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Models.Entity
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConfirmed { get; set; }
        public string IdentityUserId { get; set; }
        [ValidateNever]
        [ForeignKey("IdentityUserId")]
        public IdentityUser? IdentityUser { get; set; }
    }
}
