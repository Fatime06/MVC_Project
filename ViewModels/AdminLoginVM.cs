using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.ViewModels
{
    public class AdminLoginVM
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6)]
        [MaxLength(10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
