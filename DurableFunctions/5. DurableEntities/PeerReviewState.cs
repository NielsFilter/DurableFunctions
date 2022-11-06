namespace DurableFunctions.DurableEntities;

public class PeerReviewState
{
    public bool BuildSucceeded { get; set; }
    public bool CommentsResolved { get; set; }
    public int Approvals { get; set; }
    public bool UnitTestPassed { get; set; }
}