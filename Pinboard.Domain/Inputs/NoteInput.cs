using Pinboard.Domain.Model;

namespace Pinboard.Domain.Inputs
{
    public class NoteInput
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Note ToDomainModel(string authorId)
        {
            return new Note
            {
                Title = Title,
                Content = Content,
                AuthorId = authorId
            };
        }
    }
}
