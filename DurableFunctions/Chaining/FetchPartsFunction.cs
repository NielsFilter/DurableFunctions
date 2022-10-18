using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public static class FetchPartsFunction
{
    [FunctionName(nameof(FetchParts))]
    public static async Task<PartsResponse> FetchParts([ActivityTrigger] IDurableActivityContext context, ILogger log)
    {
        log.LogInformation("Fetching parts");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        var parts = new PartsResponse()
        {
            MyParts = new List<string>()
            {
                "Screws",
                "Motors",
                "Duct tape"
            }
        };
        log.LogInformation($"Found parts {string.Join(", ", parts)}");
        return parts;
    }
}

public class PartsResponse
{
    public List<string> MyParts { get; set; }
}