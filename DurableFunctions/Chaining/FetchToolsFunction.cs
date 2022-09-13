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
        await Task.Delay(30_000);
        return new ToolsResponse()
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
    }
}

public class ToolsResponse
{
    public List<string> MyTools { get; set; }
    public List<string> MyFriendsTools { get; set; }
}