using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.DurableEntities;

public class BuildSucceeded
{
    [Function(nameof(BuildSucceeded))]
    public async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "PR/{prId}/BuildSucceeded")] HttpRequestMessage req,
        int prId,
        [DurableClient] DurableClientContext starter,
        ILogger log)
    {
        //TODO: Entity
        throw new NotImplementedException();
        // var entity = new EntityId("PeerReviewAction", prId.ToString());
        // await entityClient.SignalEntityAsync(entity, "PrAction", PeerReviewActionTypes.BuildSucceeded);
        // return req.CreateResponse(HttpStatusCode.OK, "Done");
    }
}