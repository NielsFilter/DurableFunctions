using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class FetchPartsFunction
{
    private readonly ILogger _logger;

    public FetchPartsFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(FetchParts))]
    public async Task<PartsResponse> FetchParts([ActivityTrigger] TaskActivityContext context)
    {
        _logger.LogInformation("Fetching parts");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        var parts = new PartsResponse
        {
            MyParts = new List<string>
            {
                "Screws",
                "Motors",
                "Duct tape"
            }
        };
        _logger.LogInformation($"Found parts {string.Join(", ", parts)}");
        return parts;
    }
}