using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public static class BuildShellFunction
{
    [FunctionName(nameof(BuildShell))]
    public static async Task<RobotResponse> BuildShell([ActivityTrigger] BuildShellInput buildInput, ILogger log)
    {
        log.LogInformation("Building shell!");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        
        var result = new RobotResponse
        {
            IsEpic = true,
            IsSolid = true,
            IsProgrammed = false,
            IsTested = false
        };
        
        log.LogInformation($"Shell built! Epic: {result.IsEpic} Solid: {result.IsSolid}");
        return result;
    }
}