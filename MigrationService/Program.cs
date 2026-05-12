using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MigrationService.Infrastructure;

var host = Host.CreateApplicationBuilder(args);
host.Services.AddDbContext<AppDbContext>(context =>
            context.UseNpgsql(host.Configuration.GetConnectionString("Default")));

var app = host.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

try
{
    await db.Database.MigrateAsync();
    Console.WriteLine("Миграции применены успешно.");
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Ошибка миграции: {ex.Message}");
    return 1;
}