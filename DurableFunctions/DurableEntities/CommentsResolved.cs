using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.DurableEntities;

public class CommentsResolved
{
    [Function(nameof(CommentsResolved))]
    public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "PR/{prId}/CommentsResolved")] HttpRequestMessage req,
        int prId,
        //TODO: [DurableClient] IDurableEntityClient entityClient,
        ILogger log)
    {
        //TODO: Entity
        throw new NotImplementedException();
        // var entity = new EntityId("PeerReviewAction", prId.ToString());
        // await entityClient.SignalEntityAsync(entity, "PrAction", PeerReviewActionTypes.CommentsResolved);
        // return req.CreateResponse(HttpStatusCode.OK, "Done");
    }
}