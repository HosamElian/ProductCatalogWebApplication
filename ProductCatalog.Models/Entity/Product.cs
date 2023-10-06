using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Models.Entity
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [Range(1, 365)]
        public short DurationInDays { get; set; }
        [Required]
        [Range(1, 1000_000)]
        public double Price { get; set; }
		[Range(1, 1000_000)]

		public int Quantity { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
		[ValidateNever]
		public string CreatedBy { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
		public Category Category { get; set; }
    }
}
