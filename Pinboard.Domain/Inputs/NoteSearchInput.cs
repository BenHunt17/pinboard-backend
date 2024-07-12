namespace Pinboard.Domain.Inputs
{
    public class NoteSearchInput
    {
        public string SearchTerm { get; set; }

        public string? Cursor { get; set; }

        public int Limit { get; set; }
    }
}
