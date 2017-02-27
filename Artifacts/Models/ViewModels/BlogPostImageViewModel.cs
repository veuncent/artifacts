using Artifacts.Models.Enums;
using Artifacts.Models.ViewModels;
using Artifacts.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Artifacts.Models.Entities
{
    public class BlogPostImageViewModel
    {
        private readonly IFileStorageService _fileStorageService;

        public BlogPostImageViewModel(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public int Id { get; set; }

        public ImageType ImageType { get; set; }

        public string AzureUrl
        {
            get
            {
                var baseUri = new Uri(_fileStorageService.GetEndpointUri());
                var uriArgs = new object[] {
                    baseUri,
                    _fileStorageService.GetContainerNameByImageType(ImageType).ToString(),
                    Id.ToString()
                };

                return String.Format("{0}/{1}/{2}", uriArgs);
            }
        }
    }
}