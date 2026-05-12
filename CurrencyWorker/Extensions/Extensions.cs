using CurrencyWorker.Infrastructure;
using CurrencyWorker.Services;
using Microsoft.EntityFrameworkCore;

namespace CurrencyWorker.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CurrencyWorkerOptions>(config.GetSection(CurrencyWorkerOptions.SectionName));

            services.AddDbContext<CurrencyDbContext>(context =>
                    context.UseNpgsql(config.GetConnectionString("Default")));
            services.AddHttpClient<ICurrencyFetcher, CurrencyFetcher>();
            services.AddHostedService<GetCurrencyBackgroundWorker>();
            return services;
        }
    }
}
