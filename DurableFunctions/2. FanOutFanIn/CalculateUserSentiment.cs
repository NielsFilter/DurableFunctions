using Company.FunctionApp1.FanOutFanIn;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.FanOutFanIn;

public class CalculateUserSentiment
{
    private readonly ICalculateProbabilityService _calculateProbabilityService;
    private readonly ILogger _logger;

    public CalculateUserSentiment(ICalculateProbabilityService calculateProbabilityService, ILogger logger)
    {
        _calculateProbabilityService = calculateProbabilityService;
        _logger = logger;
    }
    
    [Function(nameof(CalculateUserSentiment))]
    public async Task<SentimentResult> Calculate([ActivityTrigger] int userId)
    {
        return await _calculateProbabilityService.Calculate(userId);
    }
}