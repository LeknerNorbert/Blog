using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.UserManagement
{
    public class AccountDto
    {
        [MinLength(1)]
        public string? Username { get; set; }
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
