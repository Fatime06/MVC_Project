using Microsoft.EntityFrameworkCore;

namespace Juan_Mvc_Project.Data
{
    public class JuanDbContext : DbContext
    {
        public JuanDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
