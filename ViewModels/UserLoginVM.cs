using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.ViewModels
{
	public class UserLoginVM
	{
		[Required]
		public string UserNameOrEmail { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
