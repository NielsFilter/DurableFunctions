using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.AsyncHttpApi;

public class AsyncHttpApi
{
    private readonly ILogger _logger;

    public AsyncHttpApi(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(AsyncHttpApi))]
    public async Task<string> RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var outputs = new List<string>();

        // Replace "hello" with the name of your Durable Activity Function.
        var password = await context.CallActivityAsync<string>(nameof(BruteForcePasswordFunction.BruteForcePassword), null);
        _logger.LogInformation($"Found the password: {password}");

        return $"Password is: {password}";
    }

    [Function("AsyncHttpApi_HttpStart")]
    public async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
        HttpRequestData req,
        [DurableClient] DurableClientContext starter,
        ILogger log)
    {
        // Function input comes from the request content.
        var instanceId = await starter.Client.ScheduleNewOrchestrationInstanceAsync(nameof(AsyncHttpApi));

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}