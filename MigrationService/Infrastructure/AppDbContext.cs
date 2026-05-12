using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace MigrationService.Infrastructure
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UserFavoriteCurrency> UserFavoriteCurrencies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("user")
                .HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Currency>().ToTable("currency");
            modelBuilder.Entity<UserFavoriteCurrency>().ToTable("user_favorite_currency")
                .HasKey(x => new { x.UserId, x.CurrencyId });
        }
    }
}
