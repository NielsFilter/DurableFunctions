using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class TestRobotFunction
{
    private readonly ILogger _logger;

    public TestRobotFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(TestRobot))]
    public async Task<RobotResponse> TestRobot([ActivityTrigger] RobotResponse robot)
    {
        _logger.LogInformation("Testing the robot");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        
        robot.IsTested = true;
        _logger.LogInformation("Robot is tested!");
        return robot;
    }
}