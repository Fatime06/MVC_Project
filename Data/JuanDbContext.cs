using Juan_Mvc_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Juan_Mvc_Project.Data
{
    public class JuanDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Slider> Services { get; set; }
        public JuanDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}

