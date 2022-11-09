using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public class InviteAcceptedFunction
{
    private readonly ILogger _logger;

    public InviteAcceptedFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(InviteAccepted))]
    public bool InviteAccepted([ActivityTrigger] string name)
    {
        _logger.LogInformation($"Woohoo! '{name}' said yes, we're going...");
        return true;
    }
}