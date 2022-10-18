using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.RequestApproval;

public class ApproveRsvpFunction
{
    private readonly ILogger _logger;

    public ApproveRsvpFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function(nameof(RsvpHttp))]
    public async Task<HttpResponseData> RsvpHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RsvpHttp/{invitationId}/{isAccepted}")] HttpRequestMessage req,
        string invitationId,
        bool isAccepted,
        [DurableClient] DurableClientContext client)
    {
        //TODO: Raise events
        throw new NotImplementedException();
        // await client.RaiseEventAsync(invitationId, "RsvpReceived", isAccepted);
        // return req.CreateResponse(HttpStatusCode.OK, "Done");
    }
}