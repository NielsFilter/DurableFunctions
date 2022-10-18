using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class ProgramRobotFunction
{
    private readonly ILogger _logger;

    public ProgramRobotFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(ProgramRobot))]
    public async Task<RobotResponse> ProgramRobot([ActivityTrigger] RobotResponse robot)
    {
        _logger.LogInformation("Writing some code");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        
        robot.IsProgrammed = true;
        _logger.LogInformation($"Robot programmed: {robot.IsProgrammed}");
        return robot;
    }
}