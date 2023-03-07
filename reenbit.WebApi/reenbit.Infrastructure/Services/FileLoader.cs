using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using reenbit.Application.Interfaces;
using reenbit.WebApi.Options;

namespace reenbit.Infrastructure.Services;

public class FileLoader : IFileLoader
{
    private readonly FileUploadOptions _options;

    public FileLoader(IOptions<FileUploadOptions> options)
    {
        _options = options.Value;
    }

    public async Task LoadFile(string email, IFormFile file, CancellationToken cancellationToken = default)
    {
        var blobName = file.FileName;
        var client = new BlobServiceClient(_options.ConnectionString);
        var containerClient = client.GetBlobContainerClient(_options.ContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var metadata = new Dictionary<string, string>
        {
           { "email", email }
        };

        await blobClient.UploadAsync(file.OpenReadStream(), true, cancellationToken);
        await blobClient.SetMetadataAsync(metadata, null, cancellationToken);
    }
}
