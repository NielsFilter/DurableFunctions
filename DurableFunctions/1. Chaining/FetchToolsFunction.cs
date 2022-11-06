using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class FetchToolsFunction
{
    private readonly ILogger _logger;

    public FetchToolsFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(FetchTools))]
    public async Task<ToolsResponse> FetchTools([ActivityTrigger] TaskActivityContext context)
    {
        _logger.LogInformation("Fetching tools");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        var tools = new ToolsResponse
        {
            MyTools = new List<string>
            {
                "Screw Driver",
                "Pliers",
                "Hammer"
            },
            MyFriendsTools = new List<string>
            {
                "Soldering Iron",
                "Toolbox"
            }
        };
        _logger.LogInformation($"Found my {string.Join(", ", tools.MyTools)}");
        _logger.LogInformation($"Borrowing my friend's {string.Join(", ", tools.MyFriendsTools)}");

        return tools;
    }
}