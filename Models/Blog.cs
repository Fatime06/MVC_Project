using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.Models
{
    public class Blog : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        public string Quote { get; set; }
        public int BlogCategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }
    }
}
