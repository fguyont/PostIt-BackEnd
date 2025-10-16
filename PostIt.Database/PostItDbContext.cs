using Microsoft.EntityFrameworkCore;
using PostIt.Database.EntityModels;

namespace PostIt.Database
{
    public class PostItDbContext : DbContext
    {
        public PostItDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
