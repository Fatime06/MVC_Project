using Juan_Mvc_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Juan_Mvc_Project.Data
{
    public class JuanDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public JuanDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}

