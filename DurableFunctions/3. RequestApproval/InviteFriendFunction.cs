using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public class InviteFriendFunction
{
    private readonly ILogger _logger;
    public InviteFriendFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(InviteFriend))]
    public async Task InviteFriend([ActivityTrigger] SendInviteRequest input, TaskActivityContext context)
    {
        _logger.LogInformation($"Sent invite to friend '{input.Friend}'!");
        
        var acceptUrl = $"http://localhost:7071/api/RsvpHttp/{input.InviteId}";
        
        _logger.LogInformation($"To accept click here: '{acceptUrl}/true'");
        _logger.LogInformation($"To decline click here: '{acceptUrl}/false'");
    }
}