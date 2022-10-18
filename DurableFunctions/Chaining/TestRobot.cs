using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public static class TestRobotFunction
{
    [FunctionName(nameof(TestRobot))]
    public static async Task<RobotResponse> TestRobot([ActivityTrigger] RobotResponse robot, ILogger log)
    {
        log.LogInformation("Testing the robot");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        
        robot.IsTested = true;
        log.LogInformation("Robot is tested!");
        return robot;
    }
}