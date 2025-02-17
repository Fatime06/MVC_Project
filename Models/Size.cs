using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.Models
{
    public class Size : BaseEntity
    {
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
    }
}
