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

        public BlogPostImageViewModel Thumbnail { get; set; }
        public BlogPostImageViewModel Banner { get; set; }

        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
    }
}