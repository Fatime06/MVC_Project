using Juan_Mvc_Project.Models;

namespace Juan_Mvc_Project.ViewModels
{
	public class ProductDetailVM
	{
		public Product Product { get; set; }
		public List<Product> RelatedProducts { get; set; }
		public bool HasCommentUser { get; set; }
		public int TotalReviews { get; set; }
		public int AvgRate { get; set; }
		public ProductReview ProductReview { get; set; }
	}
}
