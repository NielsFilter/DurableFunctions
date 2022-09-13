using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public static class InviteFriendFunction
{
    [FunctionName(nameof(InviteFriend))]
    public static async Task InviteFriend([ActivityTrigger] IDurableActivityContext context, ILogger log)
    {
        var name = context.GetInput<string>();
        log.LogInformation($"Sent invite to friend '{name}'!");
        
        var acceptUrl = $"http://localhost:7071/api/RsvpHttp/{context.InstanceId}";
        log.LogInformation($"To accept click here: '{acceptUrl}/true'");
        log.LogInformation($"To decline click here: '{acceptUrl}/false'");
    }
}