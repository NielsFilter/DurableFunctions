using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class BuildShellFunction
{
    private readonly ILogger _logger;

    public BuildShellFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(BuildShell))]
    public async Task<RobotResponse> BuildShell([ActivityTrigger] BuildShellInput buildInput)
    {
        _logger.LogInformation("Building shell!");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        
        var result = new RobotResponse
        {
            IsEpic = true,
            IsSolid = true,
            IsProgrammed = false,
            IsTested = false
        };
        
        _logger.LogInformation($"Shell built! Epic: {result.IsEpic} Solid: {result.IsSolid}");
        return result;
    }
}