using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public static class RemindFriendFunction
{
    [FunctionName(nameof(RemindFriend))]
    public static async Task RemindFriend([ActivityTrigger] string name, ILogger log)
    {
        log.LogInformation($"Give '{name}' a call to remind about event");
    }
}