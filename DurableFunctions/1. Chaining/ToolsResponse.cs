namespace Microsoft.DurableTask;

public class ToolsResponse
{
    public ToolsResponse()
    {
        MyTools = new List<string>();
        MyFriendsTools = new List<string>();
    }

    public List<string> MyTools { get; set; }
    public List<string> MyFriendsTools { get; set; }
}