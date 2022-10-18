namespace DurableFunctions;

public class SomeService
{
    public async Task<string> SomeAsyncCall()
    {
        var httpClient = new HttpClient();
        var result = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
        return await result.Content.ReadAsStringAsync();
    }
}