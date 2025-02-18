namespace Juan_Mvc_Project.Models
{
    public class ProductImage : BaseEntity
    {
		public string Name { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public bool? Status { get; set; }
	}
}
