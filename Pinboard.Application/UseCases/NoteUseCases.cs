using FluentValidation;
using Pinboard.Application.Validators;
using Pinboard.Domain.Inputs;
using Pinboard.Domain.Interfaces;
using Pinboard.Domain.Interfaces.Repositories;
using Pinboard.Domain.Interfaces.UseCases;
using Pinboard.Domain.Model;

namespace Pinboard.Application.UseCases
{
    public class NoteUseCases : INoteUseCases
    {
        private readonly IDataContext _dataContext;
        private readonly IUserState _userState;

        public NoteUseCases(IDataContext dataContext, IUserState userState)
        {
            _dataContext = dataContext;
            _userState = userState;
        }

        public PaginatedItems<Note> SearchNotes(NoteSearchInput input)
        {
            new NoteSearchInputValidator().ValidateAndThrow(input);

            return _dataContext.NoteRepository.Search(input, _userState.Identifier);
        }

        public Note AddNote(NoteInput input)
        {
            var note = input.ToDomainModel(_userState.Identifier);

            new NoteValidator().ValidateAndThrow(note);

            return _dataContext.NoteRepository.Create(note);
        }

        public Note UpdateTitle(string id, string title)
        {
            var existingNote = _dataContext.NoteRepository.Get(id);

            if (existingNote.AuthorId != _userState.Identifier)
            {
                throw new UnauthorizedAccessException("User is not authorized to modify another user's note");
            }

            existingNote.Title = title;
            new NoteValidator().ValidateAndThrow(existingNote);

            return _dataContext.NoteRepository.UpdateTitle(id, title);
        }

        public Note UpdateContent(string id, string content)
        {
            var existingNote = _dataContext.NoteRepository.Get(id);
            existingNote.Content = content;
            new NoteValidator().ValidateAndThrow(existingNote);

            if (existingNote.AuthorId != _userState.Identifier)
            {
                throw new UnauthorizedAccessException("User is not authorized to modify another user's note");
            }

            return _dataContext.NoteRepository.UpdateContent(id, content);
        }

        public void DeleteNotes(IEnumerable<string> ids)
        {
            var notes = _dataContext.NoteRepository.GetByIds(ids);

            if (notes.Any(x => x.AuthorId != _userState.Identifier))
            {
                throw new UnauthorizedAccessException("User is not authorized to delete another user's note");
            }

            _dataContext.NoteRepository.DeleteByIds(ids);
        }
    }
}
