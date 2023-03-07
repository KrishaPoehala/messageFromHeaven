using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using reenbit.Application.Interfaces;

namespace reenbit.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileLoader _fileLoader;
        public FilesController(IFileLoader fileLoader)
        {
            _fileLoader = fileLoader;
        }


        [HttpPost]
        [Route("upload/{email}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(string email,CancellationToken cancellationToken = default)
        {
            var files = await Request.ReadFormAsync(cancellationToken);
            if (!files.Files.Any())
            {
                return NotFound();
            }

            var file = files.Files[0];
            await _fileLoader.LoadFile(email, file, cancellationToken);
            return Ok();
        }
    }
}