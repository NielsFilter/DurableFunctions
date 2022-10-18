using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class FetchToolsFunction
{
    [FunctionName(nameof(FetchTools))]
    public static async Task<ToolsResponse> FetchTools([ActivityTrigger] IDurableActivityContext context, ILogger log)
    {
        log.LogInformation("Fetching tools");
        await Task.Delay(RobotConstants.WorkflowStepDelay);
        var tools = new ToolsResponse()
        {
            MyTools = new List<string>()
            {
                "Screw Driver",
                "Pliers",
                "Hammer"
            },
            MyFriendsTools = new List<string>()
            {
                "Soldering Iron",
                "Toolbox"
            }
        };
        log.LogInformation($"Found my {string.Join(", ", tools.MyTools)}");
        log.LogInformation($"Borrowing my friend's {string.Join(", ", tools.MyFriendsTools)}");

        return tools;
    }
}

public class ToolsResponse
{
    public List<string> MyTools { get; set; }
    public List<string> MyFriendsTools { get; set; }
}