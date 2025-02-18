using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.ViewModels
{
	public class UserRegisterVM
	{
		[Required]
		public string FullName { get; set; }

		[Required]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[Compare("Password")]
		[DataType(DataType.Password)]

		public string ConfirmPassword { get; set; }
	}
}
