using Company.FunctionApp1.FanOutFanIn;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace DurableFunctions.FanOutFanIn;

public class PrintReport
{
    private readonly ISentimentReportGenerator _reportGenerator;

    public PrintReport(ISentimentReportGenerator reportGenerator)
    {
        _reportGenerator = reportGenerator;
    }
    
    [Function(nameof(PrintReport))]
    public async Task Print([ActivityTrigger] List<SentimentResult> results)
    {
        await _reportGenerator.GenerateReport(results);
    }
}