using Juan_Mvc_Project.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Juan_Mvc_Project.Models
{
    public class Product : BaseEntity
    {
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
		public string Description { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal SalePrice { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal CostPrice { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal DiscountPercentage { get; set; }
		public bool InStock { get; set; }
		public int Rate { get; set; }
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }
		public List<ProductImage> ProductImages { get; set; }
		[NotMapped]
		[AllowedTypes("image/jpeg", "image/png")]
		public List<IFormFile> Photos { get; set; }
		public List<ProductReview> ProductReviews { get; set; }
		public List<ProductSize> ProductSizes { get; set; }
		[NotMapped]
		public List<int> SizeIds { get; set; }

		public Product()
		{
			ProductImages = new List<ProductImage>();
			ProductReviews = new List<ProductReview>();
			ProductSizes = new List<ProductSize>();
		}
	}
}
