using System;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.AsyncHttpApi;

public static class AsyncHttpApi
{
    [FunctionName("AsyncHttpApi")]
    public static async Task<string> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger logger)
    {
        var outputs = new List<string>();

        // Replace "hello" with the name of your Durable Activity Function.
        var password = await context.CallActivityAsync<string>(nameof(BruteForcePasswordFunction.BruteForcePassword), null);
        logger.LogInformation($"Found the password: {password}");

        return $"Password is: {password}";
    }

    [FunctionName("AsyncHttpApi_HttpStart")]
    public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
        HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        // Function input comes from the request content.
        var instanceId = await starter.StartNewAsync("AsyncHttpApi", null);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}