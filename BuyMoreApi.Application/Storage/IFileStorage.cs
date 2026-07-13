using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Storage
{
    /// <summary>
    /// Abstraction over the physical storage mechanism. Each implementation decides how to persist the bytes.
    /// </summary>
    public interface IFileStorage
    {
        Task<string> SaveAsync(FileUploadRequest request, CancellationToken cancellationToken = default);

        Task<Stream?> GetAsync(string path, CancellationToken cancellationToken = default);

        Task DeleteAsync(string path, CancellationToken cancellationToken = default);
    }
}
