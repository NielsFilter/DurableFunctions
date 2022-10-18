using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public class RemindFriendFunction
{
    private readonly ILogger _logger;
    public RemindFriendFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(RemindFriend))]
    public async Task RemindFriend([ActivityTrigger] string name)
    {
        _logger.LogInformation($"Give '{name}' a call to remind about event");
    }
}