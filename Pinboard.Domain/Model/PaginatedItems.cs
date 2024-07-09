namespace Pinboard.Domain.Model
{
    public class PaginatedItems<T>
    {
        public IEnumerable<T> Items { get; set; }

        public string? NextCursor { get; set; }
    }
}
