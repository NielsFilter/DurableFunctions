using System.Text;
using HtmlTableHelper;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Company.FunctionApp1.FanOutFanIn;

public interface ISentimentReportGenerator
{
    Task<Uri> GenerateReport(List<SentimentResult> results);
}

public class SentimentReportGenerator : ISentimentReportGenerator
{
    private readonly IAzureStorageProvider _storageProvider;
    private readonly ILogger<SentimentReportGenerator> _logger;

    public SentimentReportGenerator(
        IAzureStorageProvider storageProvider,
        ILogger<SentimentReportGenerator> logger)
    {
        _storageProvider = storageProvider;
        _logger = logger;
    }
    
    public async Task<Uri> GenerateReport(List<SentimentResult> results)
    {
        _logger.LogInformation($"Generating report for {results.Count} users");
        var reportStream = CreateReportStream(results);

        return await _storageProvider.UploadBlobFromStreamAsync(
            reportStream,
            $"report{DateTime.UtcNow:yyyy-MM-dd}.html",
            "sentiment-reports");
    }

    private static MemoryStream CreateReportStream(List<SentimentResult> results)
    {
        var html = results.OrderByDescending(x => x.Sentiment).ToHtmlTable() ?? "";
        return new MemoryStream(Encoding.UTF8.GetBytes(html));
    }
}