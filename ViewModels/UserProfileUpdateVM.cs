using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.ViewModels
{
	public class UserProfileUpdateVM
	{
		[Required]
		public string UserName { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[MinLength(6)]
		public string CurrentPassword { get; set; }

		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Compare("NewPassword")]
		[MinLength(6)]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}
