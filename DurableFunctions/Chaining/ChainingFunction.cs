using System;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public static class ChainingFunction
{
    [FunctionName("ChainingFunction_HttpStart")]
    public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
        HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        var instanceId = await starter.StartNewAsync("ChainingFunction");
        return starter.CreateCheckStatusResponse(req, instanceId);
    }

    [FunctionName("ChainingFunction")]
    public static async Task RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context,
        ILogger logger)
    {
        var tools = await context.CallActivityAsync<ToolsResponse>(nameof(FetchToolsFunction.FetchTools), null);
        var parts = await context.CallActivityAsync<PartsResponse>(nameof(FetchPartsFunction.FetchParts), null);

        var buildInput = new BuildShellInput { Tools = tools, Parts = parts };

        var robot = await context.CallActivityAsync<RobotResponse>(nameof(BuildShellFunction.BuildShell), buildInput);
        robot = await context.CallActivityAsync<RobotResponse>(nameof(ProgramRobotFunction.ProgramRobot), robot);
        robot = await context.CallActivityAsync<RobotResponse>(nameof(TestRobotFunction.TestRobot), robot);

        logger.LogInformation("Robot is complete!");
    }
}