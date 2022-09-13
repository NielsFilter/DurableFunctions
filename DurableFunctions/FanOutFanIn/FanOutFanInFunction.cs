using System;
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

public static partial class FanOutFanInFunction
{
    [FunctionName("FanOutFanIn_HttpStart")]
    public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
        HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        var body = await req.Content!.ReadAsStringAsync();
        var input = JsonConvert.DeserializeObject<PrimeNumberInput>(body);
        
        var instanceId = await starter.StartNewAsync("FanOutFanIn", input);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
    
    
    [AllowAnonymous]
    [FunctionName("FanOutFanIn")]
    public static async Task RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var tasks = new List<Task<PrimeNumberResult>>();
        var input = context.GetInput<PrimeNumberInput>();

        Console.WriteLine("hello");

        // Get a list of N work items to process in parallel.
        for (var i = input.Start; i <= input.Stop; i++)
        {
            tasks.Add(context.CallActivityAsync<PrimeNumberResult>("PrimeNumberChecker", i));
        }
        
        await Task.WhenAll(tasks);
        
        var results = tasks.Select(x => x.Result);
        await context.CallActivityAsync<int>("PrintPrimeNumberResult", results);
    }

    [FunctionName("PrimeNumberChecker")]
    public static async Task<PrimeNumberResult> PrimeNumberChecker([ActivityTrigger] int number, ILogger log)
    {
        var rnd = new Random();
        await Task.Delay(rnd.Next(0, 60_000)); // simulate some work here...
        log.LogInformation($"Check if {number} is prime");
        return new PrimeNumberResult
        {
            Number = number,
            IsPrime = number.IsPrime()
        };
    }
    
    [FunctionName("PrintPrimeNumberResult")]
    public static Task PrintPrimeNumberResult([ActivityTrigger] List<PrimeNumberResult> results, ILogger log)
    {
        log.LogInformation($"Done checking prime numbers. Here's the result");
        foreach (var resultItem in results)
        {
            log.LogInformation($"{resultItem.Number}\t{resultItem.IsPrime}");
        }

        return Task.CompletedTask;
    }
}