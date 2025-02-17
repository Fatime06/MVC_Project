using System.ComponentModel.DataAnnotations.Schema;

namespace Juan_Mvc_Project.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Discount { get; set; }
        public bool InStock { get; set; }
        public bool IsNew { get; set; }
        public int CategoryId { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal CostPrice { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductTag> ProductTags { get; set; }

        public Product()
        {
            ProductImages = new List<ProductImage>();
            ProductSizes = new List<ProductSize>();
        }
    }
}
