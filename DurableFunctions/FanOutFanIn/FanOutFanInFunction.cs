using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurableFunctions.FanOutFanIn;

public static class FanOutFanInFunction
{
    [FunctionName("FanOutFanIn_HttpStart")]
    public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        var body = await req.Content!.ReadAsStringAsync();
        var input = JsonConvert.DeserializeObject<SentimentUserInput>(body);
        
        var instanceId = await starter.StartNewAsync("FanOutFanIn", input);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
    
    
    [AllowAnonymous]
    [FunctionName("FanOutFanIn")]
    public static async Task RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context,
        ILogger log)
    {
        var input = context.GetInput<SentimentUserInput>();
        log.LogInformation($"Found {input.NumberOfUsers} users to queue");

        // Get a list of N work items to process in parallel.
        var tasks = new List<Task<SentimentResult>>();
        for (var userId = 1; userId <= input.NumberOfUsers; userId++)
        {
            tasks.Add(context.CallActivityAsync<SentimentResult>("CalculateBuyProbability", userId));
        }

        await Task.WhenAll(tasks);
        
        var results = tasks.Select(x => x.Result);
        await context.CallActivityAsync("PrintReport", results);
    }
}