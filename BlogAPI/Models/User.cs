using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Nickname { get; set; }
        public bool IsLocked { get; set; }
        // A jogosultsági szinteket majd egy külön táblába fogom tenni, hogy 
        // ne lehessen még véletlenül se módosítani azt API-on keresztül
    }
}
