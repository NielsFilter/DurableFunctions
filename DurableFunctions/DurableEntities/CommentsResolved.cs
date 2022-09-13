using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.DurableEntities;

public class CommentsResolved
{
    [FunctionName("CommentsResolved")]
    public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "PR/{prId}/CommentsResolved")] HttpRequestMessage req,
        int prId,
        [DurableClient] IDurableEntityClient entityClient,
        ILogger log)
    {
        var entity = new EntityId("PeerReviewAction", prId.ToString());
        await entityClient.SignalEntityAsync(entity, "PrAction", PeerReviewActionTypes.CommentsResolved);
        return req.CreateResponse(HttpStatusCode.OK, "Done");
    }
}