using System;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public static class EventCoordinatorFunction
{
    [FunctionName(nameof(EventCoordinator))]
    public static async Task EventCoordinator([OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var inviteReq = context.GetInput<InviteRequest>();
        await context.CallActivityAsync(nameof(InviteFriendFunction.InviteFriend), inviteReq.Friend);

        using var timeoutCts = new CancellationTokenSource();
        var reminderDueTime = context.CurrentUtcDateTime.AddSeconds(15);
        var reminderTimeout = context.CreateTimer(reminderDueTime, timeoutCts.Token);

        var rsvpForEvent = context.WaitForExternalEvent<bool>("RsvpReceived");
        if (reminderTimeout == await Task.WhenAny(reminderTimeout, rsvpForEvent))
        {
            await context.CallActivityAsync(nameof(RemindFriendFunction.RemindFriend), inviteReq.Friend);
        }
        else
        {
            timeoutCts.Cancel();
            if (rsvpForEvent.Result)
            {
                await context.CallActivityAsync(nameof(InviteAcceptedFunction.InviteAccepted), inviteReq.Friend);
            }
            else
            {
                await context.CallActivityAsync(nameof(FindNewFriendFunction.FindNewFriend), inviteReq.Friend);
            }
        }
    }

    [FunctionName(nameof(InviteFriendHttp))]
    public static async Task<HttpResponseMessage> InviteFriendHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        var inviteReq = await req.Content.ReadAsAsync<InviteRequest>();
        var instanceId = await starter.StartNewAsync(nameof(EventCoordinator), inviteReq);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}