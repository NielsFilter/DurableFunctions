using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.FanOutFanIn;

public static class PrintReport
{
    [FunctionName("PrintReport")]
    public static async Task Print([ActivityTrigger] List<SentimentResult> results, ILogger log)
    {
        await Task.Yield();
        
        log.LogInformation("Generating report...");
        foreach (var resultItem in results)
        {
            log.LogInformation($"User {resultItem.UserId}\t{resultItem.Sentiment:D2}%");
        }
    }
}