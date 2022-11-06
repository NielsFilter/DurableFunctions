using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using RandomNameGeneratorLibrary;

namespace Company.FunctionApp1.FanOutFanIn;

public interface ICalculateProbabilityService
{
    Task<SentimentResult> Calculate(int userId);
}

public class CalculateProbabilityService : ICalculateProbabilityService
{
    private readonly IPersonNameGenerator _nameGenerator;
    private readonly ILogger<CalculateProbabilityService> _logger;

    public CalculateProbabilityService(IPersonNameGenerator nameGenerator, ILogger<CalculateProbabilityService> logger)
    {
        _nameGenerator = nameGenerator;
        _logger = logger;
    }
    
    public async Task<SentimentResult> Calculate(int userId)
    {
        var rnd = new Random();
        
        // simulate some work here...
        await Task.Delay(rnd.Next(0, 5_000));
        var happinessSentiment = rnd.Next(0, 100);

        _logger.LogInformation($"Completed sentiment calculation for user {userId}");
        
        // context.
        return new SentimentResult
        {
            User = GetRandomName(),
            Sentiment = happinessSentiment
        };
    }

    private string GetRandomName()
    {
        return $"{_nameGenerator.GenerateRandomFirstName()} {_nameGenerator.GenerateRandomLastName()}";
    }
}