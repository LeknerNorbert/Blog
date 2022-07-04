using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class BlogDto
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string? Title { get; set; }
        [StringLength(10000, MinimumLength = 1)]
        public string? Entry { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
