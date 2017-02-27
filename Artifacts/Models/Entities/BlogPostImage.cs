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
    public class BlogPostImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ImageType ImageType { get; set; }
    }
}