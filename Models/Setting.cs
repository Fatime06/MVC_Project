using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.Models
{
	public class Setting
	{
		[Key]
		public string Key { get; set; }
		public string Value { get; set; }
	}
}
