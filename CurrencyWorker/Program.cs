using System.Text;
using CurrencyWorker.Extensions;
using Serilog;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog((_, config) =>
        config.ReadFrom.Configuration(builder.Configuration)
              .WriteTo.Console());

    builder.Services.AddWorkerServices(builder.Configuration);

    var host = builder.Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Приложение неожиданно завершило работу");
}
finally
{
    await Log.CloseAndFlushAsync();
}