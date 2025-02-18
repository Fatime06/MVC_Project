using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.ViewModels
{
	public class ForgotPasswordVM
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
