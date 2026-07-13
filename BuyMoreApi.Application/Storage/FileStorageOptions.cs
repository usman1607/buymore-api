namespace BuyMoreApi.Application.Storage
{
    /// <summary>
    /// High-level storage configuration plus provider-specific sub-options.
    /// </summary>
    public sealed class FileStorageOptions
    {
        public FileStorageProvider Provider { get; set; } = FileStorageProvider.Local;
        public LocalFileStorageOptions Local { get; set; } = new();
        public AwsS3FileStorageOptions Aws { get; set; } = new();
        public AzureBlobStorageOptions Azure { get; set; } = new();
    }

    public sealed class LocalFileStorageOptions
    {
        public string RootPath { get; set; } = "Storage";
        public bool OverwriteExisting { get; set; }
    }

    public sealed class AwsS3FileStorageOptions
    {
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Region { get; set; } = "us-east-1";
        public string BucketName { get; set; } = string.Empty;
    }

    public sealed class AzureBlobStorageOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
    }
}
