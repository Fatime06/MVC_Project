using Juan_Mvc_Project.Models;

namespace Juan_Mvc_Project.ViewModels
{
    public class HomeVM
    {
		public List<Slider> Sliders { get; set; }
		public List<Product> NewProducts { get; set; }
		public List<Product> Products { get; set; }
		public List<Service> Services { get; set; }

	}
}
