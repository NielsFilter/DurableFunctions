using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public static class FindNewFriendFunction
{
    [FunctionName(nameof(FindNewFriend))]
    public static async Task FindNewFriend([ActivityTrigger] string name, ILogger log)
    {
        log.LogInformation("Find another friend...");
    }
}