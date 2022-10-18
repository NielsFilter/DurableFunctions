using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Company.FunctionApp1.FanOutFanIn;

public interface IAzureStorageProvider
{
    Task<Uri> UploadBlobFromStreamAsync(Stream stream, string blobName, string containerName);
}

public class AzureStorageProvider : IAzureStorageProvider
{
    private readonly IConfiguration _config;

    public AzureStorageProvider(IConfiguration config)
    {
        _config = config;
    }

    private BlobContainerClient CreateContainerClient(string containerName)
    {
        return new BlobContainerClient(_config["storageConnectionString"], containerName);
    }
    
    private async Task<BlobContainerClient> CreateOrGetContainerAsync(string containerName)
    {
        if (string.IsNullOrWhiteSpace(containerName))
        {
            throw new ArgumentNullException(nameof(containerName));
        }

        var containerClient = CreateContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

        return containerClient;
    }

    private async Task<BlobClient> GetBlobClientAsync(string blobName, string containerName)
    {
        var container = await CreateOrGetContainerAsync(containerName);
        var blobClient = container.GetBlobClient(blobName);
        return blobClient;
    }
    
    public async Task<Uri> UploadBlobFromStreamAsync(Stream stream, string blobName, string containerName)
    {
        var blob = await GetBlobClientAsync(blobName, containerName);
        await blob.UploadAsync(stream, overwrite: true);
        return blob.Uri;
    }
}