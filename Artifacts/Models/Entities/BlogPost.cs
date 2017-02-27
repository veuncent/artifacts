using Artifacts.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Artifacts.Models.Enums;

namespace Artifacts.Models.Entities
{
    public class BlogPost : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }

        public virtual BlogPostImage Thumbnail { get; set; }
        public virtual BlogPostImage BannerImage { get; set; }

        public virtual ICollection<BlogPostTag> Tags { get; set; }

        public void ViewModelToNewEntity(BlogPostViewModel viewModel)
        {

            Id = viewModel.Id;
            Title = viewModel.Title;
            Body = viewModel.Body;
            Thumbnail = new BlogPostImage()
            {
                ImageType = ImageType.BlogPostThumbnail
            };
            BannerImage = new BlogPostImage()
            {
                ImageType = ImageType.BlogPostBanner
            };
            Tags = viewModel.Tags;
        }

        public void ViewModelToExistingEntity(BlogPostViewModel viewModel)
        {
            Id = viewModel.Id;
            Title = viewModel.Title;
            Body = viewModel.Body;
            Tags = viewModel.Tags;
        }
    }
}
