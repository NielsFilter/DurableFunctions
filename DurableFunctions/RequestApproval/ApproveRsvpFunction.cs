using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public class ApproveRsvpFunction
{
    [FunctionName(nameof(RsvpHttp))]
    public static async Task<HttpResponseMessage> RsvpHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RsvpHttp/{invitationId}/{isAccepted}")] HttpRequestMessage req,
        string invitationId,
        bool isAccepted,
        [DurableClient] IDurableOrchestrationClient client)
    {
        await client.RaiseEventAsync(invitationId, "RsvpReceived", isAccepted);
        return req.CreateResponse(HttpStatusCode.OK, "Done");
    }
}