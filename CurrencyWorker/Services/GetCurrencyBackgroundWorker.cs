using CurrencyWorker.Infrastructure;
using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Xml;

namespace CurrencyWorker.Services
{
    public sealed class GetCurrencyBackgroundWorker : BackgroundService
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ICurrencyFetcher _currencyFetcher;
        private readonly ILogger<GetCurrencyBackgroundWorker> _logger;
        private readonly TimeSpan _interval;

        public GetCurrencyBackgroundWorker(IServiceScopeFactory serviceScopeFactory,
                                            ICurrencyFetcher currencyFetcher,
                                            ILogger<GetCurrencyBackgroundWorker> logger,
                                            IOptions<CurrencyWorkerOptions> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _currencyFetcher = currencyFetcher;
            _logger = logger;
            _interval = TimeSpan.FromHours(options.Value.IntervalHours);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Фоновый процесс запущен");
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunCycleAsync(stoppingToken);
                try
                {
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task RunCycleAsync(CancellationToken ct)
        {
            try
            {
                var currencies = await _currencyFetcher.FetchAsync(ct);
                using var scope = _serviceScopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<CurrencyDbContext>();

                var existing = await db.Currencies.ToDictionaryAsync(x => x.Name, ct);
                var updated = 0;
                var inserted = 0;

                foreach (var dto in currencies)
                {
                    if (existing.TryGetValue(dto.Name, out var entity))
                    {
                        entity.Rate = dto.Rate;
                        updated++;
                    }
                    else
                    {
                        db.Currencies.Add(new Currency { Name = dto.Name, Rate = dto.Rate });
                        inserted++;
                    }
                }

                await db.SaveChangesAsync(ct);
                _logger.LogInformation("Обновлено: {Updated}, новых: {Inserted}", updated, inserted);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Ошибка HTTP {StatusCode}", ex.StatusCode);
            }
            catch (XmlException ex)
            {
                _logger.LogError(ex, "Ошибка парсинга XML. Строка: {Line}", ex.LineNumber);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка сохранения курсов");
            }
        }
    }
}
