namespace Microsoft.DurableTask;

public class InviteFriendRequest
{
    public string? Friend { get; set; }
    public int ReminderCount { get; set; }
}