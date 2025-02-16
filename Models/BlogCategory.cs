using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.Models
{
    public class BlogCategory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
