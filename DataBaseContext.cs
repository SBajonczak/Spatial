using Microsoft.EntityFrameworkCore;

namespace spatial
{
    public class DataBaseContext:DbContext
    {

        public DataBaseContext(DbContextOptions opts) : base(opts)
        {

        }

        public DbSet<City> Cities { get; set; } 
    }
}
