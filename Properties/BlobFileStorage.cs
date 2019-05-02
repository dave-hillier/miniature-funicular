using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Properties
{
    public interface IFileStorage
    {
        Task<string> StoreAsync(string tenant, IFormFile file);
        Task<bool> ExistsAsync();
    }

    public class FileStorageHealthCheck : IHealthCheck
    {
        private readonly IFileStorage _fileStorage;

        public FileStorageHealthCheck(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var blobExists = await _fileStorage.ExistsAsync();
            return blobExists ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy("Images Blob does not exist");
        }
    }
    
    public class BlobFileStorageConfig
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
    
    public class BlobFileStorage : IFileStorage
    {
        public BlobFileStorage(IOptions<BlobFileStorageConfig> options)
        {
            CloudStorageAccount.TryParse(options.Value.ConnectionString, out var storageAccount);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = cloudBlobClient.GetContainerReference(options.Value.ContainerName);
        }

        private readonly CloudBlobContainer _blobContainer;
        public async Task<string> StoreAsync(string tenant, IFormFile file)
        {
            var uploadedFileName = $"{tenant}/{Guid.NewGuid()}/{file.FileName}";
            var blob = _blobContainer.GetBlockBlobReference(uploadedFileName);

            await blob.UploadFromStreamAsync(file.OpenReadStream());

            return $"{blob.Uri}";
        }

        public async Task<bool> ExistsAsync()
        {
            return await _blobContainer.ExistsAsync();
        }
    }
}