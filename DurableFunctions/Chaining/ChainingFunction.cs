using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class ChainingFunction
{
    private readonly ILogger _logger;
    public ChainingFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function("ChainingFunction_HttpStart")]
    public async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
        HttpRequestData req,
        [DurableClient] DurableClientContext starter)
    {
        var instanceId = await starter.Client.ScheduleNewOrchestrationInstanceAsync(nameof(ChainingFunction));
        
        return starter.CreateCheckStatusResponse(req, instanceId);
    }

    [Function(nameof(ChainingFunction))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var tools = await context.CallActivityAsync<ToolsResponse>(nameof(FetchToolsFunction.FetchTools));
        var parts = await context.CallActivityAsync<PartsResponse>(nameof(FetchPartsFunction.FetchParts));
       
        var buildInput = new BuildShellInput { Tools = tools, Parts = parts };

        var robot = await context.CallActivityAsync<RobotResponse>(nameof(BuildShellFunction.BuildShell), buildInput);
        robot = await context.CallActivityAsync<RobotResponse>(nameof(ProgramRobotFunction.ProgramRobot), robot);
        robot = await context.CallActivityAsync<RobotResponse>(nameof(TestRobotFunction.TestRobot), robot);
        
        _logger.LogInformation($"Completed robot. Programmed: {robot.IsProgrammed}, tested: {robot.IsTested}, solid: {robot.IsSolid}, epic: {robot.IsEpic}");
    }
}