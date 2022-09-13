using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public static class InviteAcceptedFunction
{
    [FunctionName(nameof(InviteAccepted))]
    public static async Task InviteAccepted([ActivityTrigger] string name, ILogger log)
    {
        log.LogInformation($"Woohoo! '{name}' said yes, we're going...");
    }
}