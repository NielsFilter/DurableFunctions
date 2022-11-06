using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace DurableFunctions.AsyncHttpApi;

public class BruteForcePasswordFunction
{
    [Function(nameof(BruteForcePassword))]
    public async Task<string> BruteForcePassword([ActivityTrigger] TaskActivityContext context)
    {
        var rnd = new Random();
        var iterations = rnd.Next(5, 100);
        for (var i = 0; i < iterations; i++)
        {
            await Task.Delay(1000);
        }

        var password = GetHashString(Guid.NewGuid().ToString());
        return password;
    }
    
    private static string GetHashString(string inputString)
    {
        var sb = new StringBuilder();
        foreach (var b in GetHash(inputString))
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString().Substring(0, 8);
    }
    
    private static byte[] GetHash(string inputString)
    {
        using HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }
}