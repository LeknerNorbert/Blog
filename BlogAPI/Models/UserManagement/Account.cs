using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.UserManagement
{
    public class Account
    {
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Nickname { get; set; }
        public bool IsArchived { get; set; }
        public ICollection<Blog>? Blogs { get; set; }
        public ICollection<Role>? Roles { get; set; }
    }
}
