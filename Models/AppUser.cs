using JuanApp.Models;
using Microsoft.AspNetCore.Identity;

namespace Juan_Mvc_Project.Models
{
	public class AppUser : IdentityUser
	{
		public string FullName { get; set; }
		public List<BasketItem> BasketItems { get; set; }
	}
}
