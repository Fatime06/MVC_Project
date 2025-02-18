using Juan_Mvc_Project.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.Models
{
    public class Slider : BaseEntity
    {
		[Required]
		[MaxLength(100)]
		public string Title { get; set; }
		[Required]
		[MaxLength(50)]
		public string UpperText { get; set; }
		[Required]
		[MaxLength(300)]
		public string Description { get; set; }
		[Required]
		[MaxLength(20)]
		public string ButtonText { get; set; }
		[Required]
		[MaxLength(100)]
		public string ButtonLink { get; set; }
		public string Image { get; set; }
		public int OrderQueue { get; set; }
		[NotMapped]
		[AllowedTypes("image/jpeg", "image/png", "image/jpg")]
		public IFormFile Photo { get; set; }
	}
}
