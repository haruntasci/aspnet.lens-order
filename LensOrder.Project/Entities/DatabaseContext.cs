using Microsoft.EntityFrameworkCore;

namespace LensOrder.Project.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options):base(options) 
        {
        }
        public DbSet<Order> Orders { get; set; }
        
    }
}
