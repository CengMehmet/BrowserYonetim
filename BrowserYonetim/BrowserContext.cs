using System.Data.Entity;

namespace BrowserYonetim
{
    public class BrowserContext : DbContext
    {
        public BrowserContext(string cs) : base(cs)
        {
            Database.SetInitializer(new DataInitializer());
        }

        public DbSet<User> UserSet { get; set; }
        public DbSet<BrowserLog> BrowserLogSet { get; set; }
    }
}