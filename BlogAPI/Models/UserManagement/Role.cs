namespace BlogAPI.Models.UserManagement
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        // Ide lehetne felsorolni a különböző jogosultságokat
        public ICollection<User>? Users { get; set; }
    }
}
