using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.FanOutFanIn;

public class FanOutFanInFunction
{
    private readonly ILogger _logger;
    public FanOutFanInFunction(ILogger logger)
    {
        _logger = logger;
    }

    [Function("FanOutFanIn_HttpStart")]
    public async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        [DurableClient] DurableClientContext starter)
    {
        var input = await req.GetFromBody<SentimentUserInput>();

        var instanceId = await starter.Client.ScheduleNewOrchestrationInstanceAsync(nameof(FanOutFanInFunction), input: input);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }

    [Function(nameof(FanOutFanInFunction))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var input = context.GetContextInput<SentimentUserInput>();
        _logger.LogInformation($"Found {input.NumberOfUsers} users to queue");

        // Get a list of N work items to process in parallel.
        var tasks = new List<Task<SentimentResult>>();
        for (var userId = 1; userId <= input.NumberOfUsers; userId++)
        {
            tasks.Add(context.CallActivityAsync<SentimentResult>(nameof(CalculateUserSentiment), userId));
        }

        await Task.WhenAll(tasks);
        
        var results = tasks.Select(x => x.Result);
        await context.CallActivityAsync(nameof(PrintReport), results);
    }
}