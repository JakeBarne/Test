using CurrencyWorker.Models;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace CurrencyWorker.Services
{
    public sealed class CurrencyFetcher : ICurrencyFetcher
    {
        private static readonly System.Globalization.CultureInfo RuCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");

        private readonly HttpClient _httpClient;
        private readonly CurrencyWorkerOptions _options;
        private readonly ILogger<CurrencyFetcher> _logger;
        public CurrencyFetcher(
                                HttpClient httpClient,
                                IOptions<CurrencyWorkerOptions> options,
                                ILogger<CurrencyFetcher> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<IReadOnlyList<CurrencyDTO>> FetchAsync(CancellationToken ct)
        {
            var xml = await _httpClient.GetStringAsync(_options.SiteUrl, ct);
            var doc = XDocument.Parse(xml);

            var result = doc.Descendants("Valute")
                .Select(v => new CurrencyDTO
                (Name: (string)v.Element("CharCode")!,
                Rate: decimal.Parse((string)v.Element("Value")!, RuCulture)))
                .ToList();

            _logger.LogInformation("Курсы успешно получены");
            return result;
        }
    }
}
