using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public static class ProgramRobotFunction
{
    [FunctionName(nameof(ProgramRobot))]
    public static async Task<RobotResponse> ProgramRobot([ActivityTrigger] RobotResponse robot, ILogger log)
    {
        log.LogInformation("Writing some code");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        
        robot.IsProgrammed = true;
        log.LogInformation($"Robot programmed: {robot.IsProgrammed}");
        return robot;
    }
}