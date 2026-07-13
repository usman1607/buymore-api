using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BuyMoreApi.Application.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.Infrastructure.Storage
{
    /// <summary>
    /// Writes files to Azure Blob Storage using the official SDK.
    /// </summary>
    public sealed class AzureBlobFileStorage : IFileStorage
    {
        private readonly AzureBlobStorageOptions _options;
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<AzureBlobFileStorage> _logger;

        public AzureBlobFileStorage(IOptions<FileStorageOptions> options, ILogger<AzureBlobFileStorage> logger)
        {
            _options = options.Value.Azure;
            _logger = logger;

            _containerClient = new BlobContainerClient(_options.ConnectionString, _options.ContainerName);
            _containerClient.CreateIfNotExists(PublicAccessType.None);
        }

        public async Task<string> SaveAsync(FileUploadRequest request, CancellationToken cancellationToken = default)
        {
            var blobClient = _containerClient.GetBlobClient(request.FileName);
            await blobClient.UploadAsync(request.Content, overwrite: true, cancellationToken);

            _logger.LogInformation("Uploaded file {File} to Azure blob container {Container}", request.FileName, _options.ContainerName);
            return blobClient.Uri.ToString();
        }

        public async Task<Stream?> GetAsync(string path, CancellationToken cancellationToken = default)
        {
            var blobClient = new BlobClient(new Uri(path));

            if (!await blobClient.ExistsAsync(cancellationToken))
            {
                _logger.LogWarning("Blob {File} not found in container {Container}", blobClient.Name, blobClient.BlobContainerName);
                return null;
            }

            var response = await blobClient.DownloadContentAsync(cancellationToken);
            return new MemoryStream(response.Value.Content.ToArray());
        }

        public async Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            var blobClient = new BlobClient(new Uri(path));
            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
            _logger.LogInformation("Deleted blob {File} from container {Container}", blobClient.Name, blobClient.BlobContainerName);
        }
    }
}
