using DS_KnowledgeWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DS_KnowledgeWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
