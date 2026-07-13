using System.IO;
using System.Threading;
using BuyMoreApi.Application.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuyMoreApi.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;

        public FilesController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
        {
            if (file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            await using var stream = file.OpenReadStream();

            var path = await _fileStorage.SaveAsync(new FileUploadRequest
            {
                Content = stream,
                FileName = file.FileName,
                ContentType = file.ContentType
            }, cancellationToken);

            return Ok(new { Path = path });
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string path, CancellationToken cancellationToken)
        {
            var stream = await _fileStorage.GetAsync(path, cancellationToken);
            if (stream is null)
            {
                return NotFound();
            }

            return File(stream, "application/octet-stream", Path.GetFileName(path));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string path, CancellationToken cancellationToken)
        {
            await _fileStorage.DeleteAsync(path, cancellationToken);
            return NoContent();
        }
    }
}
