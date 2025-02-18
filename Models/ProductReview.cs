namespace Juan_Mvc_Project.Models
{
	public class ProductReview : BaseEntity
	{
		public string Text { get; set; }
		public int Rate { get; set; }
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
	}
	public enum ReviewStatus
	{
		Pending,
		Accepted,
		Rejected
	}
}
