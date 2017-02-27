using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artifacts.Models.Enums
{
    public static class AzureContainerNameExtensions
    {
        public static string ToString(this AzureContainerName containerName)
        {
            switch (containerName)
            {
                case AzureContainerName.Thumbnails:
                    return "artifacts-blogpost-thumbnails";
                case AzureContainerName.Banners:
                    return "artifacts-blogpost-banners";
                case AzureContainerName.Images:
                    return "artifacts-blogpost-images";
                default:
                    throw new NotImplementedException("AzureContainerName enum not mapped to an official Azure container name");
            };
        }
    }

    public enum AzureContainerName
    {
        Thumbnails,
        Banners,
        Images
    }
}
