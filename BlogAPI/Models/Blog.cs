using BlogAPI.Models.UserManagement;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string? Title { get; set; }
        [StringLength (1000, MinimumLength = 1)]
        public string? Entry { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;
        public bool IsArchived { get; set; } = false;
        public User? Author { get; set; }
    }
}
