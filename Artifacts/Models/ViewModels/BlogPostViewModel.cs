using Artifacts.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artifacts.Models.ViewModels
{
    public class BlogPostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public ICollection<BlogPostTag> Tags { get; set; }

        public byte[] ThumbnailBytesUpload { get; set; }
        public string ThumbnailUrl { get; set; }

        public byte[] BannerBytesUpload { get; set; }
        public string BannerUrl { get; set; }

        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
    }
}