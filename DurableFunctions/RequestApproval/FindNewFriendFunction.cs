using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public class FindNewFriendFunction
{
    private readonly ILogger _logger;

    public FindNewFriendFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(FindNewFriend))]
    public async Task FindNewFriend([ActivityTrigger] string name, ILogger log)
    {
        _logger.LogInformation("Find another friend...");
    }
}