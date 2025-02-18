using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.Attributes
{
	public class MaxSizeAttribute : ValidationAttribute
	{
		private int _size;
		public MaxSizeAttribute(int size)
		{
			_size = size;
		}
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			List<IFormFile> files = new List<IFormFile>();
			if (value is List<IFormFile> fileList) files = fileList;
			if (value is IFormFile file) files.Add(file);
			foreach (var item in files)
			{
				if (item.Length > _size)
				{
					string message = "file size must be less than 2mb";
					return new ValidationResult(message);
				}
			}

			return ValidationResult.Success;
		}
	}
}
