using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Issues
{
    public interface IFileStorage
    {
        Task<string> StoreAsync(string tenant, IFormFile file);
        Task Check();
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

        public async Task Check()
        {
            if (!await _blobContainer.ExistsAsync())
                throw new Exception("Blob container does not exist"); // TODO: returns/ensure
        }
    }
}