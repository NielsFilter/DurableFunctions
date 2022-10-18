namespace DurableFunctions.DurableEntities;

public class UnitTestsPassed
{
    //TODO: Entity
    // [Function(nameof(UnitTestsPassed))]
    // public static async Task<HttpResponseData> HttpStart(
    //     [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "PR/{prId}/UnitTestsPassed")] HttpRequestMessage req,
    //     int prId,
    //     [DurableClient] IDurableEntityClient entityClient,
    //     ILogger log)
    // {
    //     var entity = new EntityId("PeerReviewAction", prId.ToString());
    //     await entityClient.SignalEntityAsync(entity, "PrAction", PeerReviewActionTypes.UnitTestsPassed);
    //     return req.CreateResponse(HttpStatusCode.OK, "Done");
    // }
}