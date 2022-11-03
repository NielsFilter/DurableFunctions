using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public class EventCoordinatorFunction
{
    
    [Function(nameof(EventCoordinator))]
    public async Task EventCoordinator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var inviteReq = context.GetContextInput<InviteFriendRequest>();
        
        // Send out invite
        var sendInviteReq = new SendInviteRequest { Friend = inviteReq.Friend, InviteId = context.InstanceId };
        await context.CallActivityAsync(nameof(InviteFriendFunction.InviteFriend), sendInviteReq);

        // Set a reminder timer if no response
        using var timeoutCts = new CancellationTokenSource();
        var reminderDueTime = context.CurrentUtcDateTime.AddHours(48);
        var reminderTimeout = context.CreateTimer(reminderDueTime, timeoutCts.Token);

        // Wait for reminder or RSVP (whichever comes first)
        var rsvpForEvent = context.WaitForExternalEvent<bool>(EventNames.RsvpReceived, timeoutCts.Token);
        if (reminderTimeout == await Task.WhenAny(reminderTimeout, rsvpForEvent))
        {
            // Send a reminder
            inviteReq.ReminderCount++;
            await context.CallSubOrchestratorAsync(nameof(EventCoordinator), input: inviteReq);
        }
        else
        {
            // RSVP received
            timeoutCts.Cancel();
            if (rsvpForEvent.Result)
            {
                // Yay!
                await context.CallActivityAsync(nameof(InviteAcceptedFunction.InviteAccepted), inviteReq.Friend);
            }
            else
            {
                // Invite a new friend
                var newInviteReq = new InviteFriendRequest() { Friend = "Lauren" };
                await context.CallSubOrchestratorAsync(nameof(EventCoordinator), input: newInviteReq);
            }
        }
    }

    [Function(nameof(InviteFriendHttp))]
    public static async Task<HttpResponseData> InviteFriendHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        [DurableClient] DurableClientContext starter,
        ILogger log)
    {
        var inviteReq = await req.GetFromBody<InviteFriendRequest>();
        
        var instanceId = await starter.Client.ScheduleNewOrchestrationInstanceAsync(nameof(EventCoordinator), input: inviteReq);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}