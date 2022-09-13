using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DurableFunctions.Chaining;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.AsyncHttpApi;

public class BruteForcePasswordFunction
{
    [FunctionName(nameof(BruteForcePassword))]
    public async Task<string> BruteForcePassword([ActivityTrigger] IDurableActivityContext context, ILogger log)
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