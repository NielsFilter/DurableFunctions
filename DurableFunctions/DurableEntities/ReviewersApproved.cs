using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.DurableEntities;

public class ReviewersApproved
{
    [FunctionName("ReviewersApproved")]
    public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "PR/{prId}/ReviewersApproved")] HttpRequestMessage req,
        int prId,
        [DurableClient] IDurableEntityClient entityClient,
        ILogger log)
    {
        var entity = new EntityId("PeerReviewAction", prId.ToString());
        await entityClient.SignalEntityAsync(entity, "PrAction", PeerReviewActionTypes.ReviewersApproved);
        return req.CreateResponse(HttpStatusCode.OK, "Done");
    }
}