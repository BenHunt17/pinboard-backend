using FluentValidation;
using Pinboard.Application.Validators;
using Pinboard.Domain.Interfaces.Inputs;
using Pinboard.Domain.Interfaces.Repositories;
using Pinboard.Domain.Interfaces.UseCases;
using Pinboard.Domain.Model;

namespace Pinboard.Application.UseCases
{
    public class NoteUseCases : INoteUseCases
    {
        //TODO - Add author filtering once authentication is implemented
        private readonly IDataContext _dataContext;

        public NoteUseCases(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Note> SearchNotes(NoteSearchInput input)
        {
            return _dataContext.NoteRepository.Search(input);
        }

        public Note AddNote(NoteInput input)
        {
            var note = input.ToDomainModel();

            new NoteValidator().ValidateAndThrow(note);

            return _dataContext.NoteRepository.Create(note);
        }

        public Note UpdateTitle(string id, string title)
        {
            var existingNote = _dataContext.NoteRepository.Get(id);
            existingNote.Title = title;
            new NoteValidator().ValidateAndThrow(existingNote);

            return _dataContext.NoteRepository.UpdateTitle(id, title);
        }

        public Note UpdateContent(string id, string content)
        {
            var existingNote = _dataContext.NoteRepository.Get(id);
            existingNote.Content = content;
            new NoteValidator().ValidateAndThrow(existingNote);

            return _dataContext.NoteRepository.UpdateContent(id, content);
        }

        public void DeleteNotes(IEnumerable<string> ids)
        {
            _dataContext.NoteRepository.DeleteByIds(ids);
        }
    }
}
