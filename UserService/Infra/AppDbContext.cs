using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infra
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("user")
                .HasIndex(u => u.Name).IsUnique();
        }
    }
}
