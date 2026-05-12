using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infra
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UserFavoriteCurrency> FavoriteCurrency { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().ToTable("currency");
            modelBuilder.Entity<UserFavoriteCurrency>().ToTable("user_favorite_currency")
                .HasKey(x => new { x.UserId, x.CurrencyId });
        }
    }
}
