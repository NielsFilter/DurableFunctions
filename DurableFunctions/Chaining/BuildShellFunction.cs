using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public static class BuildShellFunction
{
    [FunctionName(nameof(BuildShell))]
    public static async Task<BuildShellResponse> BuildShell([ActivityTrigger] BuildShellInput buildInput, ILogger log)
    {
        log.LogInformation("Building shell!");
        await Task.Delay(30_000);
        return new BuildShellResponse
        {
            IsEpic = true,
            IsSolid = true,
            IsProgrammed = false
        };
    }
}

public class BuildShellInput
{
    public ToolsResponse Tools { get; set; }
    public PartsResponse Parts { get; set; }
}

public class BuildShellResponse
{
    public bool IsSolid { get; set; }
    public bool IsEpic { get; set; }
    public bool IsProgrammed { get; set; }
}