using System.ComponentModel.DataAnnotations;

namespace Juan_Mvc_Project.Attributes
{
	public class AllowedTypesAttribute : ValidationAttribute
	{
		private string[] _allowedTypes;
		public AllowedTypesAttribute(params string[] types)
		{
			_allowedTypes = types;
		}
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			List<IFormFile> files = new List<IFormFile>();
			if (value is List<IFormFile> fileList) files = fileList;
			if (value is IFormFile file) files.Add(file);
			foreach (var item in files)
			{
				if (!_allowedTypes.Contains(item.ContentType))
				{
					string message = "file content types must be jpeg or png";
					return new ValidationResult(message);
				}

			}
			return ValidationResult.Success;

		}
	}
}
