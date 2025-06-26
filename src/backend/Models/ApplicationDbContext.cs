using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Investment> Investments { get; set; }
        public DbSet<Asset> Assets { get; set; }
    }
}
