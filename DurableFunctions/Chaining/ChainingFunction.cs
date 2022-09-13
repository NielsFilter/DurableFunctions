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
        // Function input comes from the request content.
        var instanceId = await starter.StartNewAsync("ChainingFunction", null);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
    
    [FunctionName("ChainingFunction")]
    public static async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context,
        ILogger logger)
    {
        var outputs = new List<string>();
        logger.LogInformation(".......STARTING.......");
        try
        {

            // Replace "hello" with the name of your Durable Activity Function.
            var tools = await context.CallActivityAsync<ToolsResponse>(nameof(FetchToolsFunction.FetchTools), null);
            logger.LogInformation($"Found my {string.Join(", ", tools.MyTools)}");
            logger.LogInformation($"Borrowing my friend's {string.Join(", ", tools.MyFriendsTools)}");
        
            var parts = await context.CallActivityAsync<PartsResponse>(nameof(FetchPartsFunction.FetchParts), null);
            logger.LogInformation($"Found parts {string.Join(", ", parts)}");

            var buildInput = new BuildShellInput
            {
                Tools = tools,
                Parts = parts,
            };
        
            var shell = await context.CallActivityAsync<BuildShellResponse>(nameof(BuildShellFunction.BuildShell), buildInput);
            logger.LogInformation($"Shell built! Epic: {shell.IsEpic} Solid: {shell.IsSolid}");
        
            shell = await context.CallActivityAsync<BuildShellResponse>(nameof(ProgramRobotFunction.ProgramRobot), shell);
            logger.LogInformation($"Robot programmed: {shell.IsProgrammed}");
        
            logger.LogInformation("Robot is complete!");
        }
        finally
        {
            logger.LogInformation("Finally!!!!");
        }

        return outputs;
    }
}