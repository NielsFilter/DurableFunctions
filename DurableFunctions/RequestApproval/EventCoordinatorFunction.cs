using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public class EventCoordinatorFunction
{
    private readonly ILogger _logger;

    public EventCoordinatorFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(EventCoordinator))]
    public async Task EventCoordinator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var inviteReq = context.GetInput<InviteRequest>();
        await context.CallActivityAsync(nameof(InviteFriendFunction.InviteFriend), inviteReq.Friend);

        using var timeoutCts = new CancellationTokenSource();
        var reminderDueTime = context.CurrentUtcDateTime.AddSeconds(15);
        var reminderTimeout = context.CreateTimer(reminderDueTime, timeoutCts.Token);

        var rsvpForEvent = context.WaitForExternalEvent<bool>("RsvpReceived", timeoutCts.Token);
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

    [Function(nameof(InviteFriendHttp))]
    public static async Task<HttpResponseData> InviteFriendHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        [DurableClient] DurableClientContext starter,
        ILogger log)
    {
        var inviteReq = await req.GetFromBody<InviteRequest>();
        
        var instanceId = await starter.Client.ScheduleNewOrchestrationInstanceAsync(nameof(EventCoordinator), input: inviteReq);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}