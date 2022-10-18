using Company.FunctionApp1.FanOutFanIn;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RandomNameGeneratorLibrary;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(config =>
    {
        config
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    })
    .ConfigureServices(services =>
    {
        services.AddTransient<ICalculateProbabilityService, CalculateProbabilityService>();
        services.AddTransient<ISentimentReportGenerator, SentimentReportGenerator>();
        services.AddTransient<IAzureStorageProvider, AzureStorageProvider>();
        services.AddTransient<IPersonNameGenerator, PersonNameGenerator>();
        services.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("DefaultLogger"));
    })
    .Build();

host.Run();