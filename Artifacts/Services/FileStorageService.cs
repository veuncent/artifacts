using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using Microsoft.Extensions.Configuration;
using Artifacts.Models.Entities;
using Artifacts.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace Artifacts.Services
{
    public interface IFileStorageService
    {
        void DeleteBlobItem(string containerName, string fileName);
        byte[] DownloadBlobItem(string containerName, string fileName);
        IEnumerable GetBlobItemListFromContainer(string containerName);
        string GetEndpointUri();
        Task<IEnumerable<IListBlobItem>> GetListOfBlobsInContainer(string containerName);
        Task UploadOrOverwriteFile(string containerName, string fileName, byte[] bytes);
        AzureContainerName GetContainerNameByImageType(ImageType imageType);
        Task UploadOrOverwriteImage(BlogPostImage image, IFormFile file);
    }

    public class FileStorageService : IFileStorageService
    {
        private CloudStorageAccount _cloudStorageAccount;
        private CloudBlobClient _cloudBlobClient;
        private readonly IConfiguration _config;

        public FileStorageService(IConfiguration config)
        {
            _config = config;
            this._cloudStorageAccount = GetCloudStorageAccount();
            this._cloudBlobClient = GetCloudBlobClient();
        }

        private CloudStorageAccount GetCloudStorageAccount()
        {
            return CloudStorageAccount.Parse(GetConnectionString());
        }

        private string GetConnectionString()
        {
            return Environment.ExpandEnvironmentVariables(_config["AzureStorageConfig:StorageConnectionString"]);
        }

        private CloudBlobClient GetCloudBlobClient()
        {
            return _cloudStorageAccount.CreateCloudBlobClient();
        }

        private CloudBlobContainer GetCloudBlobContainerReference(string containerName)
        {
            return _cloudBlobClient.GetContainerReference(containerName);
        }

        async public Task<IEnumerable<IListBlobItem>> GetListOfBlobsInContainer(string containerName)
        {
            var container = GetCloudBlobContainerReference(containerName);

            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;

            resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, null, continuationToken, null, null);

            return resultSegment.Results;
        }


        private CloudBlockBlob GetBlockBlobReference(string containerName, string fileName)
        {
            var container = GetCloudBlobContainerReference(containerName);
            return container.GetBlockBlobReference(fileName);
        }


        public string GetEndpointUri()
        {
            return GetCloudStorageAccount().FileEndpoint.AbsoluteUri;
        }

        public async Task UploadOrOverwriteFile(string containerName, string fileName, byte[] bytes)
        {
            var blockBlobReference = GetBlockBlobReference(containerName, fileName);
            await blockBlobReference.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
        }

        public async Task UploadOrOverwriteImage(BlogPostImage image, IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                var azureContainerName = GetContainerNameByImageType(image.ImageType).ToString();
                await UploadOrOverwriteFile(azureContainerName, image.Id.ToString(), memoryStream.ToArray());
            }
        }

        public IEnumerable GetBlobItemListFromContainer(string containerName)
        {
            var task = GetListOfBlobsInContainer(containerName);
            task.Wait();
            return task.Result
                .OfType<CloudBlockBlob>()
                .Select(x => x.Name)
                .ToList();
        }

        public byte[] DownloadBlobItem(string containerName, string fileName)
        {
            var blockBlob = GetBlockBlobReference(containerName, fileName);
            blockBlob.FetchAttributesAsync();

            byte[] bytes = new byte[blockBlob.Properties.Length];
            blockBlob.DownloadToByteArrayAsync(bytes, 0);

            return bytes;
        }

        public void DeleteBlobItem(string containerName, string fileName)
        {
            var blockBlob = GetBlockBlobReference(containerName, fileName);
            blockBlob.DeleteIfExistsAsync();
        }

        public AzureContainerName GetContainerNameByImageType(ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.BlogPostThumbnail:
                    return AzureContainerName.Thumbnails;
                case ImageType.BlogPostBanner:
                    return AzureContainerName.Banners;
                case ImageType.BlogPostBodyImage:
                    return AzureContainerName.Images;
                default:
                    throw new NotImplementedException("ImageType not mapped to Azure container");
            };
        }
    }
}