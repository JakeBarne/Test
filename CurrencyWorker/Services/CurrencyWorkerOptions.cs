
namespace CurrencyWorker.Services
{
    public sealed class CurrencyWorkerOptions
    {
        public const string SectionName = "Worker";
        public string SiteUrl { get; init; } = string.Empty;
        public int IntervalHours { get; init; } = 1;
    }
}
