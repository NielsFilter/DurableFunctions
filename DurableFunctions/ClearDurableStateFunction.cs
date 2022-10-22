using DurableTask.AzureStorage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;

namespace DurableFunctions;

public class ClearDurableStateFunction
{
    private readonly IConfiguration _config;

    public ClearDurableStateFunction(IConfiguration config)
    {
        _config = config;
    }
    
    [Function(nameof(ClearDurableStateFunction))]
    public async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete")]
        HttpRequestData req,
        [DurableClient] DurableClientContext starter)
    {
        var storageService = new AzureStorageOrchestrationService(new AzureStorageOrchestrationServiceSettings
        {
            StorageConnectionString = _config["storageConnectionString"],
            TaskHubName = starter.TaskHubName
        });
        await storageService.DeleteAsync();
        return req.CreateResponse(System.Net.HttpStatusCode.OK);
    }
}