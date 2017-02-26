using Artifacts.Models.ViewModels;
using Artifacts.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Artifacts.Models.Entities
{
    public class BlogPostImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ImageType ImageType { get; set; }

        // Save Azure image path to db for speed
        private string _azureUrl { get; set; }
        public string AzureUrl
        {
            get
            {
                return _azureUrl;
            }
            set
            {
                var baseUri = new Uri(FileStorageService.GetEndpointUri());
                var uriArgs = new object[] {
                    baseUri,
                    AzureContainerName,
                    Id.ToString()
                };

                _azureUrl = String.Format("{0}/{1}/{2}", uriArgs);
            }
        }

        [NotMapped]
        public string AzureContainerName
        {
            get
            {
                switch (ImageType)
                {
                    case ImageType.BlogPostThumbnail:
                        return "artifacts-blogpost-thumbnails";
                    case ImageType.BlogPostBanner:
                        return "artifacts-blogpost-banners";
                    case ImageType.BlogPostBodyImage:
                        return "artifacts-blogpost-images";
                    default:
                        throw new NotImplementedException("ImageType not mapped to Azure container");
                };
            }
        }

        public async Task SaveImageAsync(byte[] bytes)
        {
            var fileStorageService = new FileStorageService();
            await fileStorageService.UploadOrOverwriteFile(AzureContainerName, Id.ToString(), bytes);
        }
    }

    public enum ImageType
    {
        BlogPostThumbnail = 0,
        BlogPostBanner = 1,
        BlogPostBodyImage = 2
    }

}