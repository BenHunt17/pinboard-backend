using Pinboard.Domain.Interfaces.Inputs;
using Pinboard.Domain.Model;

namespace Pinboard.Domain.Interfaces.Repositories
{
    public interface INoteRepository
    {
        Note Get(string id);

        IEnumerable<Note> GetAll();

        IEnumerable<Note> Search(NoteSearchInput input);

        Note Create(Note note);

        Note UpdateTitle(string id, string title);

        Note UpdateContent(string id, string content);

        void DeleteByIds(IEnumerable<string> ids);
    }
}
