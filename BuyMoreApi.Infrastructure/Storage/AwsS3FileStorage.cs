using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BuyMoreApi.Application.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.Infrastructure.Storage
{
    /// <summary>
    /// Persists files to an AWS S3 bucket. Requires the AWSSDK.S3 package and credentials in configuration.
    /// </summary>
    public sealed class AwsS3FileStorage : IFileStorage
    {
        private readonly AwsS3FileStorageOptions _options;
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<AwsS3FileStorage> _logger;

        public AwsS3FileStorage(IOptions<FileStorageOptions> options, ILogger<AwsS3FileStorage> logger)
        {
            _options = options.Value.Aws;
            _logger = logger;

            var region = RegionEndpoint.GetBySystemName(_options.Region);
            _s3Client = new AmazonS3Client(_options.AccessKey, _options.SecretKey, region);
        }

        public async Task<string> SaveAsync(FileUploadRequest request, CancellationToken cancellationToken = default)
        {
            using var stream = request.Content;

            var putRequest = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = request.FileName,
                InputStream = stream,
                ContentType = request.ContentType
            };

            await _s3Client.PutObjectAsync(putRequest, cancellationToken);
            _logger.LogInformation("Uploaded file {File} to S3 bucket {Bucket}", request.FileName, _options.BucketName);

            return $"s3://{_options.BucketName}/{request.FileName}";
        }

        public async Task<Stream?> GetAsync(string path, CancellationToken cancellationToken = default)
        {
            var key = ExtractKey(path);

            try
            {
                var response = await _s3Client.GetObjectAsync(_options.BucketName, key, cancellationToken);
                _logger.LogInformation("Downloaded file {File} from S3 bucket {Bucket}", key, _options.BucketName);
                return response.ResponseStream;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("File {File} not found in S3 bucket {Bucket}", key, _options.BucketName);
                return null;
            }
        }

        public async Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            var key = ExtractKey(path);
            await _s3Client.DeleteObjectAsync(_options.BucketName, key, cancellationToken);
            _logger.LogInformation("Deleted file {File} from S3 bucket {Bucket}", key, _options.BucketName);
        }

        private string ExtractKey(string path)
        {
            return path.StartsWith("s3://") ? path.Substring($"s3://{_options.BucketName}/".Length) : path;
        }
    }
}
