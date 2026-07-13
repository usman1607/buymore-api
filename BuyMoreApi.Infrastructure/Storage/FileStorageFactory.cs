using System;
using BuyMoreApi.Application.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.Infrastructure.Storage
{
    /// <summary>
    /// Resolves the correct file storage implementation based on configuration.
    /// </summary>
    public sealed class FileStorageFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FileStorageOptions _options;

        public FileStorageFactory(IServiceProvider serviceProvider, IOptions<FileStorageOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public IFileStorage Create()
        {
            return _options.Provider switch
            {
                FileStorageProvider.Local => _serviceProvider.GetRequiredService<LocalFileStorage>(),
                FileStorageProvider.AwsS3 => _serviceProvider.GetRequiredService<AwsS3FileStorage>(),
                FileStorageProvider.AzureBlob => _serviceProvider.GetRequiredService<AzureBlobFileStorage>(),
                _ => throw new InvalidOperationException($"Unsupported file storage provider '{_options.Provider}'.")
            };
        }
    }
}
