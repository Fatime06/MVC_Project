using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public List<Product> Products { get; set; }

        public Category()
        {
            Products = new();
        }
    }
}
