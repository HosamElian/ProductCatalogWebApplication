using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Models.Entity
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

    }
}
