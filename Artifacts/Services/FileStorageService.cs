using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace Artifacts.Services
{
    public class FileStorageService
    {
        private CloudStorageAccount _cloudStorageAccount;
        private CloudBlobClient _cloudBlobClient;

        public FileStorageService()
        {
            this._cloudStorageAccount = GetCloudStorageAccount();
            this._cloudBlobClient = GetCloudBlobClient();
        }

        private static CloudStorageAccount GetCloudStorageAccount()
        {
            return CloudStorageAccount.Parse(GetConnectionString());
        }

        private static string GetConnectionString()
        {
            return Environment.ExpandEnvironmentVariables(CloudConfigurationManager.GetSetting("StorageConnectionString"));
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


        public static string GetEndpointUri()
        {
            return GetCloudStorageAccount().FileEndpoint.AbsoluteUri;
        }

        public async Task UploadOrOverwriteFile(string containerName, string fileName, byte[] bytes)
        {
            var blockBlobReference = GetBlockBlobReference(containerName, fileName);
            await blockBlobReference.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
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
    }
}