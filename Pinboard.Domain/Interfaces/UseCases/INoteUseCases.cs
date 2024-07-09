using Pinboard.Domain.Interfaces.Inputs;
using Pinboard.Domain.Model;

namespace Pinboard.Domain.Interfaces.UseCases
{
    public interface INoteUseCases
    {
        PaginatedItems<Note> SearchNotes(NoteSearchInput input);

        Note AddNote(NoteInput input);

        Note UpdateTitle(string id, string title);
        
        Note UpdateContent(string id, string content);

        void DeleteNotes(IEnumerable<string> ids);
    }
}
