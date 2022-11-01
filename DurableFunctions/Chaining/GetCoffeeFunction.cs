using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class HaveCoffeeFunction
{
    private readonly ILogger _logger;

    public HaveCoffeeFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(HaveCoffeeFunction))]
    public async Task Coffee([ActivityTrigger] TaskActivityContext context)
    {
        _logger.LogInformation("Make coffee");
        _logger.LogInformation("Drink coffee");
        
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        
        _logger.LogInformation("Ah... much better");
    }
}