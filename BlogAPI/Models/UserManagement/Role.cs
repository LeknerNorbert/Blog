namespace BlogAPI.Models.UserManagement
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }   
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanRead { get; set; }
        public ICollection<Account>? Accounts { get; set; }
    }
}
