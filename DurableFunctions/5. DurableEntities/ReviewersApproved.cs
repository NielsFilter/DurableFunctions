using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.DurableEntities;

public class ReviewersApproved
{
    [Function(nameof(ReviewersApproved))]
    public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "PR/{prId}/ReviewersApproved")] HttpRequestMessage req,
        int prId,
        //TODO: [DurableClient] IDurableEntityClient entityClient,
        ILogger log)
    {
        //TODO: Entity
        throw new NotImplementedException();
        // var entity = new EntityId("PeerReviewAction", prId.ToString());
        // await entityClient.SignalEntityAsync(entity, "PrAction", PeerReviewActionTypes.ReviewersApproved);
        // return req.CreateResponse(HttpStatusCode.OK, "Done");
    }
}