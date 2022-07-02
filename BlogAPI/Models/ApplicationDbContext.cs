using BlogAPI.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        internal Task SaveChangesasync()
        {
            throw new NotImplementedException();
        }
    }
}
