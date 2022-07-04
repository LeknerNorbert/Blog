using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.UserManagement
{
    public class Account
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public bool IsArchived { get; set; } = false;
        public ICollection<Blog>? Blogs { get; set; }
        public Role? Role { get; set; }
    }
}
