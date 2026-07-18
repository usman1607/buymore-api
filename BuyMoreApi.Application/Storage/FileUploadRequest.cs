using System.IO;

namespace BuyMoreApi.Application.Storage
{
    /// <summary>
    /// Describes an upload so every storage provider can store the bits consistently.
    /// </summary>
    public sealed class FileUploadRequest
    {
        public required Stream Content { get; init; }
        public required string FileName { get; init; }
        public string? Folder { get; set;}
        public string ContentType { get; init; } = "application/octet-stream";
    }
}
