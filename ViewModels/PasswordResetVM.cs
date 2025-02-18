using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.ViewModels
{
	public class PasswordResetVM
	{
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[Compare("Password")]
		[DataType(DataType.Password)]

		public string ConfirmPassword { get; set; }
		public string Email { get; set; }
		public string Token { get; set; }
	}
}
