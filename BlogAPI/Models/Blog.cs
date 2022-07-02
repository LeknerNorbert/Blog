using BlogAPI.Models.UserManagement;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string? Title { get; set; }
        [StringLength (10000, MinimumLength = 1)]
        public string? Entry { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;
        public bool IsArchived { get; set; } = false;
        public Account? Author { get; set; }
    }
}
