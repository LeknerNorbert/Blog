namespace BlogAPI.Models.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Entry { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsArchived { get; set; }
        public string? AuthorNickname { get; set; }
    }
}
