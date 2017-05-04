using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Artifacts.Services;
using System.Collections;
using System.Linq;

namespace Artifacts.Tests.Services
{
    [TestClass]
    public class FileStorageServiceTests
    {
        private string _containerName = "artifacts-blogpost-images-test";
        private string _filename = "testSubFolder/testFileName.png";

        [TestMethod]
        public void GetEndpointUriTest()
        {
            Assert.IsNotNull(FileStorageService.GetEndpointUri());
        }

        [TestMethod]
        public void FileStorageServiceConstructorTest()
        {
            var fileStorageService = new FileStorageService();
            Assert.IsNotNull(fileStorageService);
        }

        [TestMethod]
        public async void UploadAndDownloadAndDeleteFileTest()
        {
            var fileStorageService = new FileStorageService();

            // Upload file
            byte[] uploadBytes = new byte[8];
            Random random = new Random();
            random.NextBytes(uploadBytes);
            await fileStorageService.UploadOrOverwriteFile(_containerName, _filename, uploadBytes);

            // Check if file was uploaded
            var blobList = fileStorageService.GetBlobItemListFromContainer(_containerName);
            CollectionAssert.Contains((ICollection)blobList, _filename);

            // Check if content of downloaded file is equal to uploaded file
            var downloadedBytes = fileStorageService.DownloadBlobItem(_containerName, _filename);
            Assert.IsTrue(uploadBytes.SequenceEqual(downloadedBytes));

            // Delete file
            fileStorageService.DeleteBlobItem(_containerName, _filename);
            var blobListAfterDelete = fileStorageService.GetBlobItemListFromContainer(_containerName);
            CollectionAssert.DoesNotContain((ICollection)blobListAfterDelete, _filename);
        }


    }
}
