using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyWorker.Infrastructure
{
    public sealed class CurrencyDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().ToTable("currency");
        }
    }
}
