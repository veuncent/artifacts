using System.ComponentModel.DataAnnotations;

namespace Artifacts.Models.Entities
{
    public class BlogPostTag
    {
        [Key]
        public int Id { get; set; }

        public string Tag { get; set; }
    }
}