using System;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.FanOutFanIn;

public static class CalculateBuyProbability
{
    [FunctionName("CalculateBuyProbability")]
    public static async Task<SentimentResult> Calculate([ActivityTrigger] int userId, ILogger log)
    {
        var rnd = new Random();
        
        // simulate some work here...
        await Task.Delay(rnd.Next(0, 5_000));
        var happinessSentiment = rnd.Next(0, 100);

        log.LogInformation($"Completed sentiment calculation for user {userId}");
        
        return new SentimentResult
        {
            UserId = userId,
            Sentiment = happinessSentiment
        };
    }
}