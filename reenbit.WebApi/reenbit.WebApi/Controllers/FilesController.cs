using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using reenbit.WebApi.Options;

namespace reenbit.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly FileUploadOptions _options;
        public FilesController(IOptions<FileUploadOptions> options)
        {
            _options = options.Value;
        }


        [HttpPost]
        [Route("upload/{email}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(string email,CancellationToken cancellationToken = default)
        {
            var files = await Request.ReadFormAsync(cancellationToken);
            var file = files.Files[0];
            var blobName = file.FileName;
            var client = new BlobServiceClient(_options.ConnectionString);
            var containerClient = client.GetBlobContainerClient(_options.ContainerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var metadata = new Dictionary<string, string>
            {
                { "email", email }
            };

            await blobClient.UploadAsync(file.OpenReadStream(), true,cancellationToken);
            await blobClient.SetMetadataAsync(metadata,null, cancellationToken);
            return Ok();
        }
    }
}