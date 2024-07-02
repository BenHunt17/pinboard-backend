using Pinboard.Domain.Model;

namespace Pinboard.Domain.Interfaces.Inputs
{
    public class NoteInput
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Note ToDomainModel()
        {
            return new Note
            {
                Title = Title,
                Content = Content
            };
        }
    }
}
