namespace Pinboard.Domain.Model
{
    public class Note
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }
    }
}
