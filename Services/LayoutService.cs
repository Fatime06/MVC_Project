using Juan_Mvc_Project.Data;

namespace Juan_Mvc_Project.Services
{
	public class LayoutService
	{
		private readonly JuanDbContext _context;

		public LayoutService(JuanDbContext context)
		{
			_context = context;
		}

		//public List<BlogCategory> GetAllCategories()
		//{
		//    return _context.BlogCategories.ToList();
		//} 

		//public Dictionary<string, string> GetAllSettings()
		//{
		//    return _context.Settings.ToDictionary(s=>s.Key, s=>s.Value);
		//}
	}
}
