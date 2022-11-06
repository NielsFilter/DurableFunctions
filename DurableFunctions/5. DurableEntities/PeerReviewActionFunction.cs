namespace DurableFunctions.DurableEntities;

public class PeerReviewActionFunction
{
    //TODO: Entity
    // [Function(nameof(PeerReviewAction))]
    // public static void PeerReviewAction([EntityTrigger] IDurableEntityContext ctx)
    // {
    //     var prState = ctx.GetState(() => new PeerReviewState());
    //     var actionType = ctx.GetInput<PeerReviewActionTypes>();
    //     switch (actionType)
    //     {
    //         case PeerReviewActionTypes.BuildSucceeded:
    //             prState.BuildSucceeded = true;
    //             ctx.SetState(prState);
    //             break;
    //         case PeerReviewActionTypes.CommentsResolved:
    //             prState.CommentsResolved = true;
    //             break;
    //         case PeerReviewActionTypes.ReviewersApproved:
    //             prState.Approvals++;
    //             break;
    //         case PeerReviewActionTypes.UnitTestsPassed:
    //             prState.UnitTestPassed = true;
    //             break;
    //     }
    //
    //     ctx.SetState(prState);
    //     
    //     if (prState.BuildSucceeded &&
    //         prState.CommentsResolved &&
    //         prState.UnitTestPassed &&
    //         prState.Approvals >= 2)
    //     {
    //         // Complete PR
    //         Console.WriteLine("PR Completed");
    //         ctx.DeleteState();
    //     }
    // }
}