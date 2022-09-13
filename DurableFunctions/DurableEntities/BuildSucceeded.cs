using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.DurableEntities;

public class BuildSucceeded
{
    [FunctionName("BuildSucceeded")]
    public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "PR/{prId}/BuildSucceeded")] HttpRequestMessage req,
        int prId,
        [DurableClient] IDurableEntityClient entityClient,
        ILogger log)
    {
        var entity = new EntityId("PeerReviewAction", prId.ToString());
        await entityClient.SignalEntityAsync(entity, "PrAction", PeerReviewActionTypes.BuildSucceeded);
        return req.CreateResponse(HttpStatusCode.OK, "Done");
    }
}