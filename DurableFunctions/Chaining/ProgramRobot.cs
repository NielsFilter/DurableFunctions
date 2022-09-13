using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public static class ProgramRobotFunction
{
    [FunctionName(nameof(ProgramRobot))]
    public static async Task<BuildShellResponse> ProgramRobot([ActivityTrigger] BuildShellResponse shell, ILogger log)
    {
        log.LogInformation("Writing some code");
        await Task.Delay(30_000);
        
        shell.IsProgrammed = true;
        return shell;
    }
}