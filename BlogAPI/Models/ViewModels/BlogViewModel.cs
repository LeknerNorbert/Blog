using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string? Title { get; set; }
        [StringLength(10000, MinimumLength = 1)]
        public string? Entry { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsArchived { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public string? AuthorNickname { get; set; }
    }
}
